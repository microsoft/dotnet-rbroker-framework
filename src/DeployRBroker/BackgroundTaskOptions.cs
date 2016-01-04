/*
 * BackgroundTaskOptions.cs
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
    /// Options used when creating a DeployR Background Task
    /// </summary>
    /// <remarks></remarks>

    public class BackgroundTaskOptions : TaskOptions
    {
        private int m_repeatCount = 0;
        private Boolean m_noProject = false;
        private long m_repeatInterval = 0;
        private long m_startTime = 0;

        /// <summary>
        /// Background task schedule repeat count.
        /// </summary>
        /// <value>repeat count value</value>
        /// <returns>repeat count value</returns>
        /// <remarks></remarks>
        public int repeatCount
        {
            get
            {
                return m_repeatCount;
            }
            set
            {
                m_repeatCount = value;
            }
        }
        /// <summary>
        /// Enable noProject option if project persistence
        /// following task execution is not required.
        /// </summary>
        /// <value>no project value</value>
        /// <returns>no project value</returns>
        /// <remarks></remarks>
        public Boolean noProject
        {
            get
            {
                return m_noProject;
            }
            set
            {
                m_noProject = value;
            }
        }

        /// <summary>
        /// Background task schedule repeat interval
        /// </summary>
        /// <value>schedule repeat interval value</value>
        /// <returns>schedule repeat interval value</returns>
        /// <remarks></remarks>
        public long repeatInterval
        {
            get
            {
                return m_repeatInterval;
            }
            set
            {
                m_repeatInterval = value;
            }
        }

        /// <summary>
        /// Background task schedule start time in UTC
        /// </summary>
        /// <value>task schedule start time value</value>
        /// <returns>task schedule start time value</returns>
        /// <remarks></remarks>
        public long startTime
        {
            get
            {
                return m_startTime;
            }
            set
            {
                m_startTime = value;
            }
        }
    }
}
