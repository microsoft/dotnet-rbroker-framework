/*
 * PooledTaskBroker.cs
 *
 * Copyright (C) 2010-2014 by Revolution Analytics Inc.
 *
 * This program is licensed to you under the terms of Version 2.0 of the
 * Apache License. This program is distributed WITHOUT
 * ANY EXPRESS OR IMPLIED WARRANTY, INCLUDING THOSE OF NON-INFRINGEMENT,
 * MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. Please refer to the
 * Apache License 2.0 (http://www.apache.org/licenses/LICENSE-2.0) for more details.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DeployR;

namespace DeployRBroker
{
    /// <summary>
    /// Represents a Pooled Broker implementation
    /// </summary>
    /// <remarks></remarks>
    public class PooledTaskBroker : RBrokerEngine
    {

        /// <summary>
        /// Constructor for specifying a Pooled Instance of RBroker
        /// </summary>
        /// <param name="brokerConfig">Pooled Broker Configuration object</param>
        /// <remarks></remarks>
        public PooledTaskBroker(PooledBrokerConfig brokerConfig)
            :base ((RBrokerConfig) brokerConfig)
        {

            m_rClient = RClientFactory.createClient(brokerConfig.deployrEndpoint, brokerConfig.maxConcurrentTaskLimit);

            m_rUser = m_rClient.login(brokerConfig.userCredentials);

            if (brokerConfig.poolCreationOptions != null)
            {
                if (brokerConfig.poolCreationOptions.releaseGridResources == true)
                {
                    m_rUser.releaseProjects();
                }
            }

            ProjectCreationOptions options = ROptionsTranslator.translate(brokerConfig.poolCreationOptions);

            List<RProject> deployrProjectPool = m_rUser.createProjectPool(brokerConfig.maxConcurrentTaskLimit, options);

            /*
             * Prep the base RBrokerEngine.
             */

            initEngine(deployrProjectPool.Count());

            /*
             * Initialize the resourceTokenPool with RProject.
             */
            foreach(RProject rProject in deployrProjectPool) 
            {
                m_resourceTokenPool.TryAdd(rProject);
            }

            try 
            {
                Task.Factory.StartNew(() => HTTPKeepAliveManager(m_rUser));
            } 
            catch(Exception rex) 
            {
                shutdown();
                throw new Exception("Broker failed to start HTTP keep-alive manager, cause: " + rex.ToString());
            }

        }

        /// <summary>
        /// Implementation of the refresh method on RBroker interface
        /// /// </summary>
        /// <param name="config">Pooled Broker Configuration object</param>
        /// <remarks></remarks>
        public new void refresh(RBrokerConfig config) 
        {

            if(!status().isIdle)
            {
                throw new Exception("RBroker is not idle, refresh not permitted.");
            }

            if(!(config is PooledBrokerConfig)) 
            {
                throw new Exception("PooledTaskBroker refresh requires PooledBrokerConfig.");
            }

            PooledBrokerConfig pooledConfig = (PooledBrokerConfig) config;

            try
            {
                /*
                 * Temporarily disable RBroker to permit
                 * configuration refresh.
                 */
                Interlocked.Exchange(ref m_refreshingConfig, 1);

                ProjectExecutionOptions options =  ROptionsTranslator.migrate(pooledConfig.poolCreationOptions);

                foreach(Object resourceToken in m_resourceTokenPool) 
                {
                    RProject rProject = (RProject) resourceToken;
                    /*
                     * Recycle project to remove all existing
                     * workspace objects and directory files.
                     */
                    rProject.recycle();
                    /*
                     * Execute code to cause workspace and directory
                     * preloads and adoptions to take place.
                     */
                    rProject.executeCode("# Refresh project on PooledTaskBroker.", options);
                }

            } 
            catch(Exception rex) 
            {
                throw new Exception("RBroker refresh failed with unexpected error=" + rex.ToString());
            } 
            finally 
            {
                /*
                 * Re-enabled RBroker following
                 * configuration refresh.
                 */
                Interlocked.Exchange(ref m_refreshingConfig, 0);
            }
        }

        /// <summary>
        /// Returns a resource token for the task back to the token pool.  
        /// </summary>
        /// <param name="task">RTask submitted for execution as a background task</param>
        /// <param name="result">RTaskResult containing the results of the completed task</param>
        /// <remarks></remarks>
        public override void callback(RTask task, RTaskResult result) 
        {

            Object obj;
            m_taskResourceTokenMap.TryGetValue(task, out obj);
            RProject rProject = (RProject)obj;

            Exception failure = result.getFailure();
            if (failure != null) 
            { 
                /*
                * On detection of an RGridException drop the RProject from
                * the pool so further tasks are not directed to that RProject.
                * We achieve this by simply not adding the RProject back to the
                * resourceTokenPool on this callback.
                *
                * We then need to adjust the parallelTaskLimit so the RBroker
                * will report the new (smaller) pool size on
                * RBroker.maxConcurrency() calls.
                */
  
                if (m_taskListener != null) 
                {
                    /*
                    * When asynchronous listener in use, failed task 
                    * executions due to slot or grid failures can be
                    * automatically resubmitted for execution by the RBroker.
                    *
                    * When RTaskResult.repeatTask is enabled the 
                    * RBrokerEngine.RBrokerListenerManager will skip
                    * calling taskListener.onTaskCompleted(task, result).
                    * This prevents a client application from seeing 
                    * (or having to handle) temporary slot or grid related
                    * failures on RTasks.
                    */
                    RTaskResultImpl resultImpl = (RTaskResultImpl) result;
                    resultImpl.repeatTask = true;
  
                    /*
                    * Now re-submit for execution using the priority
                    * queue to expedite processing.
                    */
  
                    try 
                    {
                        submit(task, true);
                    } 
                    catch (Exception tex) 
                    {
                        throw new Exception("PooledTaskBroker: callback, task re-submission ex=" + tex.ToString());
                    }
                }


                int resizedPoolSize = (int)Interlocked.Decrement(ref m_parallelTaskLimit);
                if (m_brokerListener != null) 
                {
                    Exception rbex;
                    if (resizedPoolSize == 0) 
                    {
                        rbex = new Exception("DeployR grid failure detected, pool no longer operational, advise RBroker shutdown.");
                        
                    } 
                    else 
                    {
                        rbex = new Exception("DeployR grid failure detected, pool size auto-adjusted, max concurrency now " + resizedPoolSize  + ".");
                    }
                    m_brokerListener.onRuntimeError(rbex.Message);
                }
            }
            else
            {
                if(rProject != null) 
                {
                    Boolean added = m_resourceTokenPool.TryAdd(rProject);

                    if(!added) 
                    {
                        throw new Exception("PooledTaskBroker: callback, project could not be added back to pool?");
                    }

                } 
                else 
                {
                    throw new Exception("PooledTaskBroker: callback, task does not have matching project?");
                }
            }
        }

        /// <summary>
        /// createBrokerWorker override method for PooledTaskBroker.
        /// </summary>
        /// <remarks></remarks>
        protected override RBrokerWorker createBrokerWorker(RTask task,
                                                   long taskIndex,
                                                   Boolean isPriorityTask,
                                                   Object resourceToken,
                                                   RBrokerEngine brokerEngine) 
        {

            return new PooledTaskWorker((PooledTask) task,
                                        taskIndex,
                                        isPriorityTask,
                                        (RProject) resourceToken,
                                        brokerEngine);
        }

        /// <summary>
        /// cloneTask override method for PooledTaskBroker.
        /// </summary>
        /// <remarks></remarks>
        protected override RTask cloneTask(RTask genesis) 
        {

            PooledTask source  = (PooledTask) genesis;
            PooledTask clone = null;
            if(source.code != "") 
            {
                clone = new PooledTask(source.code,
                                       source.options);
            } 
            else 
            {
                clone = new PooledTask(source.filename,
                                       source.directory,
                                       source.author,
                                       source.version,
                                       source.options);
            }

            if(source.external != "") 
            {
                clone.external = source.external;
            }
            clone.setToken(source.getToken());
            return clone;
        }

        /*
         * HTTPKeepAliveManager
         *
         * Prevents authenticated HTTP session from timing out
         * due to inactivity to ensure pool of RProject remain
         * live and available to PooledTaskBroker.
         */
        private void HTTPKeepAliveManager(RUser rUser)  
        {

            int PING_INTERVAL = 60000;

            try 
            {
                while (Interlocked.Read(ref m_taskBrokerIsActive) == 1) 
                {
                    try 
                    {
                        if(rUser != null) 
                        {
                            /* No-Op Ping Authenticated HTTP Session.*/
                            rUser.autosaveProjects(false);
                        }

                        try 
                        {
                            Thread.Sleep(PING_INTERVAL);
                        } 
                        catch(Exception iex) 
                        {
                            throw new Exception("Sleep Exception= " + iex.ToString());
                        }
                    } 
                    catch(Exception ex) 
                    {
                        throw new Exception("PooledTaskBroker: HTTPKeepAliveManager ex=" + ex.ToString());
                    }
                }

            } 
            catch(Exception rex) 
            {
                throw new Exception("PooledTaskBroker: HTTPKeepAliveManager rex=" + rex.ToString());
            }
        }

    }
}
