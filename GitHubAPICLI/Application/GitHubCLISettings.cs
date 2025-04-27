using NanoDNA.CLIFramework.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GitHubAPICLI.Application
{
    /// <summary>
    /// Defines the Settings for the GitHub API CLI Application
    /// </summary>
    public class GitHubCLISettings : Setting
    {
        /// <inheritdoc/>
        public override string ApplicationName => "GitHubAPICLI";

        /// <inheritdoc/>
        public override string GlobalFlagPrefix => DEFAULT_GLOBAL_FLAG_PREFIX;

        /// <inheritdoc/>
        public override string GlobalShorthandFlagPrefix => DEFAULT_GLOBAL_SHORTHAND_FLAG_PREFIX;

        /// <summary>
        /// The GitHub Personal Access Token (PAT) used for authentication with the GitHub API.
        /// </summary>
        [JsonProperty("GitHubPAT")]
        public string GitHubPAT { get; private set; }

        /// <summary>
        /// The Webhook Secret used for the Webhook Server to Authenticate with GitHub
        /// </summary>
        [JsonProperty("WebhookSecret")]
        public string WebhookSecret { get; private set; }

        /// <summary>
        /// The Default Docker Image used for the Action Workers of new Unregistered Repositories received through the Webhook Server
        /// </summary>
        [JsonProperty("DefaultDockerImage")]
        public string DefaultDockerImage { get; private set; }

        /// <summary>
        /// The Webhook Server Port used for the Webhook Server to receive Webhooks from GitHub
        /// </summary>
        [JsonProperty("WebhookServerPort")]
        public int WebhookServerPort { get; private set; }

        /// <summary>
        /// List of Active Runners Registered by the Application
        /// </summary>
        [JsonProperty("RegisteredRunners")]
        public List<RegisteredRunner> RegisteredRunners { get; private set; } = new List<RegisteredRunner>();

        /// <summary>
        /// List of Action Worker to Fill in A Repos Workflows
        /// </summary>
        [JsonProperty("ActionWorkerConfigs")]
        public List<ActionWorkerConfig> ActionWorkerConfigs { get; private set; } = new List<ActionWorkerConfig>();

        /// <summary>
        /// The Output Directory for the Logs of the Webhook Runners
        /// </summary>
        [JsonProperty("LogsOutput")]
        public string LogsOutput { get; private set; }

        /// <summary>
        /// Sets the GitHub Personal Access Token (PAT) for the application.
        /// </summary>
        /// <param name="pat">GitHub PAT</param>
        public void SetGitHubPAT(string pat)
        {
            GitHubPAT = pat;
        }

        /// <summary>
        /// Sets the Webhook Secret for the Webhook Server to Authenticate with GitHub
        /// </summary>
        /// <param name="secret">Webhook Secret</param>
        public void SetWebhookSecret(string secret)
        {
            WebhookSecret = secret;
        }

        /// <summary>
        /// Sets the Default Docker Image for the Action Workers of new Unregistered Repositories received through the Webhook Server
        /// </summary>
        /// <param name="image">Docker Image Name</param>
        public void SetDefaultDockerImage(string image)
        {
            DefaultDockerImage = image;
        }

        /// <summary>
        /// Sets the Webhook Server Port for the Webhook Server to receive Webhooks from GitHub
        /// </summary>
        /// <param name="port">Webhook Server to Start on</param>
        public void SetWebhookServerPort(int port)
        {
            WebhookServerPort = port;
        }

        /// <summary>
        /// Sets the Output Directory for the Logs of the Webhook Runners
        /// </summary>
        /// <param name="output">Output Directory</param>
        public void SetLogsOutput(string output)
        {
            LogsOutput = output;
        }

        /// <summary>
        /// Adds a Configuration for Filling in a Repository's Workflows
        /// </summary>
        /// <param name="worker">Worker Config to Register to the CLI App</param>
        public void AddActionWorkerConfig(ActionWorkerConfig worker)
        {
            if (!ActionWorkerConfigs.Any((repoWorker) => repoWorker.SameRepoAs(worker)))
            {
                ActionWorkerConfigs.Add(worker);
                return;
            }

            ReplaceActionWorkerConfig(worker);
        }

        /// <summary>
        /// Replaces a Action Worker Config info with the Latest info
        /// </summary>
        /// <param name="worker">Action Worker Config Info</param>
        public void ReplaceActionWorkerConfig(ActionWorkerConfig worker)
        {
            if (!ActionWorkerConfigs.Any((repoWorker) => repoWorker.SameRepoAs(worker)))
                return;

            ActionWorkerConfigs.Remove(ActionWorkerConfigs.FirstOrDefault((repoWorker) => repoWorker.SameRepoAs(worker)));
            ActionWorkerConfigs.Add(worker);
        }


        /// <summary>
        /// Adds a Registered Action Worker that has been Spawned by the CLI Application
        /// </summary>
        /// <param name="runner"></param>
        public void AddRegisteredRunner(RegisteredRunner runner)
        {
            RegisteredRunners.Add(runner);
        }

        /// <summary>
        /// Removes a Registered Action Worker that has been Spawned by the CLI Application once its been Unregistered
        /// </summary>
        /// <param name="runner">Registered Runner Instance Info to remove</param>
        public void RemoveRegisteredRunner(RegisteredRunner runner)
        {
            if (!RegisteredRunners.Any((regRunner) => regRunner.RunnerID == runner.RunnerID && regRunner.RunnerName == runner.RunnerName))
                return;

            RegisteredRunner remRunner = RegisteredRunners.First((regRunner) => regRunner.RunnerID == runner.RunnerID && regRunner.RunnerName == runner.RunnerName);

            RegisteredRunners.Remove(remRunner);
        }
    }
}
