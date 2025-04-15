using NanoDNA.CLIFramework.Data;
using Newtonsoft.Json;
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

        /// <inheritdoc/>
        [JsonProperty("GitHubPAT")]
        public string GitHubPAT { get; private set; }

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
        /// Sets the GitHub Personal Access Token (PAT) for the application.
        /// </summary>
        /// <param name="pat">GitHub PAT</param>
        public void SetGitHubPAT(string pat)
        {
            GitHubPAT = pat;
        }

        /// <summary>
        /// Adds a Configuration for Filling in a Repository's Workflows
        /// </summary>
        /// <param name="worker">Worker Config to Register to the CLI App</param>
        public void AddActionWorkerConfig(ActionWorkerConfig worker)
        {
            if (ActionWorkerConfigs.Any((repoWorker) => repoWorker.RepoOwner == worker.RepoOwner && repoWorker.RepoName == worker.RepoName && repoWorker.ContainerImage == worker.ContainerImage))
                return;

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
