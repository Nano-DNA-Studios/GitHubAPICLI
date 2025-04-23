using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Events;
using NanoDNA.GitHubManager.Models;
using NanoDNA.GitHubManager.Services;
using System;
using System.Collections.Generic;

namespace GitHubAPICLI.Commands
{
    internal class StartWebhookServer : Command
    {
        public StartWebhookServer(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "startwebhookserver";

        public override string Description => "Starts a Webhook Server for receiving Webhooks from GitHub API related to Action Workflows";

        public Dictionary<string, Runner> Runners { get; set; }

        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            if (args.Length != 1)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the Webhook Server Port can be provided");
                return;
            }

            string webhookSecret = args[0];

            GitHubWebhookService webhookService = new GitHubWebhookService(webhookSecret);

            webhookService.On<WorkflowRunEvent>(workflowRun =>
            {
                settings = Setting.LoadSettings<GitHubCLISettings>();

                if (workflowRun == null)
                {
                    Console.WriteLine("Received a null WorkflowRunEvent");
                    return;
                }

                WorkflowRun run = workflowRun.WorkflowRun;

                if (run.Status != "queued")
                {
                    Console.WriteLine($"Received a WorkflowRunEvent with status: {run.Status}");
                    return;
                }

                Repository repo = Repository.GetRepository(run.Repository.Owner.Login, run.Repository.Name);

                RunnerBuilder builder = new RunnerBuilder($"{run.Repository.Name}-{run.ID}", "mrdnalex/github-action-worker-container-dotnet", repo, true);

                builder.AddLabel($"run-{workflowRun.WorkflowRun.ID}");

                Runner runner = builder.Build();

                runner.Start();

                settings.AddRegisteredRunner(new RegisteredRunner(repo.Owner.Login, repo.Name, runner.ID, runner.Name));
                settings.SaveSettings();

                runner.StopRunner += (run) =>
                {
                    Console.WriteLine(run.Container.GetLogs());

                    WorkflowRun[] runs = repo.GetWorkflows();

                    foreach (WorkflowRun workRun in runs)
                    {
                        if (workRun.ID == workflowRun.WorkflowRun.ID)
                        {
                            Console.WriteLine($"Workflow Run: {workRun.ID} Status: {workRun.Status}");

                            workRun.GetLogs();

                            WorkflowJob[] jobs = workRun.GetJobs();

                        }
                    }
                };
            });

            webhookService.StartAsync();

            while (true) { }
        }
    }
}
