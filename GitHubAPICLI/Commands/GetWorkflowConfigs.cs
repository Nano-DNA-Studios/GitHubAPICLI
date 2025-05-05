using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using System;

namespace GitHubAPICLI.Commands
{
    /// <summary>
    /// Gets and Displays the Registered Workflow Configs for Filling in Repo Workflows
    /// </summary>
    internal class GetWorkflowConfigs : Command
    {
        /// <summary>
        /// Initializes a new Command Instance of <see cref="GetWorkflowConfigs"/>
        /// </summary>
        /// <param name="dataManager">DataManager containing context for the Command</param>
        public GetWorkflowConfigs(IDataManager dataManager) : base(dataManager) { }

        /// <inheritdoc/>
        public override string Name => "getworkflowconfigs";

        /// <inheritdoc/>
        public override string Description => "Displays all the Registered Workflow Configs for Filling in Repo Workflows";

        /// <inheritdoc/>
        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (settings.ActionWorkerConfigs == null || settings.ActionWorkerConfigs.Count == 0)
            {
                Console.WriteLine("No Action Worker Configs Found");
                return;
            }

            Console.WriteLine($"========== Action Worker Configs ==========");

            foreach (ActionWorkerConfig config in settings.ActionWorkerConfigs)
            {
                Console.WriteLine($"Owner: {config.RepoOwner}");
                Console.WriteLine($"Repo: {config.RepoName}");
                Console.WriteLine($"Container Image: {config.ContainerImage}");
                Console.WriteLine($"==========================================");
            }
        }
    }
}
