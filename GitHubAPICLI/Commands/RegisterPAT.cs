using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using System;

namespace GitHubAPICLI.Commands
{
    internal class RegisterPAT : Command
    {
        public RegisterPAT(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "registerpat";

        public override string Description => "Registers the Applications GitHub PAT Token";

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
