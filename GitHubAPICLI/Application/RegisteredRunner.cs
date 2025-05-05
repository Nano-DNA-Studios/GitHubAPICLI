
namespace GitHubAPICLI.Application
{
    /// <summary>
    /// Defines the Relevant Information for Action Worker Runner Regsitered with a Repository
    /// </summary>
    public class RegisteredRunner
    {
        /// <summary>
        /// Name of the Repository Owner
        /// </summary>
        public string RepoOwner { get; private set; }

        /// <summary>
        /// Name of the Repository
        /// </summary>
        public string RepoName { get; private set; }

        /// <summary>
        /// ID of the Runner
        /// </summary>
        public long RunnerID { get; private set; }

        /// <summary>
        /// Name of the Runner
        /// </summary>
        public string RunnerName { get; private set; }

        /// <summary>
        /// Initializes a new Instance of <see cref="RegisteredRunner"/>
        /// </summary>
        /// <param name="repoOwner">Name of the Repository Owner</param>
        /// <param name="repoName">Name of the Repository</param>
        /// <param name="runnerID">ID of the Runner</param>
        /// <param name="runnerName">Name of the Runner</param>
        public RegisteredRunner(string repoOwner, string repoName, long runnerID, string runnerName)
        {
            RepoOwner = repoOwner;
            RepoName = repoName;
            RunnerID = runnerID;
            RunnerName = runnerName;
        }

        /// <summary>
        /// Checks if the Current Action Worker Runner is the Same as Another
        /// </summary>
        /// <param name="runner">Action Worker Runner to Compare</param>
        /// <returns>True if they are the Same, False otherwise</returns>
        public bool SameAs(RegisteredRunner runner)
        {
            return RepoOwner == runner.RepoOwner &&
                   RepoName == runner.RepoName &&
                   RunnerID == runner.RunnerID &&
                   RunnerName == runner.RunnerName;
        }
    }
}
