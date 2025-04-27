using NanoDNA.CLIFramework.Data;
using NanoDNA.CLIFramework.Flags;
using System;
using System.Collections.Generic;

namespace GitHubAPICLI.Application
{
    /// <summary>
    /// Defines the Information for the GitHub CLI Data Manager
    /// </summary>
    public class GitHubCLIDataManager : DataManager
    {
        /// <summary>
        /// Initializes a new Instance of the <see cref="GitHubCLIDataManager"/>
        /// </summary>
        /// <param name="settings">Data Managers Settings</param>
        /// <param name="globalFlags">Global Flags Set by the User</param>
        public GitHubCLIDataManager(Setting settings, Dictionary<Type, Flag> globalFlags) : base(settings, globalFlags) { }
    }
}
