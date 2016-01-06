/*
 * RBrokerListener.cs
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

namespace DeployRBroker
{
    /// <summary>
    /// Runtime options for a Pooled Task.
    /// </summary>
    /// <remarks></remarks>
    public interface RBrokerListener
    {
        /// <summary>
        /// Asynchronous callback notification when a runtime
        /// error occurs within RBroker.
        /// </summary>
        /// <param name="error">String indicating error that occurred</param>
        /// <remarks></remarks>
        void onRuntimeError(String error);

        /// <summary>
        /// Asynchronous callback notification RBrokerRuntimeStats.
        /// </summary>
        /// <param name="stats">RBrokerRuntimeStats reference</param>
        /// <param name="maxConcurrency">Maximum concurrency allowed</param>
        /// <remarks></remarks>
        void onRuntimeStats(RBrokerRuntimeStats stats, int maxConcurrency);
    }
}
