/*
 * RTaskTokenImpl.cs
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
using System.Threading.Tasks;

namespace DeployRBroker
{
    /// <summary>
    /// Represents a handle to an RTask that is live on an RBroker
    /// </summary>
    /// <remarks></remarks>
    public class RTaskTokenImpl : RTaskToken
    {
        private RTask m_task;
        private RTaskResult m_result;
        private Task<RTaskResult> m_future;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected RTaskTokenImpl()
        {
        }

        internal RTaskTokenImpl(RTask task) 
        {
            m_task = task;
        }

        internal RTaskTokenImpl(RTask task, Task<RTaskResult> future) 
        {
            m_task = task;
            m_future = future;
        }

        /// <summary>
        /// RTask object
        /// </summary>
        /// <returns>RTask object</returns>
        /// <remarks></remarks>
        public RTask getTask() 
        {
            return m_task;
        }

        /// <summary>
        /// RTaskResult object
        /// </summary>
        /// <returns>RTaskResult object</returns>
        /// <remarks></remarks>
        public RTaskResult getResult() 
        {

            if(m_result != null) 
            {
                return m_result;
            } 
            else 
            {
                while(m_future == null) 
                {
                    try 
                    {
                        Thread.Sleep(250);
                    } 
                    catch(Exception iex) 
                    {
                        throw iex;
                    }
                }
                m_result = m_future.Result;
                return m_result;
            }
        }

        /// <summary>
        /// RTask future
        /// </summary>
        /// <returns>RTask future</returns>
        /// <remarks></remarks>
        public Task getFuture() 
        {
            return m_future;
        }

        /// <summary>
        /// RTask completion status
        /// </summary>
        /// <returns>Indicator that RTask has completed</returns>
        /// <remarks></remarks>
        public Boolean isDone() 
        {
            return (m_future != null) ? m_future.IsCompleted : false;
        }

        /// <summary>
        /// RTask cancel status
        /// </summary>
        /// <returns>Indicator if RTask has been cancelled</returns>
        /// <remarks></remarks>
        public Boolean isCancelled() 
        {
            return (m_future != null) ? m_future.IsCanceled: false;
        }

        /// <summary>
        /// Cancel an RTask
        /// </summary>
        /// <param name="mayInterruptIfRunning">Indicator if the task can be interrupted</param>
        /// <returns>Indicator that a Cancel command has been sent</returns>
        /// <remarks></remarks>
        public Boolean cancel(Boolean mayInterruptIfRunning) 
        {

            if(m_result != null) 
            {
                // RTask completed, can not be cancelled.
                return false;
            } 
            else
            {
                if (m_future != null)
                {
                    // RTask completed, can not be cancelled.
                    if (m_future.IsCompleted || m_future.IsCanceled)
                    {
                        return false;
                    }

                    // Delegate cancel operation to Future.cancel().
                    //TODO: return m_future.Cancel;
                    return false;

                }
                else
                {
                    // RTask still pending onTask() confirmation
                    // from RBroker, can not be cancelled.
                    return false;
                }
            }
        }

        /// <summary>
        /// Asynchronous callback notification when a TaskToken is returned to the pool.
        /// </summary>
        /// <param name="task">The RTask instance</param>
        /// <param name="future">The Operating System task reference</param>
        /// <remarks></remarks>
        public void onTask(RTask task, Task<RTaskResult> future) 
        {
            m_future = future;
        }
    }
}
