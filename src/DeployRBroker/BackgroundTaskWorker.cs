/*
 * BackgroundTaskWorker.cs
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
using DeployR;

namespace DeployRBroker
{
    /// <summary>
    /// Represents a Background RBrokerWorker implementation
    /// </summary>
    /// <remarks></remarks>
    public class BackgroundTaskWorker : RBrokerWorker
    {
        private BackgroundTask m_task;
        private long m_executorTaskRef;
        private Boolean m_isPriorityTask;
        private RUser m_rUser;
        private int m_resourceToken;
        private BackgroundTaskBroker m_rBroker;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected BackgroundTaskWorker()
        {
        }

        /// <summary>
        /// Constructor for specifying a Background Instance of RBrokerWorker
        /// </summary>
        /// <param name="task">BackgroundTask reference</param>
        /// <param name="executorTaskRef">Reserved for future use</param>
        /// <param name="isPriorityTask">Boolean indicating if this ia high priority task</param>
        /// <param name="rUser">RUser reference</param>
        /// <param name="resourceToken">integer referencing the token from the reosurce pool</param>
        /// <param name="rBroker">RBroker reference</param>
        /// <remarks></remarks>
        public BackgroundTaskWorker(BackgroundTask task,
                                    long executorTaskRef,
                                    Boolean isPriorityTask,
                                    RUser rUser,
                                    int resourceToken,
                                    RBroker rBroker) 
        {

            m_task = task;
            m_executorTaskRef = executorTaskRef;
            m_isPriorityTask = isPriorityTask;
            m_rUser = rUser;
            m_resourceToken = resourceToken;
            m_rBroker = (BackgroundTaskBroker) rBroker;

        }


        /// <summary>
        /// Run the acutal background task using Job API
        /// </summary>
        /// <returns>Results of the background task</returns>
        /// <remarks></remarks>
        public RTaskResult call() 
        {


            RTaskResult taskResult = null;
            RJob rJob = null;

            long timeOnCall = 0L;
            //long timeOnServer = 0L;

            try 
            {

                long startTime = Environment.TickCount;

                JobExecutionOptions options = ROptionsTranslator.translate(m_task.options, m_isPriorityTask);

                if(m_task.code != "") 
                {
                    rJob = m_rUser.submitJobCode(m_task.name,
                                        m_task.description,
                                        m_task.code,
                                        options);
                } 
                else
                {
                    if(m_task.external != "") 
                    {
                        rJob = m_rUser.submitJobExternal(m_task.external,
                                        m_task.description,
                                        m_task.code,
                                        options);
                    } 
                    else 
                    {

                        rJob = m_rUser.submitJobScript(m_task.name,
                                          m_task.description,
                                          m_task.filename,
                                          m_task.directory,
                                          m_task.author,
                                          m_task.version,
                                          options);
                    }
                }

                timeOnCall = Environment.TickCount - startTime;

                taskResult = new RTaskResultImpl(rJob.about().id,
                                                 RTaskType.BACKGROUND,
                                                 true,
                                                 0L,
                                                 0L,
                                                 timeOnCall,
                                                 null);

            } 
            catch (Exception ex) 
            {

                //if(ex. instanceof InterruptedException) 
                //{
                    try 
                    {
                        /*
                         * If RTaskToken.cancel() call raises InterruptedException
                         * then ensure any corresponding scheduled RJob is
                         * also cancelled.
                         */
                        rJob.cancel();
                    } 
                    catch(Exception iex) 
                    {
                        throw new Exception("RBroker: could not cancel job, cause:  " + iex.ToString());
                    }
                //}

                taskResult = new RTaskResultImpl(null,
                                                 RTaskType.BACKGROUND,
                                                 false,
                                                 0L,
                                                 0L,
                                                 0L,
                                                 ex);
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
