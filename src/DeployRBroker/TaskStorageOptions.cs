/*
 * TaskStorageOptions.cs
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
    /// Task storage options. Supports the storage of working directory
    /// files and workspace R objects into the  DeployR repository
    /// following the execution of a Task.
    /// </summary>
    /// <remarks></remarks>
    public class TaskStorageOptions
    {
        private String m_files = "";
        private String m_objects = "";
        private String m_workspace = "";
        private String m_directory = "";
        private Boolean m_newVersion = false;
        private Boolean m_published = false;

        /// <summary>
        /// Comma-separated list of working directory files to be stored
        /// in the repository following an execution.
        /// </summary>
        /// <value>String specifying working directory files to save</value>
        /// <returns>String specifying working directory files to save</returns>
        /// <remarks></remarks>
        public String files
        {
            get
            {
                return m_files;
            }
            set
            {
                m_files = value;
            }
        }

        /// <summary>
        /// Comma-separated list of workspace objects to be stored in the
        /// repository following an execution.
        /// </summary>
        /// <value>String specifying workspace objects to save</value>
        /// <returns>String specifying workspace objects to save</returns>
        /// <remarks></remarks>
        public String objects
        {
            get
            {
                return m_objects;
            }
            set
            {
                m_objects = value;
            }
        }

        /// <summary>
        /// Specify a filename and the contents of the entire workspace will
        /// be saved as filename.rData in the repository.
        /// </summary>
        /// <value>String specifying filename used to save workspace</value>
        /// <returns>String specifying filename used to save workspace</returns>
        /// <remarks></remarks>
        public String workspace
        {
            get
            {
                return m_workspace;
            }
            set
            {
                m_workspace = value;
            }
        }
        
        /// <summary>
         /// Specify a directory into which each of the stored files, objects
         /// and/or workspace will be saved in the repository.
        /// </summary>
        /// <value>Name of the directory in the repository to save files</value>
        /// <returns>Name of the directory in the repository to save files</returns>
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
        /// Enable to create new version of files being stored in the 
        /// repository following an execution. Default behavior is to overwrite
        /// files that already exist in the repository.
        /// </summary>
        /// <value>Flag indicated if new verion of a file should be created</value>
        /// <returns>Flag indicated if new verion of a file should be created</returns>
        /// <remarks></remarks>
        public Boolean newVersion
        {
            get
            {
                return m_newVersion;
            }
            set
            {
                m_newVersion = value;
            }
        }

        /// <summary>
        /// Enable to assign public access on stored files in the 
        /// repository following an execution. By default private access is
        /// assigned to files being stored in the repository.
        /// </summary>
        /// <value>Flag indicating if public access to files in the repository should be granted</value>
        /// <returns>Flag indicating if public access to files in the repository are granted</returns>
        /// <remarks></remarks>
        public Boolean published
        {
            get
            {
                return m_published;
            }
            set
            {
                m_published = value;
            }
        }
    }
}
