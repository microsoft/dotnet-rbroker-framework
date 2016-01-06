/*
 * TaskPreloadOptions.cs
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
    /// Task preload options. Supports the loading of DeployR-managed repository files into the R session before a Task is executed.
    /// </summary>
    /// <remarks></remarks>
    public class TaskPreloadOptions
    {
        private String m_filename = "";
        private String m_directory = "";
        private String m_author = "";
        private String m_version = "";

        /// <summary>
        /// Comma-separated list of repository filenames.
        /// </summary>
        /// <value>String specifying filename(s) to load</value>
        /// <returns>String of filename(s)</returns>
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
        /// Comma-separated list of directories, one directory per filename
        /// </summary>
        /// <value>String specifying directories(s) associated with filename(s)</value>
        /// <returns>String of directories(s)</returns>
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
        /// Comma-separated list of authors, one directory per filename
        /// </summary>
        /// <value>String specifying authors(s) associated with filename(s)</value>
        /// <returns>String of authors(s)</returns>
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
        /// Comma-separated list of versions, one directory per filename
        /// </summary>
        /// <value>String specifying versions(s) associated with filename(s)</value>
        /// <returns>String of version(s)</returns>
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
    }
}
