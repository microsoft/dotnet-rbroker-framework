/*
 * DiscreteSimulation.cs
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
using DeployR;
using DeployRBroker;

namespace Discrete
{
    /*
     * DiscreteSimulation
     *
     * So far these examples have demonstrated how to execute 
     * and handle the result of a single RTask.
     *
     * How do we scale up using the RBroker programming model?
     *
     * We need to solve to two technical challenges:
     *
     * 1. How do we build a client application to test DeployR scale?
     * 1. How does a client application handle RTask results at scale?
     *
     * This example demonstrates how RTaskAppSimulator can be
     * used to simulate arbitrarily large RTask throughput for
     * task-testing or load-testing.
     *
     * Using an RTaskListener demonstrates how to handle
     * RTask results at scale.
     *
     * Steps:
     *
     * 1. Using RBroker
     * 2. Register an RTaskAppSimulator, an RTaskListener
     * and an RBrokerListener.
     * 3. To simulate the execution of many RTask
     * 4. Retrieve RTask results on asynchronous callback.
     * 5. And observe RBroker built-in profiling capabilities.
     */
    
    public class DiscreteSimulation
    {
        static public void Execute()
        {
            try 
            {

                /*
                 * 1. Create RBroker instance using RBrokerFactory.
                 *
                 * This example creates an anonymous DiscreteTaskBroker.
                 */

                DiscreteBrokerConfig brokerConfig = new DiscreteBrokerConfig(Program.DEPLOYR_ENDPOINT, null, 5);

                RBroker rBroker = RBrokerFactory.discreteTaskBroker(brokerConfig);


                /*
                 * 2. Create an instance of RTaskAppSimulator. It will drive
                 * RTasks through the RBroker.
                 */

                SampleAppSimulation simulation = new SampleAppSimulation(rBroker);


                /*
                 * 3. Launch RTaskAppSimulator simulation.
                 */
                rBroker.simulateApp(simulation);

                /*
                 * 4. Block until all tasks are complete, and shutdown has been called
                 */
                rBroker.waitUntilShutdown();

            } 
            catch(Exception tex) 
            {
                Console.WriteLine("constructor: ex=" + tex.ToString());
            }

        }
    }
}
