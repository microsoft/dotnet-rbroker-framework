/*
 * DiscreteProfiling.cs
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

namespace Discrete
{
   /*
    * DiscreteProfiling
    *
    * Scenario:
    *
    * RBroker "Hello World" example that demonstrates
    * runtime statistics
    * 1. Using RBroker
    * 2. Register RTaskListener for asynchronous
    * notifications on RTask completion events.
    * 2. Register RBrokerListener for asynchronous
    * notifications on RBroker statistic events.
    * 3. Execute a single RTask.
    * 4. And get notified of RTask completion plus
    * RBroker statistics events.
    */

    public class DiscreteProfiling
    {
        static public void Execute()
        {
            try {

                /*
                 * 1. Create RBroker instance using RBrokerFactory.
                 *
                 * This example creates a DiscreteTaskBroker.
                 */

                DiscreteBrokerConfig brokerConfig = new DiscreteBrokerConfig(Program.DEPLOYR_ENDPOINT);
                RBroker rBroker = RBrokerFactory.discreteTaskBroker(brokerConfig);

                /*
                 * 2. Register RTaskListener for asynchronous
                 * notifications on RTask completion.
                 */

                SampleTaskBrokerListener sampleListeners = new SampleTaskBrokerListener(rBroker);

                rBroker.addTaskListener(sampleListeners);
                rBroker.addBrokerListener(sampleListeners);


                /*
                 * 3. Define RTask
                 *
                 * This example creates a DiscreteTask that will
                 * execute an R script, /testuser/tutorial-rbroker/5SecondNoOp.
                 */

                RTask rTask = RTaskFactory.discreteTask(Program.TUTORIAL_NOOP_SCRIPT,
                                                        Program.TUTORIAL_REPO_DIRECTORY,
                                                        Program.TUTORIAL_REPO_OWNER,
                                                        "",
                                                        null);

                /*
                 * 4. Submit RTask to RBroker for execution.
                 *
                 * The RTaskToken is returned immediately. You can
                 * use the token to track the progress of RTask
                 * and/or block while waiting for a result.
                 *
                 * However, in this example we are going to allow
                 * the RTaskListener handle the result so 
                 * there is nothing further for us to do here after
                 * we submit the RTask.
                 */

                RTaskToken rTaskToken = rBroker.submit(rTask);

                Console.WriteLine("DiscreteProfiling: submitted " + rTask +
                        " for execution on RBroker.");


            } 
            catch(Exception tex) 
            {
                Console.WriteLine("DiscreteProfiling: ex=" + tex.ToString());
            }
        }

        private class SampleTaskBrokerListener : RTaskListener, RBrokerListener 
        {

            RBroker m_rBroker;
            
            public SampleTaskBrokerListener(RBroker rBroker)
            {
                m_rBroker = rBroker;
            }

            public void onTaskCompleted(RTask rTask, RTaskResult rTaskResult) 
            {
                Console.WriteLine("DiscreteProfiling: onTaskCompleted: " + rTask + ", result: " + rTaskResult.getTimeOnCall());
                RBrokerStatsHelper.printRTaskResult(rTask, rTaskResult, null);
            }

            public void onTaskError(RTask rTask, String error) 
            {
                Console.WriteLine("DiscreteProfiling: onTaskError: " + rTask + ", error: " + error);
                RBrokerStatsHelper.printRTaskResult(rTask, null, error);
            }

            public void onRuntimeError(String error) 
            {
                Console.WriteLine("\nonRuntimeError: error: " + error);
            }

            public void onRuntimeStats(RBrokerRuntimeStats stats, int maxConcurrency) 
            {
                RBrokerStatsHelper.printRBrokerStats(stats, maxConcurrency);
                m_rBroker.shutdown();
                Console.WriteLine("DiscreteProfiling: rBroker has been shutdown.");
            }
        }
    }
}
