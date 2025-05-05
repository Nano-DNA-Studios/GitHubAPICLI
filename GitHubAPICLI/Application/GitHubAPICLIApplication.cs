using NanoDNA.CLIFramework;

namespace GitHubAPICLI.Application
{
    /// <summary>
    /// Defines the GitHub API CLI Application
    /// </summary>
    public class GitHubAPICLIApplication : CLIApplication<GitHubCLISettings, GitHubCLIDataManager>
    {
        /// <summary>
        /// Initializes a new Instance of the <see cref="GitHubAPICLIApplication"/>
        /// </summary>
        public GitHubAPICLIApplication() : base()
        {
        }
    }
}
