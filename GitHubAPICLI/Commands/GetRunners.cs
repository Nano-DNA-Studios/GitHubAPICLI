using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubAPICLI.Commands
{
    internal class GetRunners : Command
    {
        public GetRunners(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "getrunners";

        public override string Description => "Gets the Runners that are registered and belong to a repository.";

        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub Owner and Repository Name can be provided");
                return;
            }

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            Repository repo = Repository.GetRepository(args[0], args[1]);

            Runner[] runners = repo.GetRunners();

            if (runners.Length == 0)
            {
                Console.WriteLine("No Runners Found");
                return;
            }

            foreach (Runner runner in runners)
            {
                Console.WriteLine($"========== Runner {runner.ID} ==========");
                Console.WriteLine($"Name: {runner.Name}");
                Console.WriteLine($"OS: {runner.OS}");
                Console.WriteLine($"Status: {runner.Status}");
                Console.WriteLine($"Busy: {runner.Busy}");
                Console.WriteLine($"Labels :");

                for (int i = 0; i < runner.Labels.Length; i++)
                {
                    RunnerLabel label = runner.Labels[i];

                    Console.WriteLine($"  {label.Name} ({label.Type})");
                }

                Console.WriteLine("==============================");
            }
        }
    }
}
