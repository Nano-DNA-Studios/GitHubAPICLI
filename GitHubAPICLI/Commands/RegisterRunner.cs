using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager.Models;
using NanoDNA.GitHubManager;
using System;

namespace GitHubAPICLI.Commands
{
    /// <summary>
    /// Registers a GitHub Action Worker Runner configuration for a specific Repository.
    /// </summary>
    internal class RegisterRunner : Command
    {
        /// <summary>
        /// Initializes a new Command Instance of <see cref="RegisteredRunner"/>
        /// </summary>
        /// <param name="dataManager">DataManager containing context for the Command</param>
        public RegisterRunner(IDataManager dataManager) : base(dataManager) { }

        /// <inheritdoc/>
        public override string Name => "registerrunner";

        /// <inheritdoc/>
        public override string Description => throw new NotImplementedException();

        /// <inheritdoc/>
        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            if (args.Length != 3)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub Owner and Repository Name can be provided");
                return;
            }

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            Repository repo = Repository.GetRepository(args[0], args[1]);

            settings.AddActionWorkerConfig(new ActionWorkerConfig(repo.Owner.Login, repo.Name, args[2]));
        }
    }
}
