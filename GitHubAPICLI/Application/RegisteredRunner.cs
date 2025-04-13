
namespace GitHubAPICLI.Application
{
    public class RegisteredRunner
    {
        public string RepoOwner { get; private set; }

        public string RepoName { get; private set; }

        public long RunnerID { get; private set; }

        public string RunnerName { get; private set; }

        public RegisteredRunner(string repoOwner, string repoName, long runnerID, string runnerName)
        {
            RepoOwner = repoOwner;
            RepoName = repoName;
            RunnerID = runnerID;
            RunnerName = runnerName;
        }
    }
}
