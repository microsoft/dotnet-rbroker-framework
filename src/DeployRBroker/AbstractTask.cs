/*
 * AbstractTask.cs
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
    /// Represents an abstract RTask for execution on an RBroker.
    /// </summary>
    /// <remarks></remarks>
    public abstract class AbstractTask : RTask
    {
        /// <summary>
        /// RTask token, optionally assigned by a client application as a form of "tag" on the task.
        /// </summary>
        protected Object m_token;

        /// <summary>
        /// RTask token, optionally assigned by a client application as a form of "tag" on the task.
        /// </summary>
        /// <returns>Token Object</returns>
        /// <remarks></remarks>
        public Object getToken() 
        {
            return m_token;
        }

        /// <summary>
        /// RTask token, optionally assigned by a client application as a form of "tag" on the task.
        /// </summary>
        /// <param name="token">RTask token object</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public void setToken(Object token) 
        {
            m_token = token;
        }
    }
}
