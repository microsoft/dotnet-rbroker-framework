/*
 * DiscreteBrokerConfig.cs
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
    /// Configuration options for a Discrete Task
    /// </summary>
    /// <remarks></remarks>
    public class DiscreteBrokerConfig : RBrokerConfig
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected DiscreteBrokerConfig()
        {
        }

        /// <summary>
        /// Constructor for specifying a configuration options for a Descrete Task
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost/deployr )</param>
        /// <remarks></remarks>
        public DiscreteBrokerConfig(String deployrEndpoint)
             : base(deployrEndpoint, null)
        {            
        }

        /// <summary>
        /// Constructor for specifying a configuration options for a Descrete Task
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost/deployr )</param>
        /// <param name="userCredentials">RAuthentication object containing the user credentials</param>
        /// <remarks></remarks>
        public DiscreteBrokerConfig(String deployrEndpoint,
                           RAuthentication userCredentials)
             : base(deployrEndpoint, userCredentials, 1)
        {
        }

        /// <summary>
        /// Constructor for specifying a configuration options for a Descrete Task
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost/deployr )</param>
        /// <param name="userCredentials">RAuthentication object containing the user credentials</param>
        /// <param name="maxConcurrentTaskLimit">integer containing the maximum concurrent task limit</param>
        /// <remarks></remarks>
        public DiscreteBrokerConfig(String deployrEndpoint,
                           RAuthentication userCredentials,
                           int maxConcurrentTaskLimit)
            : base(deployrEndpoint, userCredentials, maxConcurrentTaskLimit) 
        {
        }
    }
}
