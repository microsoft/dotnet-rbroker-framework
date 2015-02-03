/*
 * DiscreteAsynchronous.cs
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
using System.Threading;
using DeployRBroker;

namespace Discrete
{
    /*
     * DiscreteAsynchronous
     *
     * Scenario:
     *
     * RBroker "Hello World" example that demonstrates
     * the basic RBroker programming model:
     * 1. Using RBroker
     * 2. Register RTaskListener for asynchronous
     * notifications on RTask completion events.
     * 3. Execute a single RTask
     * 4. And get notified of RTask completion.
     */
    
    public class DiscreteAsynchronous
    {
        static public void Execute()
        {
            try
            {

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

                SampleTaskListener myTaskListener = new SampleTaskListener(rBroker);

                rBroker.addTaskListener(myTaskListener);


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

                Console.WriteLine("DiscreteAsynchronous: submitted " + rTask + " for execution on RBroker.");

                /*
                 * 5. Block until all tasks are complete, and shutdown has been called
                 */
                rBroker.waitUntilShutdown();

            }
            catch (Exception tex)
            {
                Console.WriteLine("DiscreteAsynchronous: ex=" + tex.ToString());
            }
        }

        private class SampleTaskListener : RTaskListener
        {
            RBroker m_rBroker;

            public SampleTaskListener(RBroker rBroker)
            {
                m_rBroker = rBroker;
            }

            public void onTaskCompleted(RTask rTask, RTaskResult rTaskResult)
            {
                Console.WriteLine("DiscreteAsynchronous: onTaskCompleted: " + rTask.ToString() 
                    + ", result: " + rTaskResult.getTimeOnCall());

                if (m_rBroker != null)
                {
                    m_rBroker.shutdown();
                    Console.WriteLine("DiscreteAsynchronous: rBroker has been shutdown.");
                }
            }

            public void onTaskError(RTask rTask, String error)
            {
                Console.WriteLine("DiscreteAsynchronous: onTaskError: " + rTask + ", error: " + error);
            }
        }
    }
}
