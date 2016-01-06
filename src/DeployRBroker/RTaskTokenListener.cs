/*
 * RTaskTokenListener.cs
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
    /// Represents a handle to an RTaskTokenListner
    /// </summary>
    /// <remarks></remarks>
    public interface RTaskTokenListener
    {
        /// <summary>
        /// Asynchronous callback notification when a TaskToken is returned to the pool.
        /// </summary>
        /// <param name="task">The RTask instance</param>
        /// <param name="future">The Operating System task reference</param>
        /// <remarks></remarks>
        void onTask(RTask task, Task<RTaskResult> future);

    }
}
