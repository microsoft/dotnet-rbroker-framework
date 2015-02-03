/*
 * RTask.cs
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
    /// Represents any R Analytics task for execution on an RBroker instance
    /// </summary>
    /// <remarks></remarks>
    public interface RTask
    {
        /*
         *  Tokens can be used to "tag" an RTask with a client application
         * specific token. These tokens can be then be used by a client
         * application when an RTask completes to decide how best to process
         * or direct the RTaskResult.
         *
         * For example, if multiple "services" within a single application
         * are using a single instance of RBroker, then each RTask submitted to
         * that RBroker could be tagged with the name of the originating service.
         */
        
        /// <summary>
        /// RTask token, optionally assigned by a client application as a form of "tag" on the task.
        /// </summary>
        /// <returns>Token Object</returns>
        /// <remarks></remarks>
        Object getToken();

        /// <summary>
        /// RTask token, optionally assigned by a client application as a form of "tag" on the task.
        /// </summary>
        /// <param name="token">Token object</param>
        /// <returns></returns>
        /// <remarks></remarks>
        void setToken(Object token);
    }
}
