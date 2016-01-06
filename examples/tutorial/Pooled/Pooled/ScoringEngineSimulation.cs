/*
 * ScoringEngineSimulation.cs
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
using System.Threading;
using DeployR;
using DeployRBroker;

namespace Pooled
{
    /*
    * This ScoringEngineSimulation is a simple extension
    * to the SampleAppSimulation class used by PooledSimulation.
    *
    * The addition of the SIMULATE_TASK_RATE_PER_MINUTE parameter
    * allows the app simulation to mimick more "real world"
    * workloads by adjusting the rate at which RTask are 
    * submitted to the RBroker for execution.
    *
    * Note, this simulation also demonstrates how input values
    * can be passed to each RTask and how output values can
    * be retrieved following the execution of each task.
    *
    * In this example, a "customerid" input value is passed on
    * each RTask. While this example passes a simple index
    * value for "customerid" a "real world" app would like pass
    * a database record id along with additional customer specific
    * data parameters.
    *
    * In this example, the value of the "score" is returned as a
    * DeployR-encoded R object and made available on the result
    * for the RTask.
    */
    
    public class ScoringEngineSimulation : RTaskListener, RBrokerListener, RTaskAppSimulator 
    {
        private int SIMULATE_TOTAL_TASK_COUNT = 100;
        private int SIMULATE_TASK_RATE_PER_MINUTE = 0;
        private RBroker m_rBroker = null;
        private int simulationStartTime = 0;

        public ScoringEngineSimulation(RBroker rBroker) 
        {
            m_rBroker = rBroker;
        }

        /*
         * RTaskAppSimulator method.
         */

        public void simulateApp(RBroker rBroker) 
        {

            /*
             * 2. Submit task(s) to RBroker for execution.
             */

            Console.WriteLine("About to simulate " +
                    SIMULATE_TOTAL_TASK_COUNT + " tasks at a rate of " +
                    SIMULATE_TASK_RATE_PER_MINUTE + " tasks per minutes.");

            simulationStartTime = System.Environment.TickCount;

            for(int tasksPushedToBroker = 0;
                    tasksPushedToBroker<SIMULATE_TOTAL_TASK_COUNT;
                    tasksPushedToBroker++) 
            {
                try {

                    /*
                     * 1. Prepare RTask for real-time scoring.
                     *
                     * In this example, we pass along a unique
                     * customer ID with each RTask. In a real-world
                     * application the input parameters on each RTask
                     * will vary depending on need, such as customer
                     * database record keys and supplimentary parameter
                     * data to facilitate the scoring.
                     */

                    PooledTaskOptions taskOptions = new PooledTaskOptions();
                    taskOptions.routputs.Add("score");
                    taskOptions.rinputs.Add(RDataFactory.createNumeric("customerid", tasksPushedToBroker));

                    RTask rTask = RTaskFactory.pooledTask(Program.TUTORIAL_RTSCORE_SCRIPT,
                                                Program.TUTORIAL_REPO_DIRECTORY,
                                                Program.TUTORIAL_REPO_OWNER,
                                                null, taskOptions);

                    RTaskToken taskToken = rBroker.submit(rTask);
                    Console.WriteLine("Submitted task " + rTask + "\n");

                    /*
                     * If further tasks need to be pushed to broker
                     * then delay for staggeredLoadInterval to simulate
                     * control of task flow rate.
                     */
                    if(tasksPushedToBroker < (SIMULATE_TOTAL_TASK_COUNT - 1)) 
                    {
                        try {

                            if(SIMULATE_TASK_RATE_PER_MINUTE != 0L)
                            {
                                int staggerLoadInterval = 60 / SIMULATE_TASK_RATE_PER_MINUTE;
                                Thread.Sleep(staggerLoadInterval * 1000);
                            }

                        } 
                        catch(Exception iex) 
                        {
                            Console.WriteLine("Runtime exception=" + iex.ToString());
                        }
                    }
                } 
                catch(Exception ex) 
                {
                    Console.WriteLine("Runtime exception=" + ex.ToString());
                }
            }

        }

        /*
         * RBrokerAsyncListener methods.
         */

        public void onTaskCompleted(RTask rTask, RTaskResult rTaskResult) 
        {
            RBrokerStatsHelper.printRTaskResult(rTask, rTaskResult, null);
            Console.WriteLine("onTaskCompleted: " + rTask + ", score " +
                ((RNumeric)rTaskResult.getGeneratedObjects()[0]).Value);
        }

        public void onTaskError(RTask rTask, String error) 
        {
            RBrokerStatsHelper.printRTaskResult(rTask, null, error);
        }

        public void onRuntimeError(String error) 
        {
            Console.WriteLine("onRuntimeError: error: " + error);
        }

        public void onRuntimeStats(RBrokerRuntimeStats stats, int maxConcurrency) 
        {
            RBrokerStatsHelper.printRBrokerStats(stats, maxConcurrency);

            if(stats.totalTasksRun == SIMULATE_TOTAL_TASK_COUNT) 
            {
                Console.WriteLine("Simulation, total time taken " +
                    (System.Environment.TickCount - simulationStartTime) + " ms.");

                m_rBroker.shutdown();
                Console.WriteLine("rBroker has been shutdown.");
            }
        }
    }
}
