/*
 * RBrokerConfig.cs
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
    /// Base class for RBroker configuratons.
    /// </summary>
    /// <remarks></remarks>
    public abstract class RBrokerConfig
    {
        private String m_deployrEndpoint = "";
        private int m_maxConcurrentTaskLimit = 10;
        private RAuthentication m_userCredentials;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected RBrokerConfig()
        {

        }

        /// <summary>
        /// Constructor that takes only a endpoint URL as input
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost:7300/deployr )</param>
        /// <remarks></remarks>
        protected RBrokerConfig(String deployrEndpoint)
        {
            m_deployrEndpoint = deployrEndpoint;
            m_userCredentials = null;
            m_maxConcurrentTaskLimit = 1;
        }

        /// <summary>
        /// Constructor that takes an endpoint URL and user credentials as inputs
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost:7300/deployr )</param>
        /// <param name="userCredentials">RAuthentication object containing the user credentials</param>
        /// <remarks></remarks>
        public RBrokerConfig(String deployrEndpoint,
                           RAuthentication userCredentials)
        {

            m_deployrEndpoint = deployrEndpoint;
            m_userCredentials = userCredentials;
            // Default: single threaded executor,
            // resulting in serial task execution.
            m_maxConcurrentTaskLimit = 1;
        }

        /// <summary>
        /// Constructor that takes an endpoint URL, user credentials and concurrent task limit as inputs
        /// </summary>
        /// <param name="deployrEndpoint">URL indicating the DeployR endpoint (i.e  http://localhost:7300/deployr )</param>
        /// <param name="userCredentials">RAuthentication object containing the user credentials</param>
        /// <param name="maxConcurrentTaskLimit">Maximum nuber of concurrent task for the RBroker instance</param>
        /// <remarks></remarks>
        public RBrokerConfig(String deployrEndpoint,
                           RAuthentication userCredentials,
                           int maxConcurrentTaskLimit)
        {

            m_deployrEndpoint = deployrEndpoint;
            m_userCredentials = userCredentials;
            m_maxConcurrentTaskLimit = maxConcurrentTaskLimit;
        }

        /// <summary>
        /// Gets the base URL used for all the API calls
        /// </summary>
        /// <value></value>
        /// <returns>String containg URL</returns>
        /// <remarks></remarks>
        public String deployrEndpoint
        {
            get
            {
                return m_deployrEndpoint;
            }
            set
            {
                m_deployrEndpoint = value;
            }
        }
        /// <summary>
        /// Gets the user credentials
        /// </summary>
        /// <value></value>
        /// <returns>User credentials</returns>
        /// <remarks></remarks>
        public RAuthentication userCredentials
        {
            get
            {
                return m_userCredentials;
            }
            set
            {
                m_userCredentials = value;
            }
        }
        /// <summary>
        /// Gets maximum number of concurrent tasks
        /// </summary>
        /// <value></value>
        /// <returns>integer containing maximum number of concurrent tasks</returns>
        /// <remarks></remarks>
        public int maxConcurrentTaskLimit
        {
            get
            {
                return m_maxConcurrentTaskLimit;
            }
            set
            {
                m_maxConcurrentTaskLimit = value;
            }
        }

    }
}
