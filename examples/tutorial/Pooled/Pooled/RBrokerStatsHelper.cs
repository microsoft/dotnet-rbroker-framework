/*
 * RBrokerStatsHelper.cs
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
using DeployRBroker;

namespace Pooled
{
    public class RBrokerStatsHelper
    {
        /**
         * Prints {@link com.revo.deployr.client.broker.RBrokerRuntimeStats}
         * to console output.
         */
        public static void printRBrokerStats(RBrokerRuntimeStats stats, int maxConcurrency)
        {

            Console.WriteLine("\n\nRBroker Activity Summary");
            Console.WriteLine("RBroker: Max Concurrency [ " + maxConcurrency + " ]");
            Console.WriteLine("RBroker: Total Tasks Run [ " + stats.totalTasksRun + " ]");

            Console.WriteLine("RBroker: Tasks Ok [ " +
                    stats.totalTasksRunToSuccess + " ] Fail [ " +
                    stats.totalTasksRunToFailure + " ]");

            long displayAvgTimeOnCode = 0L;
            long displayAvgTimeOnServer = 0L;
            long displayAvgTimeOnCall = 0L;

            if (stats.totalTasksRunToSuccess > 0)
            {
                displayAvgTimeOnCode = stats.totalTimeTasksOnCode / stats.totalTasksRunToSuccess;
                displayAvgTimeOnServer = stats.totalTimeTasksOnServer / stats.totalTasksRunToSuccess;
                displayAvgTimeOnCall = stats.totalTimeTasksOnCall / stats.totalTasksRunToSuccess;
            }

            Console.WriteLine("RBroker: Task Average Time On Code [ " + displayAvgTimeOnCode + " ]");
            Console.WriteLine("RBroker: Task Average Time On Server [ " + displayAvgTimeOnServer + " ]");
            Console.WriteLine("RBroker: Task Average Time On Call   [ " + displayAvgTimeOnCall + " ]\n");
        }

        /**
            * Prints {@link com.revo.deployr.client.broker.RTaskResult}
            * to console output.
            */
        public static void printRTaskResult(RTask task, RTaskResult result, String error)
        {

            Console.WriteLine("\nTask: " + task);

            if (error != null)
            {
                Console.WriteLine("Status[fail]: cause=" + error);
            }
            else
            {

                switch (result.getType())
                {

                    case RTaskType.DISCRETE:
                        if (result.isSuccess())
                        {
                            Console.WriteLine("Status[ok]: [ code : " +
                                    result.getTimeOnCode() + " , server : " +
                                    result.getTimeOnServer() + " , call : " +
                                    result.getTimeOnCall() + " ]");
                        }
                        else
                        {
                            Console.WriteLine("Status[fail]: cause=" + result.getFailure());
                        }
                        break;

                    case RTaskType.POOLED:
                        if (result.isSuccess())
                        {
                            Console.WriteLine("Status[ok]: [ code : " +
                                    result.getTimeOnCode() + " , server : " +
                                    result.getTimeOnServer() + " , call : " +
                                    result.getTimeOnCall() + " ]");
                        }
                        else
                        {
                            Console.WriteLine("Status[fail]: cause=" + result.getFailure());
                        }
                        break;

                    case RTaskType.BACKGROUND:
                        if (result.isSuccess())
                        {
                            Console.WriteLine("Status[ok]: [ server : " +
                                    result.getTimeOnServer() + " , call : " +
                                    result.getTimeOnCall() + " ]");
                        }
                        else
                        {
                            Console.WriteLine("Status[fail]: cause=" + result.getFailure());
                        }
                        break;

                }
            }
        }
    }
}
