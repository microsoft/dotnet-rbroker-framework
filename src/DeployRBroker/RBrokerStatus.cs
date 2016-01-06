/*
 * RBrokerStatus.cs
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
    /// RBroker status indicating number of currently queued and executing RTasks
    /// </summary>
    /// <remarks></remarks>
    public class RBrokerStatus
    {
        private int m_pendingTasks = 0;
        private int m_executingTasks = 0;
        private Boolean m_isIdle = false;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected RBrokerStatus()
        {
        }

        internal RBrokerStatus(int pendingTasks, int executingTasks) 
        {
            m_pendingTasks = pendingTasks;
            m_executingTasks = executingTasks;
            m_isIdle = (pendingTasks + executingTasks) == 0;
        }

        /// <summary>
        /// Reutrns a string representation of the RBrokerStatuss
        /// </summary>
        /// <value></value>
        /// <returns>Reutrns a string representation of the RBrokerStatuss</returns>
        /// <remarks></remarks>
        public String toString()
        {
            return "RBrokerStatus: [ " + m_pendingTasks +
                    " ] [ " + m_executingTasks +
                    " ] [ " + m_isIdle + " ]\n";
        }


        /// <summary>
        /// Number of RTasks currently on RBroker queues pending execution.
        /// </summary>
        /// <value></value>
        /// <returns>integer containing the pending tasks</returns>
        /// <remarks></remarks>
        public int pendingTasks
        {
            get
            {
                return m_pendingTasks;
            }
            set
            {
                m_pendingTasks = value;
            }
        }

        /// <summary>
        /// Number of RTasks currently executingon RBroker.
        /// </summary>
        /// <value></value>
        /// <returns>integer containing the executing tasks</returns>
        /// <remarks></remarks>
        public int executingTasks
        {
            get
            {
                return m_executingTasks;
            }
            set
            {
                m_executingTasks = value;
            }
        }

        /// <summary>
        /// Flag indicating if RBroker is idle. Idle indicates there are currently no pending or executing RTask
        /// An idle RBroker can be safely shutdown
        /// </summary>
        /// <value></value>
        /// <returns>Boolean indicating idle status</returns>
        /// <remarks></remarks>
        public Boolean isIdle
        {
            get
            {
                return m_isIdle;
            }
            set
            {
                m_isIdle = value;
            }
        }
    }
}
