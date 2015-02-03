/*
 * RBrokerFactory.cs
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
    /// Factory class to create instance of the RBroker implementations (Discrete, Background or Pooled).
    /// </summary>
    /// <remarks></remarks>
    public class RBrokerFactory
    {
        /// <summary>
        /// Utility function for creating a Discrete Instance of RBroker
        /// </summary>
        /// <param name="brokerConfig">Discrete Broker Configuration object</param>
        /// <returns>DiscreteTaskBroker instance</returns>
        /// <remarks></remarks>
        public static RBroker discreteTaskBroker(DiscreteBrokerConfig brokerConfig)
        {
            return new DiscreteTaskBroker(brokerConfig);
        }

        /// <summary>
        /// Utility function for creating a Pooled Instance of RBroker
        /// </summary>
        /// <param name="brokerConfig">Pooled Broker Configuration object</param>
        /// <returns>PooledTaskBroker instance</returns>
        /// <remarks></remarks>
        public static RBroker pooledTaskBroker(PooledBrokerConfig brokerConfig)
        {
            return new PooledTaskBroker(brokerConfig);
        }

        /// <summary>
        /// Utility function for creating a Background Instance of RBroker
        /// </summary>
        /// <param name="brokerConfig">Background Broker Configuration object</param>
        /// <returns>BackgroundTaskBroker instance</returns>
        /// <remarks></remarks>
        public static RBroker backgroundTaskBroker(BackgroundBrokerConfig brokerConfig)
        {
            return new BackgroundTaskBroker(brokerConfig);
        }
    }
}
