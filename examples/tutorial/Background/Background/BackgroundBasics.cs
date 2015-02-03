/*
 * BackgroundBasics.cs
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
using DeployR;

namespace Background
{
    /*
     * BackgroundBasics
     *
     * Scenario:
     *
     * RBroker "Hello World" example that demonstrates
     * the basic background RBroker programming model:
     * 1. Using RBroker
     * 2. Submitting RTask for background execution.
     * 3. Retrieving Job handle from RTaskResult.
     *
     * Important:
     *
     * To handle the results of a Background RTask you
     * must transition from using the RBroker Framework
     * API to using the .NET Client Library API. See 
     * example code below for details.
     */

    public class BackgroundBasics
    {
        static public void Execute()
        {
            try 
            {

                /*
                 * 1. Create RBroker instance using RBrokerFactory.
                 *
                 * This example creates a BackgroundTaskBroker.
                 */

                RAuthentication rAuth = new RBasicAuthentication(Program.AUTH_USER_NAME, Program.AUTH_PASSWORD);

                BackgroundBrokerConfig brokerConfig = new BackgroundBrokerConfig(Program.DEPLOYR_ENDPOINT, rAuth);
                RBroker rBroker = RBrokerFactory.backgroundTaskBroker(brokerConfig);

                /*
                 * 2. Register RTaskListener for asynchronous
                 * notifications on RTask completion.
                 */

                BackgroundTaskListener myTaskListener = new BackgroundTaskListener(rBroker);

                rBroker.addTaskListener(myTaskListener);


                /*
                 * 3. Define RTask
                 *
                 * This example creates a BackgroundTask that will
                 * execute an artibrary block of R code.
                 */

                String rCode = "x <- rnorm(100)";
                RTask rTask = RTaskFactory.backgroundTask("Example Background RTask",
                                                            "Example background RTask.",
                                                            rCode, 
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

                Console.WriteLine("Submitted " + rTask + " for execution on RBroker.");

                /*
                 * 4. Block until all tasks are complete, and shutdown has been called
                 */
                rBroker.waitUntilShutdown();

            } 
            catch(Exception tex) 
            {
                Console.WriteLine("Runtime exception=" + tex.ToString());
            }
        }

        private class BackgroundTaskListener : RTaskListener 
        {
            RBroker m_rBroker = null;

            public BackgroundTaskListener(RBroker rBroker) 
            {
                m_rBroker = rBroker;
            }

            public void onTaskCompleted(RTask rTask, RTaskResult rTaskResult) 
            {

                Console.WriteLine("onTaskCompleted: " + rTask + ", result: " + rTaskResult);

                /*
                 * Retrieve Job identifier from RTaskResult.
                 */
                String jobID = rTaskResult.getID();

                Console.WriteLine("onTaskCompleted: " + rTask + ", background Job ID: " + jobID);

                if(m_rBroker != null) 
                {

                    /*
                     * Important:
                     *
                     * To handle the results of a Background RTask you
                     * must transition from using the RBroker Framework
                     * API to using the .NET Client Library API.
                     */
                    RUser rUser = m_rBroker.owner();

                    if(rUser != null) 
                    {
                        try 
                        {

                            RJob rJob = rUser.queryJob(jobID);

                            Console.WriteLine("onTaskCompleted: " +rTask + ", rJob: " + rJob);

                            /*
                             * Next handle the result of the RJob as appropriate.
                             * In this example, simly cancel and delete the job.
                             */

                            try 
                            {
                                rJob.cancel();
                            } 
                            catch(Exception cex) 
                            {
                                Console.WriteLine("rJob.cancel ex=" + cex.ToString());
                            }
                            try 
                            {
                                rJob.delete();
                            } 
                            catch(Exception dex) 
                            {
                                Console.WriteLine("rJob.delete ex=" + dex.ToString());
                            }

                        } 
                        catch(Exception jex) 
                        {
                            Console.WriteLine("rUser.queryJob ex=" + jex.ToString());
                        }
                    }

                    m_rBroker.shutdown();
                    Console.WriteLine("BackgroundBasics: rBroker has been shutdown.");
                }
            }

            public void onTaskError(RTask rTask, String error) 
            {
                Console.WriteLine("BackgroundBasics: onTaskError: " + rTask + ", error: " + error);

                if(m_rBroker != null) 
                {
                    m_rBroker.shutdown();
                    Console.WriteLine("BackgroundBasics: rBroker has been shutdown.");
                }
            }
        }
    }
}
