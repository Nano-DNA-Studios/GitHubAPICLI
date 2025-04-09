using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Models;
using Newtonsoft.Json;
using System;

namespace GitHubAPICLI.Commands
{
    internal class GetRepo : Command
    {
        public GetRepo(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "getrepo";

        public override string Description => "Gets the JSON of a Repository";

        public override void Execute(string[] args)
        {
            Console.WriteLine("Getting Repo");

            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            Console.WriteLine(JsonConvert.SerializeObject(settings, Formatting.Indented));

            Console.WriteLine("GitHub PAT: " + settings.GitHubPAT);

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            Repository repo = Repository.GetRepository("Nano-DNA-Studios", "NanoDNA.GitHubManager");

            Console.WriteLine(JsonConvert.SerializeObject(repo, Formatting.Indented));
        }
    }
}
