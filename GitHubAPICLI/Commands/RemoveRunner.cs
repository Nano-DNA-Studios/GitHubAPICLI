using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager.Models;
using NanoDNA.GitHubManager;
using System;
using System.Linq;
using NanoDNA.DockerManager;

namespace GitHubAPICLI.Commands
{
    internal class RemoveRunner : Command
    {
        //Make this a command for Removing a runner from a repository 

        //Optional not fill in the repo, org and runner name, and remove all the ones that are stored / tracked in settings

        public RemoveRunner(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "removerunner";

        public override string Description => "Removes / Unregisters Runner through the GitHub API";

        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            if (args.Length == 0)
            {
                RemoveRegsiteredRunners();
                return;
            }

            if (args.Length != 3)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub Owner and Repository Name can be provided");
                return;
            }

            string repoOwner = args[0];
            string repoName = args[1];
            string runnerIDStr = args[2];

            Repository repo = Repository.GetRepository(repoOwner, repoName);

            Runner[] runners = repo.GetRunners();

            if (runners == null || runners.Length == 0)
            {
                Console.WriteLine("No Runners Found");
                return;
            }

            if (!long.TryParse(runnerIDStr, out long runnerID))
            {
                Console.WriteLine("Invalid value provided for Runner ID");
                return;
            }

            if (!runners.Any((runner) => runner.ID == runnerID))
            {
                Console.WriteLine($"Runner {runnerID} not found in the repository.");
                return;
            }

            try
            {
                repo.RemoveRunner(runnerID);
                Console.WriteLine($"Runner {runnerID} removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to remove runner: {ex.Message}");
                return;
            }
        }

        private void RemoveReposRunners (Repository repo)
        {
            Runner[] runners = repo.GetRunners();

            if (runners == null || runners.Length == 0)
            {
                Console.WriteLine("No Runners Found");
                return;
            }

            foreach (Runner runner in runners)
            {
                repo.RemoveRunner(runner.ID);






            }







        }

        /// <summary>
        /// Removes all the Saved Registered Runners
        /// </summary>
        private void RemoveRegsiteredRunners()
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (settings.RegisteredRunners == null || settings.RegisteredRunners.Count == 0)
            {
                Console.WriteLine("No Registered Runners Found");
                return;
            }

            RegisteredRunner currentRunner = settings.RegisteredRunners[0];

            Console.WriteLine("Removing all registered runners...");

            try
            {
                foreach (RegisteredRunner regRunner in settings.RegisteredRunners.ToArray())
                {
                    currentRunner = regRunner;

                    Repository repository = Repository.GetRepository(regRunner.RepoOwner, regRunner.RepoName);

                    repository.RemoveRunner(regRunner.RunnerID);

                    settings.RemoveRegisteredRunner(regRunner);

                    Docker.RemoveContainer(regRunner.RunnerName.ToLower(), true);

                    Console.WriteLine($"Removed Registered Runner {currentRunner.RunnerName}(ID : {currentRunner.RunnerID}) from {currentRunner.RepoOwner}/{currentRunner.RepoName}");
                }
            }
            catch (Exception ex)
            {
                settings.SaveSettings();
                Console.WriteLine($"Failed to Remove Registered Runner {currentRunner.RunnerName}(ID : {currentRunner.RunnerID}) from {currentRunner.RepoOwner}/{currentRunner.RepoName}: {ex.Message}");
                return;
            }

            settings.SaveSettings();
            Console.WriteLine("Removed all registered runners");
            return;
        }
    }
}
