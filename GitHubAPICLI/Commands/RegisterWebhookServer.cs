using GitHubAPICLI.Application;
using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using System;
using System.IO;

namespace GitHubAPICLI.Commands
{
    internal class RegisterWebhookServer : Command
    {
        public RegisterWebhookServer(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "registerwebhookserver";

        public override string Description => "Registers the Info needed to Launch a new Webhook Server for GitHub Actions to be received";

        public override void Execute(string[] args)
        {
            GitHubCLISettings settings = (GitHubCLISettings)DataManager.Settings;

            if (string.IsNullOrEmpty(settings.GitHubPAT))
            {
                Console.WriteLine("GitHub PAT is not set. Please register it using the 'registerpat' command.");
                return;
            }

            if (args.Length != 4)
            {
                Console.WriteLine("Invalid Number of Arguments Provided, the Webhook Secret, Default Docker Image, Port Number and the Logs Output for the Server must be specified");
                return;
            }

            //Check if the Default Docker Image is a valid Docker Image

            string webhookSecret = args[0];
            string defaultDockerImage = args[1];
            string logsOutput = args[3];

            if (!int.TryParse(args[2], out int port))
            {
                Console.WriteLine("Invalid Port Number Provided");
                return;
            }

            if (!Directory.Exists(logsOutput))
            {
                Console.WriteLine("Invalid Directory Provided for the Logs Output, Directory does not exist");
                return;
            }

            settings.SetWebhookSecret(webhookSecret);
            settings.SetDefaultDockerImage(defaultDockerImage);
            settings.SetWebhookServerPort(port);
            settings.SetLogsOutput(logsOutput);
            settings.SaveSettings();

            Console.WriteLine("Webhook Server Settings have been Set!");
        }
    }
}
