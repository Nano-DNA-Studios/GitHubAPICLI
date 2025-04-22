using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Models;
using System;
using System.Linq;

namespace GitHubAPICLI.Commands
{
    internal class GetWorkflows : Command
    {
        public GetWorkflows(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "getworkflows";

        public override string Description => "Gets all the Queued Workflows belonging to a GitHub Repsository or, all registered Repositories";

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

        private void DisplayRegisteredRepoWorkflows ()
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

        private void DisplayRepositoryWorkflows (Repository repo)
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
            {
                DisplayWorkflowInfo(workflow);
            } 
        }

        private void DisplayWorkflowInfo (WorkflowRun workflow)
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
            Console.WriteLine($"Jobs URL : {workflow.JobsURL}");


            WorkflowJob[] jobs = workflow.GetJobs();

            if (jobs == null || jobs.Length == 0)
            {
                Console.WriteLine("No Jobs Found for this Workflow");
                Console.WriteLine("============================================");
                return;
            }

            foreach (WorkflowJob job in jobs)
            {
                Console.WriteLine($"    ========== Job {job.ID} ==========");
                Console.WriteLine($"    Job Name: {job.Name}");
                Console.WriteLine($"    Node ID: {job.NodeID}");
                Console.WriteLine($"    Status: {job.Status}");
                Console.WriteLine($"    Steps: {job.Steps.Length}");
                Console.WriteLine($"    Conclusion: {job.Conclusion}");
                Console.WriteLine($"    Created At: {job.CreatedAt}");
                Console.WriteLine($"    Started At: {job.StartedAt}");
                Console.WriteLine($"    Completed At: {job.CompletedAt}");
                Console.WriteLine($"    Run Attempt: {job.RunAttempt}");
                Console.WriteLine($"    Runner Name: {job.RunnerName}");
                Console.WriteLine($"    HTML URL: {job.HtmlURL}");
                Console.WriteLine("     =======================================");
            }
            Console.WriteLine("=============================================");
        }
    }
}
