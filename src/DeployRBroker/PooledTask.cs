/*
 * PooledTask.cs
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
    /// Represents a Pooled Task for execution on an RBroker
    /// </summary>
    /// <remarks></remarks>
    public class PooledTask : AbstractTask
    {
        private String m_code = "";
        private String m_filename = "";
        private String m_author = "";
        private String m_directory = "";
        private String m_version = "";
        private String m_external = "";
        private PooledTaskOptions m_options;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks></remarks>
        protected PooledTask()
        {
        }

        /// <summary>
        /// Constructor for specifying a a Pooled Task
        /// </summary>
        /// <param name="code">R code to be executed on the pooled task</param>
        /// <param name="options">Specification of options for the pooled task</param>
        /// <remarks></remarks>
        public PooledTask(String code, PooledTaskOptions options)
        {
            m_code = code;
            m_options = options;
        }

        /// <summary>
        /// Constructor for specifying a a Pooled Task
        /// </summary>
        /// <param name="externalURL">URL that represents an R Script executed on the pooled task</param>
        /// <param name="hasURL">Boolean specifying that a URL will be use to execute on the pooled task</param>
        /// <param name="options">Specification of options for the background task</param>
        /// <remarks></remarks>
        public PooledTask(String externalURL, Boolean hasURL, PooledTaskOptions options)
        {
            m_external = externalURL;
            m_options = options;
        }

        /// <summary>
        /// Constructor for specifying a a Pooled Task
        /// </summary>
        /// <param name="filename">Name of repository R Script executed on the pooled task</param>
        /// <param name="directory">Directory in the repository where the R Script is located</param>
        /// <param name="author">Author of the repository R Script</param>
        /// <param name="version">Optional version of the R Script</param>
        /// <param name="options">Specification of options for the pooled task</param>
        /// <remarks></remarks>
        public PooledTask(String filename,
                           String directory,
                           String author,
                           String version,
                           PooledTaskOptions options)
        {
            m_filename = filename;
            m_directory = directory;
            m_author = author;
            m_version = version;
            m_options = options;
        }

        /// <summary>
        /// Returns a string description of the Pooled Task
        /// </summary>
        /// <returns>Returns a string description of the Pooled Task</returns>
        /// <remarks></remarks>
        public String toString()
        {

            if (m_code != "")
            {
                return "PooledTask: [ " + m_code + " ]";
            }
            else
            {
                if (m_external != "")
                {
                    return "PooledTask: [ " + m_external + " ]";
                }
                else
                {
                    return "PooledTask: [ " + m_filename + " , " +
                                m_directory + " , " +
                                m_author + " , " +
                                m_version + " ]";
                }
            }
        }

        /// <summary>
        /// A block of executable R code
        /// </summary>
        /// <value></value>
        /// <returns>String containing the R code</returns>
        /// <remarks></remarks>
        public String code
        {
            get
            {
                return m_code;
            }
            set
            {
                m_code = value;
            }
        }

        /// <summary>
        /// The filename of a repository-managed executable R script.
        /// </summary>
        /// <value></value>
        /// <returns>String containing the filename</returns>
        /// <remarks></remarks>
        public String filename
        {
            get
            {
                return m_filename;
            }
            set
            {
                m_filename = value;
            }
        }

        /// <summary>
        /// The author of a repository-managed executable R script.
        /// </summary>
        /// <value></value>
        /// <returns>String containing the author</returns>
        /// <remarks></remarks>
        public String author
        {
            get
            {
                return m_author;
            }
            set
            {
                m_author = value;
            }
        }

        /// <summary>
        /// The directory of a repository-managed executable R script.
        /// </summary>
        /// <value></value>
        /// <returns>String containing the directory</returns>
        /// <remarks></remarks>
        public String directory
        {
            get
            {
                return m_directory;
            }
            set
            {
                m_directory = value;
            }
        }

        /// <summary>
        /// The version of a repository-managed executable R script.
        /// </summary>
        /// <value></value>
        /// <returns>String containing the version</returns>
        /// <remarks></remarks>
        public String version
        {
            get
            {
                return m_version;
            }
            set
            {
                m_version = value;
            }
        }

        /// <summary>
        /// Exteral property represents a URL/path to an external script or chain of scripts
        /// </summary>
        /// <value></value>
        /// <returns>String containing the external URL</returns>
        /// <remarks></remarks>
        public String external
        {
            get
            {
                return m_external;
            }
            set
            {
                m_external = value;
            }
        }

        /// <summary>
        /// The PooledTaskOptions for this task
        /// </summary>
        /// <value></value>
        /// <returns>PooledTaskOptions object</returns>
        /// <remarks></remarks>
        public PooledTaskOptions options
        {
            get
            {
                return m_options;
            }
            set
            {
                m_options = value;
            }
        }

    }
}
