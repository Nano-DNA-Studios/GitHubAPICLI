using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Models;
using System;

namespace GitHubAPICLI.Commands
{
    /// <summary>
    /// Fills in all the Pending Workflows by Spawning a GitHub Action Worker for them
    /// </summary>
    internal class FillWorkflows : Command
    {
        /// <summary>
        /// Initializes a new Command Instance of <see cref="FillWorkflows"/>
        /// </summary>
        /// <param name="dataManager">DataManager containing context for the Command</param>
        public FillWorkflows(IDataManager dataManager) : base(dataManager) { }

        /// <inheritdoc/>
        public override string Name => "fillworkflows";

        /// <inheritdoc/>
        public override string Description => "Fills in all Pending Workflow Jobs by Spawning a GitHub Action Worker for them";

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
                Console.WriteLine("Filling in all Workflows for all Workflows in Repositories that have Action Worker Configs");
                FillAllWorkflows();
                return;
            }

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub Owner and Repository Name can be provided");
                return;
            }

            Repository repo = Repository.GetRepository(args[0], args[1]);

            if (repo == null)
            {
                Console.WriteLine($"Repository {args[0]}/{args[1]} not found.");
                return;
            }

            FillRepoWorkflows(repo);
        }

        /// <summary>
        /// Fills in All the Hanging Workflows for All the Repositories that have Registered Action Worker Configs
        /// </summary>
        private void FillAllWorkflows()
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            foreach (ActionWorkerConfig workerConfig in settings.ActionWorkerConfigs)
            {
                Repository repo = Repository.GetRepository(workerConfig.RepoOwner, workerConfig.RepoName);

                if (repo == null)
                {
                    Console.WriteLine($"Repository {workerConfig.RepoOwner}/{workerConfig.RepoName} not found.");
                    continue;
                }

                FillRepoWorkflows(repo);
            }
        }

        /// <summary>
        /// Fills in All Hanging Workflows for a Repository by Spawning a GitHub Action Worker for them
        /// </summary>
        /// <param name="repo">Repository to Fill In</param>
        private void FillRepoWorkflows(Repository repo)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            WorkflowRun[] workflows = repo.GetWorkflows();

            Console.WriteLine($"Filling in Workflows for {repo.FullName}");

            foreach (WorkflowRun workflow in workflows)
            {
                if (workflow.Status != "queued")
                    continue;

                RunnerBuilder builder = new RunnerBuilder($"{repo.Name}-{workflow.ID}", "mrdnalex/github-action-worker-container-dotnet", repo, false);

                builder.AddLabel($"run-{workflow.ID}");

                Runner runner = builder.Build();

                runner.Start();
                runner.SyncInfo();

                Console.WriteLine($"Runner {runner.Name} started for Workflow {workflow.ID} in {repo.FullName}");

                settings.AddRegisteredRunner(new RegisteredRunner(repo.Owner.Login, repo.Name, runner.ID, runner.Name));
            }

            settings.SaveSettings();
        }
    }
}
