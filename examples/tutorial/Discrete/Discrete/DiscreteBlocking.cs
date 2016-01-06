/*
 * DiscreteBlocking.cs
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
     * DiscreteBlocking
     *
     * Scenario:
     *
     * RBroker "Hello World" example that demonstrates
     * the basic RBroker programming model:
     * 1. Using RBroker
     * 2. To execute a single RTask
     * 3. And block for the RTask result.
     */
    
    public class DiscreteBlocking
    {
        static public void Execute()
        {
            RBroker rBroker = null;

            try 
            {

                /*
                 * 1. Create RBroker instance using RBrokerFactory.
                 *
                 * This example creates a DiscreteTaskBroker.
                 */

                DiscreteBrokerConfig brokerConfig = new DiscreteBrokerConfig(Program.DEPLOYR_ENDPOINT);
                rBroker = RBrokerFactory.discreteTaskBroker(brokerConfig);


                /*
                 * 2. Define RTask
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
                 * 3. Submit RTask to RBroker for execution.
                 *
                 * Note, unlike an RClient.executeScript call or
                 * an RProject.executeScript call the RBroker.submit
                 * call is non-blocking.
                 *
                 * The RTaskToken is returned immediately. You can
                 * use the token to track the progress of RTask
                 * and/or block while waiting for a result.
                 */

                RTaskToken rTaskToken = rBroker.submit(rTask);

                Console.WriteLine("DiscreteBlocking: submitted " + rTask + " for execution on RBroker.");

                /*
                 * 4. Demonstrate blocking for an RTask result.
                 *
                 * The call to getResult() will either return
                 * an RTaskResult or raise an Exception. An Exception
                 * indicates the RTask failed to complete and why.
                 */

                RTaskResult rTaskResult = rTaskToken.getResult();

                Console.WriteLine("DiscreteBlocking: " + rTask + " completed, result=" + rTaskResult.getTimeOnCall());


            } catch(Exception tex)
            {
                Console.WriteLine("DiscreteBlocking: error=" + tex.ToString());
            } 
            finally 
            {
                /*
                 * Final Step: Shutdown RBroker to release
                 * all associated resources, connections.
                 */
                if(rBroker != null) 
                {
                    rBroker.shutdown();
                    Console.WriteLine("DiscreteBlocking: rBroker has been shutdown.");
                }
            }

        }
    }
}
