using GitHubAPICLI.Application;

namespace GitHubAPICLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GitHubAPICLIApplication app = new GitHubAPICLIApplication();
            app.Run(args);
        }
    }
}
