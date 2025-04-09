using NanoDNA.CLIFramework.Data;

namespace GitHubAPICLI
{
    internal class GitHubCLISettings : Setting
    {
        public override string ApplicationName => "GitHubAPICLI";

        public override string GlobalFlagPrefix => DEFAULT_GLOBAL_FLAG_PREFIX;

        public override string GlobalShorthandFlagPrefix => DEFAULT_GLOBAL_SHORTHAND_FLAG_PREFIX;
    }
}
