using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Models;
using System;

namespace GitHubAPICLI.Commands
{
    /// <summary>
    /// Gets all the Registered Active Runners belonging to a Repository
    /// </summary>
    internal class GetRunners : Command
    {
        /// <summary>
        /// Initializes a new Command Instance of <see cref="GetRunners"/>
        /// </summary>
        /// <param name="dataManager">DataManager containing context for the Command</param>
        public GetRunners(IDataManager dataManager) : base(dataManager) { }

        /// <inheritdoc/>
        public override string Name => "getrunners";

        /// <inheritdoc/>
        public override string Description => "Gets the Runners that are registered and belong to a repository.";

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

            if (args.Length == 0)
            {
                DisplayAllRunners();
                return;
            }

            if (args.Length == 2)
            {
                DisplayRepoRunners(args[0], args[1]);
                return;
            }

            Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub Owner and Repository Name can be provided, or None");
        }

        /// <summary>
        /// Displays all the Runners for the Repositories that have Action Worker Configs Registered
        /// </summary>
        private void DisplayAllRunners()
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            foreach (ActionWorkerConfig workerConfig in settings.ActionWorkerConfigs)
            {
                DisplayRepoRunners(workerConfig.RepoOwner, workerConfig.RepoName);
                Console.WriteLine("\n");
            } 
        }

        /// <summary>
        /// Displays all the Runners Associated to a Single Repository
        /// </summary>
        /// <param name="repoOwner">Name of the Repository Owner</param>
        /// <param name="repoName">Name of the Repository</param>
        private void DisplayRepoRunners(string repoOwner, string repoName)
        {
            Repository repo = Repository.GetRepository(repoOwner, repoName);

            if (repo == null)
            {
                Console.WriteLine($"Repository {repoOwner}/{repoName} not found.");
                return;
            }

            Runner[] runners = repo.GetRunners();

            if (runners == null || runners.Length == 0)
            {
                Console.WriteLine($"No Runners Found for {repo.Owner.Login}/{repo.Name}");
                return;
            }

            Console.WriteLine($"========== {repo.Owner.Login}/{repo.Name} Runners ==========\n");

            foreach (Runner runner in runners)
            {
                DisplayRunner(runner);
                Console.WriteLine("\n");
            }
                

            Console.WriteLine("===================================================");
        }

        /// <summary>
        /// Displays all the Runners that are Registered to a Repository in a Formatted Manner
        /// </summary>
        /// <param name="runner"></param>
        private void DisplayRunner(Runner runner)
        {
            Console.WriteLine($"========== Runner {runner.ID} ==========");
            Console.WriteLine($"Name: {runner.Name}");
            Console.WriteLine($"OS: {runner.OS}");
            Console.WriteLine($"Status: {runner.Status}");
            Console.WriteLine($"Busy: {runner.Busy}");
            Console.WriteLine($"Labels :");

            for (int i = 0; i < runner.Labels.Length; i++)
            {
                RunnerLabel label = runner.Labels[i];

                Console.WriteLine($"  {label.Name} ({label.Type})");
            }

            Console.WriteLine("==============================");
        }
    }
}