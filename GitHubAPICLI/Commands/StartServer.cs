using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Events;
using NanoDNA.GitHubManager.Models;
using NanoDNA.GitHubManager.Services;
using System;
using System.IO;
using System.Linq;

namespace GitHubAPICLI.Commands
{
    /// <summary>
    /// Starts a Thread Blocking Webhooks Server for GitHub API Action Workflows
    /// </summary>
    internal class StartServer : Command
    {
        /// <summary>
        /// Initializes a new Command Instanc of <see cref="StartServer"/>
        /// </summary>
        /// <param name="dataManager"></param>
        public StartServer(IDataManager dataManager) : base(dataManager) { }

        /// <summary>
        /// Thread Lock Object for Thread Safety when Saving Settings
        /// </summary>
        public object threadLock = new object();

        /// <inheritdoc/>
        public override string Name => "startserver";

        /// <inheritdoc/>
        public override string Description => "Starts a Webhook Server for receiving Webhooks from GitHub API related to Action Workflows";

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

            if (args.Length != 0)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, No arguments should be provided. Webhook Server should be registered through \"registerwebhookserver\" Command");
                return;
            }

            if (settings.WebhookSecret == null || settings.WebhookSecret == string.Empty)
            {
                Console.WriteLine("Webhook Secret is not set. Please register it using the 'registerwebhookserver' command.");
                return;
            }

            if (settings.DefaultDockerImage == null || settings.DefaultDockerImage == string.Empty)
            {
                Console.WriteLine("Default Docker Image is not set. Please register it using the 'registerwebhookserver' command.");
                return;
            }

            if (settings.WebhookServerPort == 0)
            {
                Console.WriteLine("Webhook Server Port is not set. Please register it using the 'registerwebhookserver' command.");
                return;
            }

            if (!Directory.Exists(settings.LogsOutput))
            {
                Console.WriteLine("Invalid Directory Provided for the Logs Output, Directory does not exist, Please register a Valid Directory using the 'registerwebhookserver' command.");
                return;
            }

            GitHubWebhookService webhookService = new GitHubWebhookService(settings.WebhookSecret);

            webhookService.On<WorkflowRunEvent>(workflowRun =>
            {
                settings = Setting.LoadSettings<GitHubCLISettings>();

                if (workflowRun == null)
                {
                    Console.WriteLine("Received a null WorkflowRunEvent");
                    return;
                }

                WorkflowRun run = workflowRun.WorkflowRun;

                if (run.Status == "completed")
                    SaveLogs(run.Repository, run);

                if (run.Status != "queued")
                    return;

                AddRunner(run);
            });

            webhookService.Start(settings.WebhookServerPort, true);
        }

        /// <summary>
        /// Adds and Registers a Runner to the Repository requesting a new Workflow Run
        /// </summary>
        /// <param name="workflowRun">Workflow Run Instance</param>
        private void AddRunner(WorkflowRun workflowRun)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            Repository repo = Repository.GetRepository(workflowRun.Repository.Owner.Login, workflowRun.Repository.Name);

            RunnerBuilder builder = new RunnerBuilder($"{workflowRun.Repository.Name}-{workflowRun.ID}", GetDockerImage(repo), repo, true);

            builder.AddLabel($"run-{workflowRun.ID}");
            builder.AddLabel("API-CLI-Webhook-Runner");

            Runner runner = builder.Build();

            runner.Start();

            settings.AddRegisteredRunner(new RegisteredRunner(repo.Owner.Login, repo.Name, runner.ID, runner.Name));

            lock (threadLock)
                settings.SaveSettings();

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Added Runner {runner.Name} ({runner.ID}) to {repo.FullName}");

            runner.StopRunner += (runnner) =>
            {
                settings.RemoveRegisteredRunner(new RegisteredRunner(repo.Owner.Login, repo.Name, runner.ID, runner.Name));

                lock (threadLock)
                    settings.SaveSettings();

                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Removed Runner {runner.Name} ({runner.ID}) to {repo.FullName}");
            };
        }

        /// <summary>
        /// Gets the Docker Image to use for the Runner. If a configuration is not found, it returns the Default Docker Image
        /// </summary>
        /// <param name="repo">Repository that is receiving a runner</param>
        /// <returns>The Docker Image to use</returns>
        private string GetDockerImage(Repository repo)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            ActionWorkerConfig config = settings.ActionWorkerConfigs.FirstOrDefault((config) => config.RepoName == repo.Name);

            string defaultDockerImage = settings.DefaultDockerImage;

            if (config != null)
                return config.ContainerImage;

            settings.AddActionWorkerConfig(new ActionWorkerConfig(repo.Owner.Login, repo.Name, defaultDockerImage));

            lock (threadLock)
                settings.SaveSettings();

            return defaultDockerImage;
        }

        /// <summary>
        /// Saves the Workflow Logs of the Workflow Run to the Logs Output Directory
        /// </summary>
        /// <param name="repo">Repository the Workflow Belongs to</param>
        /// <param name="workflowRun">Workflow Run that is having it's Logs saved</param>
        private void SaveLogs(Repository repo, WorkflowRun workflowRun)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            WorkflowRun workRun = repo.GetWorkflows().FirstOrDefault((run) => run.ID == workflowRun.ID);

            string repoDirectory = Path.Join(settings.LogsOutput, repo.Name);

            if (workRun == null)
                return;

            if (!Directory.Exists(repoDirectory))
                Directory.CreateDirectory(repoDirectory);

            File.WriteAllBytes(Path.Join(repoDirectory, $"{repo.Name}-{workRun.ID}-Logs.zip"), workRun.GetLogs());
        }
    }
}