/*
 * Program.cs
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

namespace Pooled
{
    class Program
    {
        public const String TUTORIAL_REPO_OWNER = "testuser";
        public const String TUTORIAL_REPO_DIRECTORY = "tutorial-rbroker";
        public const String TUTORIAL_RTSCORE_SCRIPT = "rtScore.R";
        public const String TUTORIAL_INSURANCE_MODEL = "insurModel.rData";
        public const String AUTH_USER_NAME = "testuser";
        public const String AUTH_PASSWORD = "changeme";
        public const String DEPLOYR_ENDPOINT = "http://localhost:7400/deployr";

        static void Main(string[] args)
        {
            //RClientFactory.setDebugMode(true);  //un-comment this line to see the HTTP calls on the API
            PooledSimulation.Execute();
        }
    }
}
