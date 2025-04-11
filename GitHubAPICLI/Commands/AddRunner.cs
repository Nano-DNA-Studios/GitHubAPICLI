using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager.Models;
using NanoDNA.GitHubManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubAPICLI.Commands
{
    internal class AddRunner : Command
    {
        public AddRunner(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "addrunner";

        public override string Description => "Adds a GitHub Action Runner to a Repository";

        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub Owner and Repository Name can be provided");
                return;
            }

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            Repository repo = Repository.GetRepository(args[0], args[1]);

            RunnerBuilder runnerBuilder = new RunnerBuilder(args[1], "mrdnalex/github-action-worker-container-dotnet", repo, false);

            runnerBuilder.AddLabel("CLI-Tool");

            Runner runner = runnerBuilder.Build();

            runner.Start();

            //Maybe add the runner and it's repo to the settings for currently active runners?

            //Save the runners info so that it can be unregistered later?
        }
    }
}
