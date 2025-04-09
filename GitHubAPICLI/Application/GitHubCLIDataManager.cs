using NanoDNA.CLIFramework.Data;
using NanoDNA.CLIFramework.Flags;
using System;
using System.Collections.Generic;

namespace GitHubAPICLI.Application
{
    public class GitHubCLIDataManager : DataManager
    {
        public GitHubCLIDataManager(Setting settings, Dictionary<Type, Flag> globalFlags) : base(settings, globalFlags)
        {
        }
    }
}
