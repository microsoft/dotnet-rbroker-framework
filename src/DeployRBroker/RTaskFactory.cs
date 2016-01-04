/*
 * RTaskFactory.cs
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

namespace DeployRBroker
{
    /// <summary>
    /// Factory class to create instance of the RTask implementations (Discrete, Background or Pooled).
    /// </summary>
    /// <remarks></remarks>
    public class RTaskFactory
    {
        /// <summary>
        /// Utility function for creating a DiscreteTask Instance of RTask
        /// </summary>
        /// <param name="filename">Name of repository R Script executed on the discrete task</param>
        /// <param name="directory">Directory in the repository where the R Script is located</param>
        /// <param name="author">Author of the repository R Script</param>
        /// <param name="version">Optional version of the R Script</param>
        /// <param name="options">Specification of options for the discrete task</param>
        /// <returns>DiscreteTask instance</returns>
        /// <remarks></remarks>
        public static RTask discreteTask(String filename,
                                         String directory,
                                         String author,
                                         String version,
                                         DiscreteTaskOptions options)
        {

            return new DiscreteTask(filename, directory, author, version, options);
        }

        /// <summary>
        /// Utility function for creating a DiscreteTask Instance of RTask
        /// </summary>
        /// <param name="externalURL">URL that represents an R Script executed on the discrete task</param>
        /// <param name="options">Specification of options for the discrete task</param>
        /// <returns>DiscreteTask instance</returns>
        /// <remarks></remarks>
        public static RTask discreteTask(String externalURL, DiscreteTaskOptions options)
        {

            return new DiscreteTask(externalURL, options);
        }

        /// <summary>
        /// Utility function for creating a PooledTask Instance of RTask
        /// </summary>
        /// <param name="filename">Name of repository R Script executed on the pooled task</param>
        /// <param name="directory">Directory in the repository where the R Script is located</param>
        /// <param name="author">Author of the repository R Script</param>
        /// <param name="version">Optional version of the R Script</param>
        /// <param name="options">Specification of options for the pooled task</param>
        /// <returns>PooledTask instance</returns>
        /// <remarks></remarks>
        public static RTask pooledTask(String filename,
                                         String directory,
                                         String author,
                                         String version,
                                         PooledTaskOptions options)
        {

            return new PooledTask(filename, directory, author, version, options);
        }

        /// <summary>
        /// Utility function for creating a PooledTask Instance of RTask
        /// </summary>
        /// <param name="codeBlock">R code to be executed on the pooled task</param>
        /// <param name="options">Specification of options for the pooled task</param>
        /// <returns>PooledTask instance</returns>
        /// <remarks></remarks>
        public static RTask pooledTask(String codeBlock, PooledTaskOptions options)
        {

            return new PooledTask(codeBlock, options);
        }

        /// <summary>
        /// Utility function for creating a PooledTask Instance of RTask
        /// </summary>
        /// <param name="externalURL">URL that represents an R Script executed on the pooled task</param>
        /// <param name="hasURL">Boolean specifying that a URL will be use to execute on the pooled task</param>
        /// <param name="options">Specification of options for the background task</param>
        /// <returns>PooledTask instance</returns>
        /// <remarks></remarks>
        public static RTask pooledTask(String externalURL, Boolean hasURL, PooledTaskOptions options)
        {

            return new PooledTask(externalURL, hasURL, options);
        }

        /// <summary>
        /// Utility function for creating a BackgroundTask Instance of RTask
        /// </summary>
        /// <param name="taskName">Name of the background task</param>
        /// <param name="taskDescription">Description of the background task</param>
        /// <param name="filename">Name of repository R Script executed on the background task</param>
        /// <param name="directory">Directory in the repository where the R Script is located</param>
        /// <param name="author">Author of the repository R Script</param>
        /// <param name="version">Optional version of the R Script</param>
        /// <param name="options">Specification of options for the background task</param>
        /// <returns>BackgroundTask instance</returns>
        /// <remarks></remarks>
        public static RTask backgroundTask(String taskName,
                                           String taskDescription,
                                           String filename,
                                           String directory,
                                           String author,
                                           String version,
                                           BackgroundTaskOptions options)
        {

            return new BackgroundTask(taskName, taskDescription, filename, directory, author, version, options);
        }

        /// <summary>
        /// Utility function for creating a BackgroundTask Instance of RTask
        /// </summary>
        /// <param name="taskName">Name of the background task</param>
        /// <param name="taskDescription">Description of the background task</param>
        /// <param name="codeBlock">R code to be executed on the background task</param>
        /// <param name="options">Specification of options for the background task</param>
        /// <returns>BackgroundTask instance</returns>
        /// <remarks></remarks>
        public static RTask backgroundTask(String taskName,
                                           String taskDescription,
                                           String codeBlock,
                                           BackgroundTaskOptions options)
        {

            return new BackgroundTask(taskName, taskDescription, codeBlock, options);
        }

        /// <summary>
        /// Utility function for creating a BackgroundTask Instance of RTask
        /// </summary>
        /// <param name="taskName">Name of the background task</param>
        /// <param name="taskDescription">Description of the background task</param>
        /// <param name="externalURL">URL that represents an R Script executed on the background task</param>
        /// <param name="hasURL">Boolean specifying that a URL will be use to execute on the background task</param>
        /// <param name="options">Specification of options for the background task</param>
        /// <returns>BackgroundTask instance</returns>
        /// <remarks></remarks>
        public static RTask backgroundTask(String taskName,
                                           String taskDescription,
                                           String externalURL,
                                           Boolean hasURL,
                                           BackgroundTaskOptions options)
        {

            return new BackgroundTask(taskName, taskDescription, externalURL, hasURL, options);
        }
    }
}
