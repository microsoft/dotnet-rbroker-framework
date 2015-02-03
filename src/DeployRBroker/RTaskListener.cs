/*
 * RTaskListener.cs
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

namespace DeployRBroker
{
    /// <summary>
    /// Asynchronous callback interface for RTask completion and error event listeners.
    /// </summary>
    /// <remarks></remarks>
    public interface RTaskListener
    {
        /// <summary>
        /// Callback function that is executed when RTask completes
        /// </summary>
        /// <param name="rTask">rTask reference</param>
        /// <param name="rTaskResult">RTaskResult reference that contains the results of the task</param>
        /// <remarks></remarks>
        void onTaskCompleted(RTask rTask, RTaskResult rTaskResult);

        /// <summary>
        /// Callback function that is executed when RTask completes
        /// </summary>
        /// <param name="rTask">rTask reference</param>
        /// <param name="error">String containing error description</param>
        /// <remarks></remarks>
        void onTaskError(RTask rTask, String error);
    }
}
