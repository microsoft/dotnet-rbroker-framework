/*
 * RBrokerEngine.cs
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
    /// Base class for creating an implementation of RBroker interface
    /// </summary>
    /// <remarks></remarks>
    public abstract class RBrokerEngine : RBroker
    {
        private RTaskAppSimulator m_appSimulator;

        ///<summary>RTaskListener reference</summary>
            protected RTaskListener m_taskListener;
        ///<summary>RBrokerListener reference</summary>
            protected RBrokerListener m_brokerListener;
        ///<summary>RBrokerConfig reference</summary>
            protected RBrokerConfig m_brokerConfig;
        ///<summary>parallel task limit</summary>
            protected long m_parallelTaskLimit;
        ///<summary>RClient reference</summary>
            protected RClient m_rClient;
        ///<summary>RUser reference</summary>
            protected RUser m_rUser;
        ///<summary>MAX_TASK_QUEUE_SIZE value</summary>
            protected const int MAX_TASK_QUEUE_SIZE = 99999;
        ///<summary>LIVE_TASK_TOKEN_PEEK_INTERVAL value</summary>
            protected const int LIVE_TASK_TOKEN_PEEK_INTERVAL = 25;
        ///<summary>initialization state of the RBrokerEngine</summary>
            protected Semaphore m_engineInitialized = new Semaphore(0, 1);
        ///<summary>tasks in the low priority queue</summary>
            protected BlockingCollection<RTask> m_pendingLowPriorityQueue = new BlockingCollection<RTask>(MAX_TASK_QUEUE_SIZE);
        ///<summary>tasks in the high priority queue</summary>
            protected BlockingCollection<RTask> m_pendingHighPriorityQueue = new BlockingCollection<RTask>(MAX_TASK_QUEUE_SIZE);
        ///<summary>member variable that holds collection of task tokens</summary>
            protected ConcurrentBag<RTaskToken> m_liveTaskTokens = new ConcurrentBag<RTaskToken>();
        ///<summary>map of RTask and Task Tokens</summary>
            protected ConcurrentDictionary<RTask, Object> m_taskResourceTokenMap;
        ///<summary>map of RTasks and Task Token Listeners</summary>
            protected ConcurrentDictionary<RTask, RTaskTokenListener> m_taskTokenListenerMap;
        ///<summary>status of the RBroker</summary>
            protected long m_taskBrokerIsActive = 1;
        ///<summary>count of tasks on RBroker</summary>
            protected long m_executorTaskCounter = 0;
        ///<summary>count of status of a referesh of the broker configuration</summary>
            protected long m_refreshingConfig = 0;
        ///<summary>
        /// For an RTask to execute, it most hold a resourceToken
        /// taken from the resourceTokenPool. The size of the
        /// resourceTokenPool determines the maximum number of
        /// concurrent RTask that can be executing at the same time.
        /// 
        /// The nature of the resourceToken itself will depend on the
        /// concrete implementation of the RBrokerEngine. Currently,
        /// the DiscteteTaskBroker and BackgroundTaskBroker use simple
        /// Object as resourceTokens. The PooledTaskBroker
        /// uses RProject.
        /// 
        /// When an RTask completes the RBrokerWorker that executed
        /// the task is responsible for making  make a callback() on
        /// RBrokerEngine signaling that the resourceToken associated
        /// with the RTask can be released back into the resourceTokenPool,
        /// making the token available for use by another RTask to run.
        /// </summary>
            protected BlockingCollection<Object> m_resourceTokenPool;
        ///<summary>Task handle to the RBrokerWorkerManager Thread</summary>
            protected Task m_RBrokerWorkerManagerTask;
        ///<summary>Task handle to the RBrokerListenerManager Thread</summary>
            protected Task m_RBrokerListenerManagerTask;
            

        /// <summary>
        /// RBrokerEngine constructor.
        /// </summary>
        /// <param name="brokerConfig">RBrokerConfig reference</param>
        /// <remarks></remarks>
        public RBrokerEngine(RBrokerConfig brokerConfig) 
        {
            m_brokerConfig = brokerConfig;
        }

        /// <summary>
        /// Iinitalization of the RBrokerEngine.
        /// </summary>
        /// <param name="parallelTaskLimit">Limit of the number of parallel tasks allowed</param>
        /// <remarks></remarks>
        protected void initEngine(int parallelTaskLimit)
        {
            try 
            {
                Interlocked.Add(ref m_parallelTaskLimit, parallelTaskLimit);

                //set the threadpool minimum size
                ThreadPool.SetMinThreads(parallelTaskLimit + 10, parallelTaskLimit + 10);
                //initialize the collections
                m_resourceTokenPool = new BlockingCollection<Object>(parallelTaskLimit);
                m_taskResourceTokenMap = new ConcurrentDictionary<RTask, Object>(parallelTaskLimit, parallelTaskLimit);
                m_taskTokenListenerMap = new ConcurrentDictionary<RTask, RTaskTokenListener>(parallelTaskLimit, parallelTaskLimit);

            } 
            catch(Exception ex) 
            {

                throw new Exception("Broker failed to initialize, cause" + ex.ToString());
            }

            try 
            {
                m_RBrokerWorkerManagerTask = Task.Factory.StartNew(() => RBrokerWorkerManager());
            } 
            catch(Exception rex) 
            {
                shutdown();
                throw new Exception("Broker failed to start worker manager, cause: " + rex.ToString());
            }

            try 
            {
                m_RBrokerListenerManagerTask = Task.Factory.StartNew(() => RBrokerListenerManager());
            } 
            catch(Exception rex) 
            {
                shutdown();
                throw new Exception("Broker failed to start listener manager, cause: " + rex.ToString());
            }

            try
            {
                 m_engineInitialized.WaitOne(5000);
            } 
            catch(Exception iex) 
            {
                shutdown();
                throw new Exception("Broker failed to initialized, cause: " + iex.ToString());
            }
        }

        /// <summary>
        /// Implementation of RBroker Interface 'refresh' method
        /// </summary>
        /// <param name="config">RBrokerConfig reference</param>
        /// <remarks></remarks>
        public void refresh(RBrokerConfig config)
        {
            throw new Exception("RBrokerEngine refresh not supported.");
        }

        /// <summary>
        /// Implementation of RBroker Interface 'submit' method
        /// </summary>
        /// <param name="task">RTask submitted to the RBroker for execution</param>
        /// <returns>RTaskToken reference</returns>
        /// <remarks></remarks>
        public RTaskToken submit(RTask task)
        {
            return submit(task, false);
        }

        /// <summary>
        /// Implementation of RBroker Interface 'submit' method
        /// </summary>
        /// <param name="task">RTask submitted to the RBroker for execution</param>
        /// <param name="priority">Boolean indicating this is a high priority task</param>
        /// <returns>RTaskToken reference</returns>
        /// <remarks></remarks>
        public RTaskToken submit(RTask task, Boolean priority)
        {

            if(Interlocked.Read(ref m_refreshingConfig) == 1) 
            {
                Console.WriteLine("RTask submissions temporarily disabled while RBroker configuration refreshes.");
            }

            try
            {

                /*
                 * We clone the incoming RTask into a new instance of
                 * RTask to ensure RTask is unique so it can be used
                 * as a key inside the taskTokenListenerMap.
                 *
                 * How could the value of RTask param not be unique?
                 *
                 * For example, RBrokerAppSimulator apps frequently
                 * create a single RTask instance and submit it many times
                 * to simulate load.
                 */
                RTask clonedTask = cloneTask(task);

                Boolean added = false;
                if(priority)
                {
                    added = m_pendingHighPriorityQueue.TryAdd(clonedTask);
                }
                else
                {
                    added = m_pendingLowPriorityQueue.TryAdd(clonedTask);
                }

                if(!added) 
                {
                    throw new Exception("Broker at capacity ( " + MAX_TASK_QUEUE_SIZE + " ), rejecting task " + clonedTask);
                }

                RTaskToken rTaskToken = new RTaskTokenImpl(task);

                /*
                 * RTask now on pending[High,Low]PriorityQueue,
                 * appending associated RTaskToken to end of
                 * liveTaskTokens.
                 */
                m_liveTaskTokens.Add(rTaskToken);

                /*
                 * Register RTask and associated RTaskToken here.
                 * Once RTask has been submitted to Executor and
                 * Future for task exists, we will make an
                 * RTaskToken.onTask callback to register Future
                 * on token.
                 */
                m_taskTokenListenerMap.TryAdd(clonedTask, rTaskToken);

                return rTaskToken;

            } 
            catch(Exception rex) 
            {
                throw new Exception("RBroker: submit failed, cause: " + rex.ToString());
            }
            
        }

        /// <summary>
        /// Implementation of RBroker Interface 'addTaskListener' method
        /// </summary>
        /// <param name="taskListener">RTaskListener reference</param>
        /// <remarks></remarks>
        public void addTaskListener(RTaskListener taskListener)
        {

            m_taskListener = taskListener;
        }

        /// <summary>
        /// Implementation of RBroker Interface 'addBrokerListener' method
        /// </summary>
        /// <param name="brokerListener">RBrokerListener reference</param>
        /// <remarks></remarks>
        public void addBrokerListener(RBrokerListener brokerListener)
        {

            m_brokerListener = brokerListener;
        }

        /// <summary>
        /// Implementation of RBroker Interface 'simulateApp' method
        /// </summary>
        /// <param name="appSimulator">RTaskAppSimulator reference</param>
        /// <remarks></remarks>
        public void simulateApp(RTaskAppSimulator appSimulator) 
        {

            /*
             * Auto-register RTaskAppSimulator as RTaskListener
             * if interface is implemented by appSimulator.
             */
            if(m_taskListener == null && (appSimulator is RTaskListener))
            {
                m_taskListener = (RTaskListener) appSimulator;
            }

            /*
             * Auto-register RTaskAppSimulator as RBrokerListener
             * if interface is implemented by appSimulator.
             */
            if(m_brokerListener == null && (appSimulator is RBrokerListener))
            {
                m_brokerListener = (RBrokerListener) appSimulator;
            }

            m_appSimulator = appSimulator;

            if(appSimulator != null) 
            {
                appSimulator.simulateApp(this);
            }

        }

        /// <summary>
        /// Implementation of RBroker Interface 'maxConcurrency' method
        /// </summary>
        /// <returns>maximum currency of tasks allowed on the broker</returns>
        /// <remarks></remarks>
        public long maxConcurrency()
        {
            return Interlocked.Read(ref m_parallelTaskLimit);
        }

        /// <summary>
        /// Implementation of RBroker Interface 'status' method
        /// </summary>
        /// <returns>RBrokerStatus reference</returns>
        /// <remarks></remarks>
        public RBrokerStatus status() 
        {

            /*
             * Pending tasks include all tasks on
             * high and low priority queues.
             */
            int pendingTasks = m_pendingHighPriorityQueue.Count() + m_pendingLowPriorityQueue.Count();

            int executingTasks = (int)Interlocked.Read(ref m_parallelTaskLimit) - m_resourceTokenPool.Count();

            return new RBrokerStatus(pendingTasks, executingTasks);
        }

        /// <summary>
        /// Implementation of RBroker Interface 'flush' method
        /// </summary>
        /// <returns>RBrokerStatus reference</returns>
        /// <remarks></remarks>
        public RBrokerStatus flush() 
        {

            /*
             * Flush all pending tasks from
             * high and low priority queues.
             */
            foreach (var item in m_pendingHighPriorityQueue.GetConsumingEnumerable())
            {
            }
            foreach (var item in m_pendingLowPriorityQueue.GetConsumingEnumerable())
            {
            }
            return status();
        }


        /// <summary>
        /// Implementation of RBroker Interface 'shutdown' method
        /// </summary>
        /// <remarks></remarks>
        public void shutdown() 
        {
            Interlocked.Exchange(ref m_taskBrokerIsActive, 0);

            if (m_resourceTokenPool.Count > 0)
            {
                Boolean releaseGridResources = false;

                if (m_brokerConfig is PooledBrokerConfig)
                {
                    PooledBrokerConfig pbcfg = (PooledBrokerConfig)m_brokerConfig;
                    releaseGridResources = pbcfg.poolCreationOptions.releaseGridResources;
                }
                
                if (releaseGridResources)
                {
                    /*
                    * If PooledTaskBroker resource tokens
                    * and rUser available, perform a server-wide
                    * flush of projects on the grid.
                    */
                    m_rUser.releaseProjects();
                }
                else
                {
                    /*
                    * If PooledTaskBroker resource tokens
                    * and rUser not available, perform
                    * project-by-project flush on the grid.
                    */

                    foreach (Object resourceToken in m_resourceTokenPool)
                    {
                        try
                        {
                            if (resourceToken is RProject)
                            {
                                RProject projectToken = (RProject)resourceToken;
                                projectToken.close();
                            }
                        }
                        catch (Exception cex)
                        {
                            throw new Exception("RBroker: project close failed, cause: " + cex.ToString());
                        }
                    }
                }
            }

            if (m_rClient != null)
            {
                try
                {
                    if (m_rUser != null)
                    {
                        m_rClient.logout(m_rUser);
                    }
                }
                catch (Exception rex)
                {
                    throw new Exception("RBroker: RClient logout failed, cause: " + rex.ToString());
                }
            }
        }

        /// <summary>
        /// Implementation of RBroker Interface 'owner' method
        /// </summary>
        /// <returns>RUser instance</returns>
        /// <remarks></remarks>
        public RUser owner()
        {
            return m_rUser;
        }

        /// <summary>
        /// Implementation of RBroker Interface 'waitUntilShutdown' method
        /// </summary>
        /// <remarks></remarks>
        public void waitUntilShutdown()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(m_RBrokerWorkerManagerTask);
            tasks.Add(m_RBrokerListenerManagerTask);
            Task.WaitAll(tasks.ToArray());
        }

        /// <summary>
        /// Callback for RBrokerWorker. Must be implemented by
        /// concrete implementation of RBrokerEngine.
        /// 
        /// Responsible for adding an engineResourceToken
        /// back into the RBrokerEngine resourceTokenPool, 
        /// making the token available for use by another
        /// RTask.
        /// </summary>
        /// <param name="task">RTask reference</param>
        /// <param name="result">RTaskResult reference</param>
        /// <remarks></remarks>
        public abstract void callback(RTask task, RTaskResult result);

        /// <summary>
        /// cloneTask method that must be implemented by
        /// concrete implementation of RBrokerEngine.
        /// 
        /// Responsible for cloning an RTask.
        /// </summary>
        /// <remarks></remarks>
        protected abstract RTask cloneTask(RTask genesis);

        /// <summary>
        /// createBrokerWorker method that must be implemented by
        /// concrete implementation of RBrokerEngine.
        /// 
        /// Responsible for creating a RBrokerWorker for a task.
        /// </summary>
        /// <remarks></remarks>
        protected abstract RBrokerWorker createBrokerWorker(RTask task,
                                                        long taskIndex,
                                                        Boolean isPriorityTask,
                                                        Object resourceToken,
                                                        RBrokerEngine brokerEngine);

        /*
         * RBrokerEngine: private implementation.
         */
        private void RBrokerWorkerManager() 
        {
            try {

                /*
                    * Signal to constructor that the brokerEngineExecutor
                    * thread is fully initialized and active, allowing
                    * the constructor to return safely to caller.
                    */
                m_engineInitialized.Release();

                while(Interlocked.Read(ref m_taskBrokerIsActive) == 1) 
                {
                    int queueCount = 0;
                    RTask nextTaskInQueue = null;

                    /*
                    * Await next queued Task.
                    */
                    Boolean priorityTaskAvailable = false;
                    while(nextTaskInQueue == null && (Interlocked.Read(ref m_taskBrokerIsActive) == 1)) 
                    {

                        /*
                        * Retrieves but does not remove the task
                        * at the head of the queue.
                        */

                        queueCount = m_pendingHighPriorityQueue.Count();

                        if (queueCount == 0)
                        {
                            queueCount = m_pendingLowPriorityQueue.Count();
                            priorityTaskAvailable = false;
                        }
                        else
                        {
                            priorityTaskAvailable = true;
                        }
                        if (queueCount == 0) 
                        {
                            try 
                            {
                                /*
                                * Avoid busy-wait, sleep.
                                */
                                Thread.Sleep(50);
                            } 
                            catch(Exception tex) 
                            {
                                throw new Exception("Exception " + nextTaskInQueue + ", ex=" + tex.ToString());
                            }
                        } 
                        else 
                        {
                            /*
                            * Retrieves and removes the task
                            * at the head of the queue.
                            */
                            if (priorityTaskAvailable == true)
                            {
                                nextTaskInQueue = m_pendingHighPriorityQueue.Take();
                            }
                            else
                            {
                                nextTaskInQueue = m_pendingLowPriorityQueue.Take();
                            }
                        }
                    }

                    /*
                    * If task found on queue and taskBroker
                    * is still active, process task.
                    */
                    if (nextTaskInQueue != null && Interlocked.Read(ref m_taskBrokerIsActive) == 1) 
                    {
                        /*
                        * Await next available resource token in pool.
                        */
                        Object resourceToken = m_resourceTokenPool.Take();
                        Boolean resourceTokenInUse = false;

                        try 
                        {

                            RBrokerWorker worker = createBrokerWorker(nextTaskInQueue,
                                                Interlocked.Increment(ref m_executorTaskCounter),
                                                priorityTaskAvailable,
                                                resourceToken,
                                                this);

                            resourceTokenInUse = true;
                            m_taskResourceTokenMap.TryAdd(nextTaskInQueue, resourceToken);
                            
                            Task<RTaskResult> future = Task.Factory.StartNew<RTaskResult>(() => worker.call(), TaskCreationOptions.None);

                            RTaskTokenListener taskTokenListener = null;
                            m_taskTokenListenerMap.TryRemove(nextTaskInQueue, out taskTokenListener);

                            if(taskTokenListener != null) 
                            {
                                taskTokenListener.onTask(nextTaskInQueue, future);
                            } 
                            else 
                            {
                                throw new Exception("RBrokerEngine: " +
                                    "taskTokenListener callback not found for " +
                                    nextTaskInQueue + ", unexpected error.");
                            }

                        } 
                        catch(Exception ex) 
                        {
                            if(!resourceTokenInUse && resourceToken != null) 
                            {
                                /*
                                * Return unused RProject instance
                                * to the tail of the project pool.
                                */
                                m_resourceTokenPool.TryAdd(resourceToken);
                            }
                            throw new Exception("RBrokerEngine:  processing task " + nextTaskInQueue + ", ex=" + ex.ToString());
                        }

                    } // nextTaskInQueue != null

                } // while taskBrokerIsActive
             } 
            catch(Exception mex) 
            {
                throw new Exception("RBrokerEngine: brokerEngineExecutor.run ex=" + mex.ToString());
            }
        }

        private void RBrokerListenerManager() 
        {
            while (Interlocked.Read(ref m_taskBrokerIsActive) == 1) 
                {
                    int tasksHandledOnLoop = 0;
     
                    try 
                    {
                        while (m_liveTaskTokens.Count() == 0 && Interlocked.Read(ref m_taskBrokerIsActive) == 1) 
                        {
                             Thread.Sleep(LIVE_TASK_TOKEN_PEEK_INTERVAL);
                        }

                        foreach (RTaskToken rTaskToken in m_liveTaskTokens) 
                        {
                           Boolean repeatTaskFound = false;

                           if(rTaskToken.isDone()) 
                           { 
                                RTaskResult result = null;

                                try 
                                {
                                    // Extract task result.
                                    result = (RTaskResult) rTaskToken.getResult();

                                    if (m_taskListener != null) 
                                    {
                                        if (((RTaskResultImpl)result).repeatTask)
                                        {
                                            repeatTaskFound = true;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                m_taskListener.onTaskCompleted(rTaskToken.getTask(), result);
                                            }
                                            catch
                                            {
                                                /*
                                                 * RBrokerEngine onTaskCompleted
                                                 * is calling back into client
                                                 * application code. That code
                                                 * could erroneously throw an
                                                 * Exception back into
                                                 * RBrokerEngine. If so, swallow
                                                 * it.
                                                 */
                                            }

                                        }
                                    }

                                } 
                                catch(Exception ex) 
                                {
                                    if (m_taskListener != null) 
                                    {
                                        m_taskListener.onTaskError(rTaskToken.getTask(), ex.ToString());
                                    }
                                }


                                RTaskToken tempTaskToken = null;
                                m_liveTaskTokens.TryTake(out tempTaskToken);

                                if (!repeatTaskFound)
                                {
                                    tasksHandledOnLoop++;
                                    updateBrokerStats(result);
                                }
                            }
                        }
                    } 
                    catch(Exception ex) 
                    {
                        if (m_brokerListener != null) 
                        {
                            m_brokerListener.onRuntimeError(ex.ToString());
                        }
                    }

                    if(tasksHandledOnLoop > 0) 
                    {
                        if (m_brokerListener != null) 
                        {
                            m_brokerListener.onRuntimeStats(buildStats(), (int)maxConcurrency());
                        }
                    }

                } // while taskBrokerIsActive
            }

            /*
             * Updates RBrokerRuntimeStats for RBroker.
             */
            private void updateBrokerStats(RTaskResult result) 
            {

                Interlocked.Increment(ref m_totalTasksRunByBroker);

                if(result.isSuccess()) 
                {
                    Interlocked.Increment(ref m_totalTasksRunToSuccess);
                }
                Interlocked.Add(ref m_totalTaskTimeOnCode, result.getTimeOnCode());
                Interlocked.Add(ref m_totalTaskTimeOnServer, result.getTimeOnServer());
                Interlocked.Add(ref m_totalTaskTimeOnCall, result.getTimeOnCall());
            }

            /*
             * Builds RBrokerRuntimeStats for RBroker
             * to publish onEngineStats().
             */
            private RBrokerRuntimeStats buildStats() 
            {

                RBrokerRuntimeStats stats = new RBrokerRuntimeStats();
                stats.totalTasksRun = Interlocked.Read(ref m_totalTasksRunByBroker);
                stats.totalTasksRunToSuccess = Interlocked.Read(ref m_totalTasksRunToSuccess);
                stats.totalTasksRunToFailure = stats.totalTasksRun - stats.totalTasksRunToSuccess;
                stats.totalTimeTasksOnCode = Interlocked.Read(ref m_totalTaskTimeOnCode);
                stats.totalTimeTasksOnServer = Interlocked.Read(ref m_totalTaskTimeOnServer);
                stats.totalTimeTasksOnCall = Interlocked.Read(ref m_totalTaskTimeOnCall);
                return stats;
            }

            private long m_totalTasksRunByBroker = 0;
            private long m_totalTasksRunToSuccess = 0;
            private long m_totalTaskTimeOnCode = 0;
            private long m_totalTaskTimeOnServer = 0;
            private long m_totalTaskTimeOnCall = 0;
        //}
    
    }
}
