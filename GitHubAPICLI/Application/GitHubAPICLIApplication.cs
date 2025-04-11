using NanoDNA.CLIFramework;
using Newtonsoft.Json;
using System;

namespace GitHubAPICLI.Application
{
    public class GitHubAPICLIApplication : CLIApplication<GitHubCLISettings, GitHubCLIDataManager>
    {
        public GitHubAPICLIApplication() : base()
        {
           // Console.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
