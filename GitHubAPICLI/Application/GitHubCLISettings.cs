using NanoDNA.CLIFramework.Data;
using Newtonsoft.Json;

namespace GitHubAPICLI.Application
{
    public class GitHubCLISettings : Setting
    {
        public override string ApplicationName => "GitHubAPICLI";

        public override string GlobalFlagPrefix => DEFAULT_GLOBAL_FLAG_PREFIX;

        public override string GlobalShorthandFlagPrefix => DEFAULT_GLOBAL_SHORTHAND_FLAG_PREFIX;

        [JsonProperty("GitHubPAT")]
        public string GitHubPAT { get; private set; }

        /// <summary>
        /// Sets the GitHub Personal Access Token (PAT) for the application.
        /// </summary>
        /// <param name="pat">GitHub PAT</param>
        public void SetGitHubPAT(string pat)
        {
            GitHubPAT = pat;
        }
    }
}
