namespace GitHubAPICLI.Application
{
    /// <summary>
    /// Defines an Action Workers Configuration for Spawning and Filling in Workflow Jobs
    /// </summary>
    public class ActionWorkerConfig
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
        /// The Docker Container Image to Use for the Action Worker
        /// </summary>
        public string ContainerImage { get; private set; }

        /// <summary>
        /// Initializes a new Instance of the <see cref="ActionWorkerConfig"/> class
        /// </summary>
        /// <param name="repoOwner">Name of the Repository Owner</param>
        /// <param name="repoName">Name of the Repository</param>
        /// <param name="containerImage">Docker Container Image for the Action Worker</param>
        public ActionWorkerConfig(string repoOwner, string repoName, string containerImage)
        {
            RepoOwner = repoOwner;
            RepoName = repoName;
            ContainerImage = containerImage;
        }

        /// <summary>
        /// Checks if the Current Action Worker Configuration is the Same as Another
        /// </summary>
        /// <param name="config">Action Worker Config to Compare</param>
        /// <returns>True if they are the Same, False otherwise</returns>
        public bool SameAs(ActionWorkerConfig config)
        {
            return RepoOwner == config.RepoOwner &&
                   RepoName == config.RepoName &&
                   ContainerImage == config.ContainerImage;
        }
    }
}
