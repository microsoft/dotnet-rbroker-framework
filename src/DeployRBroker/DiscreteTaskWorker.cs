/*
 * DiscreteTaskWorker.cs
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
    /// Represents a Discrete RBrokerWorker implementation
    /// </summary>
    /// <remarks></remarks>
    public class DiscreteTaskWorker : RBrokerWorker
    {
        private DiscreteTask m_task;
        private long m_executorTaskRef;
        private Boolean m_isPriorityTask;
        private RClient m_rClient;
        private int m_resourceToken;
        private DiscreteTaskBroker m_rBroker;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected DiscreteTaskWorker()
        {
        }

        /// <summary>
        /// Constructor for specifying a Discrete Instance of RBrokerWorker
        /// </summary>
        /// <param name="task">DiscreteTask reference</param>
        /// <param name="executorTaskRef">Reserved for future use</param>
        /// <param name="isPriorityTask">Boolean indicating if this ia high priority task</param>
        /// <param name="rClient">RClient reference</param>
        /// <param name="resourceToken">integer referencing the token from the reosurce pool</param>
        /// <param name="rBroker">RBroker reference</param>
        /// <remarks></remarks>
        public DiscreteTaskWorker(DiscreteTask task,
                                  long executorTaskRef,
                                  Boolean isPriorityTask,
                                  RClient rClient,
                                  int resourceToken,
                                  RBroker rBroker) 
        {

            m_task = task;
            m_executorTaskRef = executorTaskRef;
            m_isPriorityTask = isPriorityTask;
            m_rClient = rClient;
            m_resourceToken = resourceToken;
            m_rBroker = (DiscreteTaskBroker) rBroker;
        }

        /// <summary>
        /// Run the acutal discrete task using Client API
        /// </summary>
        /// <returns>Results of the discrete task</returns>
        /// <remarks></remarks>
        public RTaskResult call() 
        {

            RTaskResult taskResult = null;

            long timeOnCall = 0L;
            //long timeOnServer = 0L;

            try 
            {

                AnonymousProjectExecutionOptions options = ROptionsTranslator.translate(m_task.options);

                long startTime = Environment.TickCount;

                RScriptExecution execResult = m_rClient.executeScript(m_task.filename,
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
                                            RTaskType.DISCRETE,
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

                //if(ex.getCause() == typeof(InterruptedException)) 
                //{
                    /*
                     * RTaskToken.cancel() can raise an InterruptedException.
                     * When an InterruptedException is detected the DiscreteTask
                     * executing on the server should be aborted at this point.
                     * However, there is no way to obtain DeployR reference, such
                     * as a projectId, for an stateless execution in-progress, so
                     * aborting the current RTask operation is not possible.
                     */
                //}

                taskResult = new RTaskResultImpl(null,
                                                 RTaskType.DISCRETE,
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
