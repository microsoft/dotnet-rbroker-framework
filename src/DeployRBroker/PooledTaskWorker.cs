/*
 * PooledTaskWorker.cs
 *
 * Copyright (C) 2010-2015 by Microsoft Corporation
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
using DeployR;
using System.Threading;

namespace DeployRBroker
{
    /// <summary>
    /// Represents a Pooled RBrokerWorker implementation
    /// </summary>
    /// <remarks></remarks>
    public class PooledTaskWorker : RBrokerWorker
    {
        private PooledTask m_task;
        private long m_executorTaskRef;
        private Boolean m_isPriorityTask;
        private RProject m_rProject;
        private PooledTaskBroker m_rBroker;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected PooledTaskWorker()
        {
        }

        /// <summary>
        /// Constructor for specifying a Pooled Instance of RBrokerWorker
        /// </summary>
        /// <param name="task">DiscreteTask reference</param>
        /// <param name="executorTaskRef">Reserved for future use</param>
        /// <param name="isPriorityTask">Boolean indicating this is a high priority task</param>
        /// <param name="resourceToken">integer referencing the token from the reosurce pool</param>
        /// <param name="rBroker">RBroker reference</param>
        /// <remarks></remarks>
        public PooledTaskWorker(PooledTask task,
                                long executorTaskRef,
                                Boolean isPriorityTask,
                                RProject resourceToken,
                                RBroker rBroker) 
        {

            m_task = task;
            m_executorTaskRef = executorTaskRef;
            m_isPriorityTask = isPriorityTask;
            m_rProject = resourceToken;
            m_rBroker = (PooledTaskBroker) rBroker;
        }

        /// <summary>
        /// Run the acutal discrete task using Project API
        /// </summary>
        /// <returns>Results of the pooled task</returns>
        /// <remarks></remarks>
        public RTaskResult call() 
        {

            RTaskResult taskResult = null;

            long timeOnCall = 0L;
            //long timeOnServer = 0L;

            try 
            {

                ProjectExecutionOptions options = ROptionsTranslator.translate(m_task.options);

                long startTime = Environment.TickCount;

                RProjectExecution execResult = m_rProject.executeScript(m_task.filename,
                                          m_task.directory,
                                          m_task.author,
                                          m_task.version,
                                          options);

                
                timeOnCall = Environment.TickCount - startTime;


                String generatedConsole = execResult.about().console;

                List<String> generatedPlots = new List<String>();
                if(execResult.about().results != null)
                {
                    foreach(RProjectResult result in execResult.about().results) 
                    {
                        generatedPlots.Add(result.about().url);
                    }
                }

                List<String> generatedFiles = new List<String>();
                if(execResult.about().artifacts != null) 
                {
                    foreach(RProjectFile artifact in execResult.about().artifacts) 
                    {
                        generatedFiles.Add(artifact.about().url);
                    }
                }

                List<RData> generatedObjects = execResult.about().workspaceObjects;

                List<String> storedFiles = new List<String>();
                if(execResult.about().repositoryFiles != null) 
                {
                    foreach(RRepositoryFile repoFile in execResult.about().repositoryFiles) 
                    {
                        storedFiles.Add(repoFile.about().url);
                    }
                }

                taskResult = new RTaskResultImpl(execResult.about().id,
                                                 RTaskType.POOLED,
                                                 true,
                                                 execResult.about().timeCode,
                                                 execResult.about().timeTotal,
                                                 timeOnCall, null,
                                                 false,
                                                 generatedConsole,
                                                 generatedPlots,
                                                 generatedFiles,
                                                 generatedObjects,
                                                 storedFiles);

            } 
            catch (Exception ex) 
            {

                //if(ex.getCause() instanceof InterruptedException) 
                //{
                    try 
                    {
                        /*
                         * If RTaskToken.cancel() raises InterruptedException
                         * then ensure any corresponding execution on RProject is
                         * also cancelled.
                         */
                        m_rProject.interruptExecution();
                    } 
                    catch(Exception iex)
                    {
                        throw new Exception("Project cancel Exception occurred:  " + iex.ToString());
                    }
                //}

                taskResult = new RTaskResultImpl(null,
                                                 RTaskType.POOLED,
                                                 false,
                                                 0L,
                                                 0L,
                                                 0L, ex);
            } 
            finally 
            {

                /*
                 * Callback to PooledTaskBroker to release
                 * RProject back into pool for other tasks.
                 */
                m_rBroker.callback(m_task, taskResult);
            }

            return taskResult;
        }
    }
}
