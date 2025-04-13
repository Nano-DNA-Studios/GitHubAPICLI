using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager.Models;
using NanoDNA.GitHubManager;
using System;

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

            if (args.Length < 4)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, the Repo Owner, Repo Name, Runner Name and Runner Image must be provided.");
                return;
            }

            string repoOwner = args[0];
            string repoName = args[1];
            string runnerName = args[2];
            string runnerImage = args[3];

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            Repository repo = Repository.GetRepository(repoOwner, repoName);

            RunnerBuilder runnerBuilder = new RunnerBuilder(runnerName, runnerImage, repo, false);

            Runner runner = runnerBuilder.Build();

            runner.Start();
            runner.SyncInfo();

            settings.AddActionWorkerConfig(new ActionWorkerConfig(repoOwner, repoName, runnerImage));
            settings.AddRegisteredRunner(new RegisteredRunner(repoOwner, repoName, runner.ID, runner.Name));
            settings.SaveSettings();

            Console.WriteLine($"Runner {runner.Name} added to repository {repo.Name}.");
        }
    }
}
