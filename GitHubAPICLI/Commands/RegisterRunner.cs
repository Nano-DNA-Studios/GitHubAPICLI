using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager.Models;
using NanoDNA.GitHubManager;
using System;

namespace GitHubAPICLI.Commands
{
    internal class RegisterRunner : Command
    {
        public RegisterRunner(IDataManager dataManager) : base(dataManager) { }

        public override string Name => "registerrunner";

        public override string Description => throw new NotImplementedException();

        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            //if (args.Length == 0)
            //{
            //    //List out all the runners that are registered in the settings
            //    Console.WriteLine("No Arguments Provided, please provide the GitHub Owner and Repository Name");
            //    return;
            //}

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
