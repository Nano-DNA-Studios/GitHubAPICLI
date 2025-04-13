using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Models;
using System;

namespace GitHubAPICLI.Commands
{
    internal class FillWorkflows : Command
    {
        public FillWorkflows(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "fillworkflows";

        public override string Description => "Fills in all Pending Workflow Jobs by Spawning a GitHub Action Worker for them";

        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            if (args.Length == 0)
            {
                //Go through all the Registered Repos
            }

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub Owner and Repository Name can be provided");
                return;
            }

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            Repository repo = Repository.GetRepository(args[0], args[1]);

            FillRepoWorkflows(repo);
        }

        /// <summary>
        /// Fills in All Hanging Workflows for a Repository by Spawning a GitHub Action Worker for them
        /// </summary>
        /// <param name="repo">Repository to Fill In</param>
        private void FillRepoWorkflows (Repository repo)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            WorkflowRun[] workflows = repo.GetWorkflows();

            foreach (WorkflowRun workflow in workflows)
            {
                if (workflow.Status != "queued") //Add a Dictionary or some kind of Enum with a Converter to string for it
                    continue;

                RunnerBuilder builder = new RunnerBuilder($"{repo.Name}-{workflow.ID}", "mrdnalex/github-action-worker-container-dotnet", repo, false);

                builder.AddLabel($"run-{workflow.ID}");

                Runner runner = builder.Build();

                runner.Start();
                runner.SyncInfo();

                settings.AddRegisteredRunner(new RegisteredRunner(repo.Owner.Login, repo.Name, runner.ID, runner.Name));
            }
        }
    }
}
