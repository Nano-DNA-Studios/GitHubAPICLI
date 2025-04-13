namespace GitHubAPICLI.Application
{
    public class ActionWorkerConfig
    {
        public string RepoOwner { get; private set; }

        public string RepoName { get; private set; }

        public string ContainerImage { get; private set; }

        public ActionWorkerConfig(string repoOwner, string repoName, string containerImage)
        {
            RepoOwner = repoOwner;
            RepoName = repoName;
            ContainerImage = containerImage;
        }
    }
}
