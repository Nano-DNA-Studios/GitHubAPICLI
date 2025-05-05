using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Models;
using System;
using System.Linq;

namespace GitHubAPICLI.Commands
{
    /// <summary>
    /// Gets all the Queued Workflows belonging to a GitHub Repository or, all registered Repositories.
    /// </summary>
    internal class GetWorkflows : Command
    {
        /// <summary>
        /// Initializes a new Command Instance of <see cref="GetWorkflows"/>
        /// </summary>
        /// <param name="dataManager">DataManager containing context for the Command</param>
        public GetWorkflows(IDataManager dataManager) : base(dataManager) { }

        /// <inheritdoc/>
        public override string Name => "getworkflows";

        /// <inheritdoc/>
        public override string Description => "Gets all the Queued Workflows belonging to a GitHub Repsository or, all registered Repositories";

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
                DisplayRegisteredRepoWorkflows();
                return;
            }

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub Owner and Repository Name can be provided, or None");
                return;
            }

            Repository repo = Repository.GetRepository(args[0], args[1]);

            if (repo == null)
            {
                Console.WriteLine($"Repository {args[0]}/{args[1]} not found.");
                return;
            }

            DisplayRepositoryWorkflows(repo);
        }

        /// <summary>
        /// Displays all the Queued Workflows for the Repositories that have Action Worker Configs Registered
        /// </summary>
        private void DisplayRegisteredRepoWorkflows()
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

                DisplayRepositoryWorkflows(repo);
            }
        }

        /// <summary>
        /// Displays the Queued Workflows for a Single Repository
        /// </summary>
        /// <param name="repo">Repository to Display the Worflows</param>
        private void DisplayRepositoryWorkflows(Repository repo)
        {
            WorkflowRun[] workflowRuns = repo.GetWorkflows();

            if (workflowRuns == null || workflowRuns.Length == 0)
            {
                Console.WriteLine("No Workflows Found for this Repository");
                return;
            }

            WorkflowRun[] queuedRuns = workflowRuns.Where(w => w.Status == "queued").ToArray();

            if (queuedRuns == null || queuedRuns.Length == 0)
            {
                Console.WriteLine("No Queued Workflows Found for this Repository");
                return;
            }

            foreach (WorkflowRun workflow in queuedRuns)
                DisplayWorkflowInfo(workflow);
        }

        /// <summary>
        /// Displays the Workflow Information to the Console in a Formatted Manner
        /// </summary>
        /// <param name="workflow">Workflow Run Info to Display</param>
        private void DisplayWorkflowInfo(WorkflowRun workflow)
        {
            Console.WriteLine($"========== Workflow {workflow.ID} ==========");
            Console.WriteLine($"Name: {workflow.Name}");
            Console.WriteLine($"Workflow ID: {workflow.WorkflowID}");
            Console.WriteLine($"Event: {workflow.Event}");
            Console.WriteLine($"Display Title: {workflow.DisplayTitle}");
            Console.WriteLine($"Status: {workflow.Status}");
            Console.WriteLine($"Conclusion: {workflow.Conclusion}");
            Console.WriteLine($"Created At: {workflow.CreatedAt}");
            Console.WriteLine($"HTML URL: {workflow.HtmlURL}");
            Console.WriteLine("=============================================");
        }
    }
}
