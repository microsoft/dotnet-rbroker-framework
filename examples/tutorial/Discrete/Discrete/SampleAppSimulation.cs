using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeployRBroker;

namespace Discrete
{
    public class SampleAppSimulation : RTaskAppSimulator, RTaskListener, RBrokerListener 
    {

        long SIMULATE_TOTAL_TASK_COUNT = 10L;
        long simulationStartTime = 0L;
        RBroker m_rBroker;
        

        public SampleAppSimulation(RBroker rBroker) 
        {
            m_rBroker = rBroker;
        }

        /*
         * RTaskAppSimulator method.
         */

        public void simulateApp(RBroker rBroker) 
        {
            /*
             * 1. Prepare RTask(s) for simulation.
             *
             * In the example we will simply simulate the execution
             * a fixed number of RTask.
             *
             * Note, this is a somewhat artificial demo as we are
             * executing the same RTask SIMULATE_TOTAL_TASK_COUNT
             * times. You can experiment by modifying the 
             * implementation to execute a range of RTask of 
             * your own choosing.
             */

            RTask rTask = RTaskFactory.discreteTask(Program.TUTORIAL_NOOP_SCRIPT,
                                                    Program.TUTORIAL_REPO_DIRECTORY,
                                                    Program.TUTORIAL_REPO_OWNER,
                                                    "",
                                                    null);

            /*
             * 2. Loop submitting SIMULATE_TOTAL_TASK_COUNT
             * task(s) to RBroker for execution.
             */

            simulationStartTime = System.Environment.TickCount;

            for(int tasksPushedToBroker = 0;
                    tasksPushedToBroker<SIMULATE_TOTAL_TASK_COUNT;
                    tasksPushedToBroker++) 
            {

                try 
                {

                    RTaskToken taskToken = rBroker.submit(rTask);
                    Console.WriteLine("simulateApp: submitted task " +
                            rTask + " for execution on RBroker.");

                } 
                catch(Exception ex) 
                {
                    Console.WriteLine("simulateApp: ex=" + ex.ToString());
                }
            }
        }

        /*
         * RBrokerAsyncListener methods.
         */

        public void onTaskCompleted(RTask rTask, RTaskResult rTaskResult) 
        {
            RBrokerStatsHelper.printRTaskResult(rTask, rTaskResult, null);

        }

        public void onTaskError(RTask rTask, String error)
        {
            RBrokerStatsHelper.printRTaskResult(rTask, null, error);
        }

        public void onRuntimeError(String error) 
        {
            Console.WriteLine("\nonRuntimeError: error: " + error + "\n");
        }

        public void onRuntimeStats(RBrokerRuntimeStats stats, int maxConcurrency) 
        {
            RBrokerStatsHelper.printRBrokerStats(stats, maxConcurrency);

            if(stats.totalTasksRun == SIMULATE_TOTAL_TASK_COUNT) 
            {
                Console.WriteLine("SampleAppSimulation: simulation, total time taken " +
                    (System.Environment.TickCount - simulationStartTime) + " ms.");

                m_rBroker.shutdown();
                Console.WriteLine("SampleAppSimulation: rBroker has been shutdown.");
            }
        }
    }
}
