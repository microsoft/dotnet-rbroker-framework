/*
 * RBrokerRuntimeStats.cs
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
    /// Summary of RTask statistics for live RBroker instances, made available on  RBrokerListener.onRuntimeStats
    /// </summary>
    /// <remarks></remarks>
    public class RBrokerRuntimeStats
    {

        private long m_totalTasksRun = 0;
        private long m_totalTasksRunToSuccess = 0;
        private long m_totalTasksRunToFailure = 0;
        private long m_totalTimeTasksOnCode = 0;
        private long m_totalTimeTasksOnServer = 0;
        private long m_totalTimeTasksOnCall = 0;

        /// <summary>
        /// Reutrns a string representation of the RBrokerRuntimeStats
        /// </summary>
        /// <value></value>
        /// <returns>Reutrns a string representation of the RBrokerRuntimeStats</returns>
        /// <remarks></remarks>
        public String toString()
        {
            return "\nRBrokerRuntimeStats:\n" +
                    "totalTasksRun: " + m_totalTasksRun +
                    "\ntotalTasksRunToSuccess: " + m_totalTasksRunToSuccess +
                    "\ntotalTasksRunToFailure: " + m_totalTasksRunToFailure +
                    "\ntotalTimeTasksOnCode: " + m_totalTimeTasksOnCode +
                    "\ntotalTimeTasksOnServer: " + m_totalTimeTasksOnServer +
                    "\ntotalTimeTasksOnCall: " + m_totalTimeTasksOnCall +
                    "\n";
        }

        /// <summary>
        /// Total number of RTasks run by an RBroker instance
        /// </summary>
        /// <value></value>
        /// <returns>Total number of RTasks run</returns>
        /// <remarks></remarks>
        public long totalTasksRun
        {
            get
            {
                return m_totalTasksRun;
            }
            set
            {
                m_totalTasksRun = value;
            }
        }

        /// <summary>
        /// Total number of RTasks run successfully by an RBroker instance
        /// </summary>
        /// <value></value>
        /// <returns>Total number of RTasks run successfully</returns>
        /// <remarks></remarks>
        public long totalTasksRunToSuccess
        {
            get
            {
                return m_totalTasksRunToSuccess;
            }
            set
            {
                m_totalTasksRunToSuccess = value;
            }
        }

        /// <summary>
        /// Total number of RTasks run that resulted in failure by an RBroker instance
        /// </summary>
        /// <value></value>
        /// <returns>Total number of RTasks run that resulted in failure</returns>
        /// <remarks></remarks>
        public long totalTasksRunToFailure
        {
            get
            {
                return m_totalTasksRunToFailure;
            }
            set
            {
                m_totalTasksRunToFailure = value;
            }
        }

        /// <summary>
        /// Total time taken on DeployR to execute the R code 
        /// The difference between totalTimeTasksOnServer and totalTimeTasksOnCode gives a good indication
        /// of the DeployR server-side overhead processing the execution.
        /// </summary>
        /// <value></value>
        /// <returns>Total time to execute R code</returns>
        /// <remarks></remarks>
        public long totalTimeTasksOnCode
        {
            get
            {
                return m_totalTimeTasksOnCode;
            }
            set
            {
                m_totalTimeTasksOnCode = value;
            }
        }

        /// <summary>
        /// Total time taken on DeployR to process RTaks successfully
        /// The difference between totalTimeTasksOnCall and totalTimeTasksOnServer gives a good indication
        /// of the network latency between your application and the DeployR server.
        /// </summary>
        /// <value></value>
        /// <returns>Total to process RTasks successfully</returns>
        /// <remarks></remarks>
        public long totalTimeTasksOnServer
        {
            get
            {
                return m_totalTimeTasksOnServer;
            }
            set
            {
                m_totalTimeTasksOnServer = value;
            }
        }

        /// <summary>
        /// Total time taken on call to DeployR to process RTasks successfully
        /// The difference between totalTimeTasksOnCall and totalTimeTasksOnServer gives a good indication
        /// of the network latency between your application and the DeployR server.
        /// </summary>
        /// <value></value>
        /// <returns>Total number of RTasks run</returns>
        /// <remarks></remarks>
        public long totalTimeTasksOnCall
        {
            get
            {
                return m_totalTimeTasksOnCall;
            }
            set
            {
                m_totalTimeTasksOnCall = value;
            }
        }
    }
}
