using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using System;

namespace GitHubAPICLI.Commands
{
    /// <summary>
    /// Registers the GitHub Personal Access Token (PAT) for the application.
    /// </summary>
    internal class RegisterPAT : Command
    {
        /// <summary>
        /// Initializes a new Command Instance of <see cref="RegisterPAT"/>
        /// </summary>
        /// <param name="dataManager">DataManager containing context for the Command</param>
        public RegisterPAT(IDataManager dataManager) : base(dataManager) { }

        /// <inheritdoc/>
        public override string Name => "registerpat";

        /// <inheritdoc/>
        public override string Description => "Registers the Applications GitHub PAT Token";

        /// <inheritdoc/>
        public override void Execute(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub PAT can be provided");
                return;
            }

            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            settings.SetGitHubPAT(args[0]);
            settings.SaveSettings();

            Console.WriteLine("GitHub PAT Registered");
        }
    }
}
