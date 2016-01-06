/*
 * RTaskToken.cs
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
using System.Threading;
using System.Threading.Tasks;

namespace DeployRBroker
{
    /// <summary>
    /// Represents a handle to an RTask that is live on an RBroker
    /// </summary>
    /// <remarks></remarks>
    public interface RTaskToken : RTaskTokenListener
    {
        /// <summary>
        /// RTask object
        /// </summary>
        /// <returns>RTask object</returns>
        /// <remarks></remarks>
        RTask getTask();

        /// <summary>
        /// RTaskResult object
        /// </summary>
        /// <returns>RTaskResult object</returns>
        /// <remarks></remarks>
        RTaskResult getResult();

        /// <summary>
        /// RTask future
        /// </summary>
        /// <returns>RTask future</returns>
        /// <remarks></remarks>
        Task getFuture();

        /// <summary>
        /// RTask completion status
        /// </summary>
        /// <returns>Indicator that RTask has completed</returns>
        /// <remarks></remarks>
        Boolean isDone();

        /// <summary>
        /// RTask cancel status
        /// </summary>
        /// <returns>Indicator if RTask has been cancelled</returns>
        /// <remarks></remarks>
        Boolean isCancelled();

        /// <summary>
        /// Cancel an RTask
        /// </summary>
        /// <param name="mayInterruptIfRunning">Indicator if the task can be interrupted</param>
        /// <returns>Indicator that a Cancel command has been sent</returns>
        /// <remarks></remarks>
        Boolean cancel(Boolean mayInterruptIfRunning);
    }
}
