/*
 * PooledBrokerConfig.cs
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
    /// Configuration options for a Pooled Task
    /// 
    /// Specifies the set of pre-initialization operations to be
    /// performed on each R Session in the pool at creation time.
    ///
    /// For example, R workspace data, such as R models, can be
    /// preloaded into each R Session in the pool at startup.
    /// Data files, such as CSV, XLS can be preloaded into the
    /// working directory for each R Session in the pool at startup.
    ///
    /// Preloading binary R data or file data at startup ensures
    /// the overhead associated with runtime dependencies for each
    /// task can be kept to a minimum at runtime.
    ///
    /// Using pool creation options intelligently can greatly help
    /// to improve overall throughput on the DeployR server.
    /// </summary>
    /// <remarks></remarks>
    public class PooledBrokerConfig : RBrokerConfig
    {
        private PoolCreationOptions m_poolCreationOptions;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected PooledBrokerConfig()
        {
        }

        /// <summary>
        /// Constructor for specifying a configuration options for a Pooled Task
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost/deployr )</param>
        /// <param name="userCredentials">RAuthentication object containing the user credentials</param>
        /// <remarks></remarks>
        public PooledBrokerConfig(String deployrEndpoint,
                                 RAuthentication userCredentials)
            : base(deployrEndpoint, userCredentials, 1)
        {

            m_poolCreationOptions = null;
        }

        /// <summary>
        /// Constructor for specifying a configuration options for a Pooled Task
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost/deployr )</param>
        /// <param name="userCredentials">RAuthentication object containing the user credentials</param>
        /// <param name="maxConcurrentTaskLimit">integer containing the maximum concurrent task limit</param>
        /// <remarks></remarks>
        public PooledBrokerConfig(String deployrEndpoint,
                                  RAuthentication userCredentials,
                                  int maxConcurrentTaskLimit)
            : base(deployrEndpoint, userCredentials, maxConcurrentTaskLimit)
        {


            m_poolCreationOptions = null;
        }

        /// <summary>
        /// Constructor for specifying a configuration options for a Pooled Task
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost/deployr )</param>
        /// <param name="userCredentials">RAuthentication object containing the user credentials</param>
        /// <param name="maxConcurrentTaskLimit">integer containing the maximum concurrent task limit</param>
        /// <param name="poolCreationOptions">options used for the Pooled task creation</param>
        /// <remarks></remarks>
        public PooledBrokerConfig(String deployrEndpoint,
                                  RAuthentication userCredentials,
                                  int maxConcurrentTaskLimit,
                                  PoolCreationOptions poolCreationOptions)
            : base(deployrEndpoint, userCredentials, maxConcurrentTaskLimit)
        {

            m_poolCreationOptions = poolCreationOptions;
        }

        /// <summary>
        /// Pooled task creation options
        /// </summary>
        /// <value>options used to create the Pooled task </value>
        /// <returns>options used to create the Pooled task</returns>
        /// <remarks></remarks>
        public PoolCreationOptions poolCreationOptions
        {
            get
            {
                return m_poolCreationOptions;
            }
            set
            {
                m_poolCreationOptions = value;
            }
        }
    }
}
