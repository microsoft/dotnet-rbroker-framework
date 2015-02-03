/*
 * RTaskType.cs
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
    /// Class containing RTask type constants.
    /// </summary>
    /// <remarks></remarks>
    public class RTaskType
    {
        /// <summary>Background TaskType</summary>
        public const int BACKGROUND = 0;
        /// <summary>Discrete TaskType</summary>
        public const int DISCRETE = 1;
        /// <summary>Pooled TaskType</summary>
        public const int POOLED = 2;
    }
}
