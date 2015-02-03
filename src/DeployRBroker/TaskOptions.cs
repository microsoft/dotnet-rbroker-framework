/*
 * TaskOptions.cs
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
using DeployR;

namespace DeployRBroker
{
    /// <summary>
    /// Task options allow the customization of pre-execution.
    /// on-execution and post-execution behaviors for a given Task.
    /// </summary>
    /// <remarks></remarks>
    public class TaskOptions
    {
        private TaskPreloadOptions m_preloadDirectory;
        private TaskPreloadOptions m_preloadWorkspace;
        private String m_preloadByDirectory = "";
        private TaskStorageOptions m_storageOptions;
        private Boolean m_encodeDataFramePrimitiveAsVector = false;
        private String m_graphicsDevice = "";
        private int m_graphicsHeight = 0;
        private int m_graphicsWidth = 0;
        private List<RData> m_rinputs = new List<RData>();
        private List<String> m_routputs = new List<String>();
        private Boolean m_echooff = false;
        private Boolean m_consoleoff = false;
        private String m_nan = "";
        private String m_infinity = "";
        private String m_csvrinputs = "";

        /// <summary>
        /// Preload working directory options allow the loading
        /// of one or more files from the repository into the
        /// working directory of the current R session prior to execution.
        ///
        /// Pre-execution option.
        /// </summary>
        /// <value>preload working directory options</value>
        /// <returns>preload working directory options</returns>
        /// <remarks></remarks>
        public TaskPreloadOptions preloadDirectory
        {
            get
            {
                return m_preloadDirectory;
            }
            set
            {
                m_preloadDirectory = value;
            }
        }

        /// <summary>
        /// Preload workspace options allow the loading of one
        /// or more binary R objects from the repository into the
        /// workspace of the current R session prior to execution.
        ///
        /// Pre-execution option.
        /// </summary>
        /// <value>preload workspace options</value>
        /// <returns>preload workspace options</returns>
        /// <remarks></remarks>
        public TaskPreloadOptions preloadWorkspace
        {
            get
            {
                return m_preloadWorkspace;
            }
            set
            {
                m_preloadWorkspace = value;
            }
        }

        /// <summary>
        /// Project adoption options allow the pre-loading
        /// of a pre-existing project workspace, project working directory,
        /// project history and/or project package dependencies
        /// into the current R session
        /// </summary>
        /// <value>preloadByDirectory option</value>
        /// <returns>preloadByDirectory option</returns>
        /// <remarks></remarks>
        public String preloadByDirectory
        {
            get
            {
                return m_preloadByDirectory;
            }
            set
            {
                m_preloadByDirectory = value;
            }
        }
        
        /// <summary>
        /// Repository storage options allow the storage of
        /// one-or-more workspace objects, the entire workspace
        /// and/or one-or-more working directory files from the
        /// current R session into the repository following the execution.
        ///
        /// Storage options are only available to AUTHENTICATED
        /// users on the call, ANONYMOUS users can not store data
        /// to the repository.
        ///
        /// Post-execution option.
        /// </summary>
        /// <value>storage adoption options</value>
        /// <returns>storage adoption options</returns>
        /// <remarks></remarks>
        public TaskStorageOptions storageOptions
        {
            get
            {
                return m_storageOptions;
            }
            set
            {
                m_storageOptions = value;
            }
        }

        /// <summary>
        /// Workspace data.frame object encoding preference when
        /// retrieving R objects from the current R session following an execution.
        /// This option works in conjunction with the robjects property on this class.
        /// The default DeployR-encoding is to encode primatives inside data.frame
        /// objects as primitives, not as vectors.
        ///
        /// Post-execution option.
        /// </summary>
        /// <value>data.frame object encoding preference</value>
        /// <returns>data.frame object encoding preference</returns>
        /// <remarks></remarks>
        public Boolean encodeDataFramePrimitiveAsVector
        {
            get
            {
                return m_encodeDataFramePrimitiveAsVector;
            }
            set
            {
                m_encodeDataFramePrimitiveAsVector = value;
            }
        }

        /// <summary>
        /// Graphics device to use on execution: "png" or "svg".
        ///
        /// On execution option.
        /// </summary>
        /// <value>graphics device name</value>
        /// <returns>graphics device name</returns>
        /// <remarks></remarks>
        public String graphicsDevice
        {
            get
            {
                return m_graphicsDevice;
            }
            set
            {
                m_graphicsDevice = value;
            }
        }

        /// <summary>
        /// Graphics height on execution
        ///
        /// On execution option.
        /// </summary>
        /// <value>graphics height</value>
        /// <returns>graphics height</returns>
        /// <remarks></remarks>
        public int graphicsHeight
        {
            get
            {
                return m_graphicsHeight;
            }
            set
            {
                m_graphicsHeight = value;
            }
        }

        /// <summary>
        /// Graphics width on execution
        ///
        /// On execution option.
        /// </summary>
        /// <value>graphics width</value>
        /// <returns>graphics width</returns>
        /// <remarks></remarks>
        public int graphicsWidth
        {
            get
            {
                return m_graphicsWidth;
            }
            set
            {
                m_graphicsWidth = value;
            }
        }

        /// <summary>
        /// List of DeployR-encoded R objects to be added to the
        /// workspace of the current R session prior to the execution.
        ///
        /// Pre-execution option.
        /// </summary>
        /// <value>List of RData encoded inputs for an R Script</value>
        /// <returns>List of RData encoded inputs for an R Script</returns>
        /// <remarks></remarks>
        public List<RData> rinputs
        {
            get
            {
                return m_rinputs;
            }
            set
            {
                m_rinputs = value;
            }
        }

        /// <summary>
        /// List of workspace objects to be retrieved from the
        /// workspace of the current R session following the execution
        /// and returned as DeployR-encoded R objects.
        ///
        /// Post-execution option.
        /// </summary>
        /// <value>list of R object names to retreive</value>
        /// <returns>list of R object names to retreive</returns>
        /// <remarks></remarks>
        public List<String> routputs
        {
            get
            {
                return m_routputs;
            }
            set
            {
                m_routputs = value;
            }
        }

        /// <summary>
        /// When enabled all R console output is suppressed and will
        /// not appear in response markup. This control has no
        /// impact on console output on the event stream.
        ///
        /// On execution option.
        /// </summary>
        /// <value>echooff value</value>
        /// <returns>echooff value</returns>
        /// <remarks></remarks>
        public Boolean echooff
        {
            get
            {
                return m_echooff;
            }
            set
            {
                m_echooff = value;
            }
        }

        /// <summary>
        /// When enabled R console output is suppressed and will
        /// not appear in response markup or on the event stream.
        ///
        /// On execution option.
        /// </summary>
        /// <value>consoleoff value</value>
        /// <returns>consoleoff value</returns>
        /// <remarks></remarks>
        public Boolean consoleoff
        {
            get
            {
                return m_consoleoff;
            }
            set
            {
                m_consoleoff = value;
            }
        }

        /// <summary>
        /// Optional custom value to denote NAN values in
        /// RevoDeployR-encoded objects in the response markup.
        /// Default is null.
        ///
        /// Post-execution option.
        /// </summary>
        /// <value>nan value</value>
        /// <returns>nan value</returns>
        /// <remarks></remarks>
        public String nan
        {
            get
            {
                return m_nan;
            }
            set
            {
                m_nan = value;
            }
        }


        /// <summary>
        /// Optional custom value to denote INFINITY values in
        /// RevoDeployR-encoded objects in the response markup.
        /// Default is 0x7ff0000000000000L.
        ///
        /// Post-execution option.
        /// </summary>
        /// <value>infinity value</value>
        /// <returns>infinity value</returns>
        /// <remarks></remarks>
        public String infinity
        {
            get
            {
                return m_infinity;
            }
            set
            {
                m_infinity = value;
            }
        }

        /// <summary>
        ///Comma-seperated list of primitive R object names and values,
        /// to be added to the workspace of the current R session prior
        /// to the execution.
        ///
        /// eg. csvrinputs=name,George,age,45
        ///
        /// Pre-execution option.
        /// </summary>
        /// <value>csvrinputs</value>
        /// <returns>csvrinputs</returns>
        /// <remarks></remarks>
        public String csvrinputs
        {
            get
            {
                return m_csvrinputs;
            }
            set
            {
                m_csvrinputs = value;
            }
        }
    }
}
