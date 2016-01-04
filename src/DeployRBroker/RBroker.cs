/*
 * RBroker.cs
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

namespace DeployRBroker
{
    /// <summary>
    /// Represents a high-level programming model for
    /// building DeployR-enabled client applications.
    /// By using RBroker an application developer can focus entirely on integrating 
    /// R Analytics, while offloading the complexity
    /// of managing client-side API task queues and server-side
    /// R session lifecycles.
    /// 
    /// The basic programming model for working with RBroker is as follows:
    /// 
    /// Decide if the R Analytics tasks for your application
    /// should execute as:
    /// 
    /// Discrete tasks: authentication optional, grid resources
    /// allocated at runtime, results returned immediately, no persistence.
    /// Good for prototyping and public facing production deployments.
    /// 
    /// Pooled tasks: authentication required, grid resources
    /// pre-allocated, results returned immediately, optional persistence
    /// to repository. Good for enterprise production deployments,
    /// consistent runtime, high-throughput environments.
    /// 
    /// Background tasks: authentication required, grid resources
    /// allocated at runtime, results persisted for later retrieval. Good
    /// for periodic, scheduled or batch processing.
    /// 
    /// Use the RBrokerFactory to create an appropriate instance of RBroker.
    /// Define the R Analytics tasks for your application as one
    /// or more RTask. Submit your RTask to RBroker for execution.
    /// Track the progress of your RTask using RTaskToken.
    /// Integrate the results of your RTask found within RTaskResult.
    /// 
    /// This programming model can be further simplified for application
    /// developers by leveraging asynchronous callbacks. Simply register
    /// an RTaskListener with your
    /// RBroker instance and the RBroker will automatically notify your
    /// application when each RTask completes.
    /// This approach allows your application to skip step 5. above
    /// and scale effortlessly.
    /// </summary>
    /// <remarks></remarks>
    public interface RBroker
    {
        /// <summary>
        /// Refresh configuration for the RBroker
        /// Note, support for refresh is only available on the
        /// PooledTaskBroker runtime. In addition, only PoolCreationOptions
        /// are processed on this call. All other RBrokerConfig options are
        /// ignored.
        /// 
        /// A refresh causes all workspace objects and directory files
        /// in the underlying R sessions within the pool to be
        /// cleared before new workspace objects and/or directory
        /// files are loaded per the new config options.
        /// 
        /// Only an idle RBroker instance can be refreshed. An RBrokerException
        /// will be raised if this method is called when the instance
        /// of RBroker is currently busy with queued or executing RTask.
        /// </summary>
        /// <param name="config">RBrokerConfig reference</param>
        /// <remarks></remarks>
        void refresh(RBrokerConfig config);

        /// <summary>
        /// Submit an RTask for execution under the control of RBroker.
        /// </summary>
        /// <param name="task">RTask reference</param>
        /// <returns>RTaskToken reference</returns>
        /// <remarks></remarks>
        RTaskToken submit(RTask task);

        /// <summary>
        /// Submit a priority RTask for execution under the control of RBroker.
        /// 
        /// Priority tasks are automatically moved to the front of the
        /// queue, ahead of all standard tasks that are already pending
        /// execution by the broker.
        /// </summary>
        /// <param name="task">RTask reference</param>
        /// <param name="priority">Boolean indicating if task should be high priority</param>
        /// <returns>RTaskToken reference</returns>
        /// <remarks></remarks>
        RTaskToken submit(RTask task, Boolean priority);

        /// <summary>
        /// Register an asynchronous listener to receive callbacks
        /// on RTask completion or failure events.
        /// </summary>
        /// <param name="taskListener">RTaskListener reference</param>
        /// <remarks></remarks>
        void addTaskListener(RTaskListener taskListener);

        /// <summary>
        /// Register an asynchronous listener to receive callbacks
        /// on RBroker runtime statistics events or runtime errors.
        /// </summary>
        /// <param name="brokerListener">RBrokerListener reference</param>
        /// <remarks></remarks>
        void addBrokerListener(RBrokerListener brokerListener);

        /// <summary>
        /// Launch an RTaskAppSimulator simulation. The RTask defined
        /// by your simulation will be automatically executed by
        /// the current instance of RBroker.
        /// 
        /// Make sure to register your RTaskListener and RBrokerListener
        /// before starting your simulation in order to receive
        /// asynchronous callbacks in your application when RTask complete
        /// and/or to receive runtime summary statistics from RBroker as the
        /// simulation proceeds.
        /// </summary>
        /// <param name="appSimulator">RTaskAppSimulator reference</param>
        /// <remarks></remarks>
        void simulateApp(RTaskAppSimulator appSimulator);

        /// <summary>
        /// Returns the task execution concurrency levels enforced for
        /// this instance of RBroker.
        /// </summary>
        /// <returns>max concurrency value</returns>
        /// <remarks></remarks>
        long maxConcurrency();

        /// <summary>
        /// Returns status indicating current RTask
        /// activity on RBroker.
        /// 
        /// This call can be used to determine if an RBroker instance is
        /// idle which can be particularly useful ahead calls to RBroker#shutdown.
        /// 
        /// The RBrokerStatus#pendingTasks and RBrokerStatus#executingTasks
        /// fields can be used by an application to estimate time remaining
        /// until RBroker reaches an idle state.
        /// 
        /// The RBrokerStatus#isIdle field is provided for convenience for an application if individual pending
        /// and executing task counts are not relevant in RBroker#shutdown decisions.
        /// </summary>
        /// <returns>RBrokerStatus reference</returns>
        /// <remarks></remarks>
        RBrokerStatus status();

        /// <summary>
        /// Flushes all pending RTask
        /// from queues maintained by RBroker.  Flushing RTask
        /// queues ensures that queued tasks will not be executed by RBroker.
        /// </summary>
        /// <returns>RBrokerStatus reference</returns>
        /// <remarks></remarks>
        RBrokerStatus flush();

        /// <summary>
        /// Release all client-side and server-side resources maintained
        /// by or on behalf of an instance of RBroker.
        /// </summary>
        /// <remarks></remarks>
        void shutdown();

        /// <summary>
        /// Returns a token indicating the owner of the current instance of RBroker
        /// by or on behalf of an instance of RBroker.
        /// </summary>
        /// <returns>For anonymous rBroker instances, null is returned. For authenticated 
        /// RBroker instances, an instance of RUser is returned.</returns>
        /// <remarks></remarks>
        RUser owner();

        /// <summary>
        /// Can be called by the RBroker client application to block 
        /// until shutdown() on RBroker is called
        /// </summary>
        /// <remarks></remarks>
        void waitUntilShutdown();

    }

}
