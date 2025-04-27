using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager.Models;
using NanoDNA.GitHubManager;
using System;

namespace GitHubAPICLI.Commands
{
    /// <summary>
    /// Adds a Runner to a Specified Repository with a Custom Name and Docker Image
    /// </summary>
    internal class AddRunner : Command
    {
        /// <summary>
        /// Initializes a new Command Instance of <see cref="AddRunner"/>
        /// </summary>
        /// <param name="dataManager">DataManager containing context for the Command</param>
        public AddRunner(IDataManager dataManager) : base(dataManager) { }

        /// <inheritdoc/>
        public override string Name => "addrunner";

        /// <inheritdoc/>
        public override string Description => "Adds a GitHub Action Runner to a Repository";

        /// <inheritdoc/>
        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            if (args.Length < 4)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, the Repo Owner, Repo Name, Runner Name and Runner Image must be provided.");
                return;
            }

            string repoOwner = args[0];
            string repoName = args[1];
            string runnerName = args[2];
            string runnerImage = args[3];

            Repository repo = Repository.GetRepository(repoOwner, repoName);

            if (repo == null)
            {
                Console.WriteLine($"Repository {repoOwner}/{repoName} not found.");
                return;
            }

            RunnerBuilder runnerBuilder = new RunnerBuilder(runnerName, runnerImage, repo, false);

            runnerBuilder.AddLabel("API-CLI-Runner");

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