/*
 * BackgroundBrokerConfig.cs
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
    /// Configuration options for a Background Task
    /// </summary>
    /// <remarks></remarks>
    public class BackgroundBrokerConfig : RBrokerConfig
    {
        private static int MAX_CONCURRENCY = 999;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected BackgroundBrokerConfig()
        {
        }

        /// <summary>
        /// Constructor for specifying a configuration options for a Background Task
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost:7300/deployr )</param>
        /// <param name="userCredentials">RAuthentication object containing the user credentials</param>
        /// <remarks></remarks>
        public BackgroundBrokerConfig(String deployrEndpoint, RAuthentication userCredentials)
            : base(deployrEndpoint, userCredentials, MAX_CONCURRENCY)
        {
        }
    }
}
