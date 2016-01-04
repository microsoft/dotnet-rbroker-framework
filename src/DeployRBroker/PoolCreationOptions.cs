/*
 * PoolCreationOptions.cs
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
using DeployR;

namespace DeployRBroker
{
    /// <summary>
    /// Project creation options. Can be used to pre-initialize data in the workspace and working directory for the new project.
    /// </summary>
    /// <remarks></remarks>
    public class PoolCreationOptions
    {
        private PoolPreloadOptions m_preloadDirectory;
        private PoolPreloadOptions m_preloadWorkspace;
        private List<RData> m_rinputs = new List<RData>();
        private Boolean m_releaseGridResources = false;
        private String m_preloadByDirectory = "";

        /// <summary>
        /// Preload working directory options allow the loading
        /// of one or more files from the repository into the
        /// working directory of the current R session
        /// </summary>
        /// <value>preload working directory options</value>
        /// <returns>preload working directory options</returns>
        /// <remarks></remarks>
        public PoolPreloadOptions preloadDirectory
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
        /// workspace of the current R session
        /// </summary>
        /// <value>preload workspace options</value>
        /// <returns>preload workspace options</returns>
        /// <remarks></remarks>
        public PoolPreloadOptions preloadWorkspace
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
        /// List of DeployR-encoded R objects to be added to the
        /// workspace of the current R session
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
        /// When the pool is shutdown, all grid resources for the user will be released.
        /// </summary>
        /// <value>releaseGridResources value</value>
        /// <returns>releaseGridResources value</returns>
        /// <remarks></remarks>
        public Boolean releaseGridResources
        {
            get
            {
                return m_releaseGridResources;
            }
            set
            {
                m_releaseGridResources = value;
            }
        }
    }
}
