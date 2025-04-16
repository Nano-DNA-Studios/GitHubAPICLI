using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using NanoDNA.GitHubManager;
using NanoDNA.GitHubManager.Models;
using System;

namespace GitHubAPICLI.Commands
{
    internal class GetRepository : Command
    {
        public GetRepository(IDataManager dataManager) : base(dataManager) { }

        public override string Name => "getrepository";

        public override string Description => "Gets the JSON of a Repository";

        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            GitHubAPIClient.SetGitHubPAT(settings.GitHubPAT);

            if (args.Length == 0)
            {
                DisplayAllRepos();
                return;
            }

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, only the GitHub Owner and Repository Name can be provided, or None");
                return;
            }

            Repository repo = Repository.GetRepository(args[0], args[1]);

            if (repo == null)
            {
                Console.WriteLine("Repository Not Found");
                return;
            }

            DisplayRepoInfo(repo);
        }

        private void DisplayAllRepos ()
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            foreach (ActionWorkerConfig workerConfig in settings.ActionWorkerConfigs)
            {
                Repository repo = Repository.GetRepository(workerConfig.RepoOwner, workerConfig.RepoName);

                if (repo == null)
                {
                    Console.WriteLine($"Repository {workerConfig.RepoOwner}/{workerConfig.RepoName} not found.");
                    continue;
                }

                DisplayRepoInfo(repo);
            }
        }

        /// <summary>
        /// Displays the Repositories Information to the Console in a Formatted Manner
        /// </summary>
        /// <param name="repo">Repositories Info</param>
        private void DisplayRepoInfo(Repository repo)
        {
            Console.WriteLine($"========== Repository Info ==========");
            Console.WriteLine($"ID: {repo.ID}");
            Console.WriteLine($"Name: {repo.Name}");
            Console.WriteLine($"Owner: {repo.Owner.Login}");
            Console.WriteLine($"Private: {repo.Private}");
            Console.WriteLine($"Language: {repo.Language}");
            Console.WriteLine($"HTMLURL: {repo.HtmlURL}");
            Console.WriteLine($"=====================================");
        }
    }
}
