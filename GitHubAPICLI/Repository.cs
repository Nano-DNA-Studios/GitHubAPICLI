using NanoDNA.CLIFramework.Commands;
using NanoDNA.CLIFramework.Data;
using System;

namespace GitHubAPICLI
{
    internal class Repository : Command
    {
        public Repository(IDataManager dataManager) : base(dataManager)
        {
        }

        public override string Name => "repository";

        public override string Description => "Gets the JSON of a Repository";

        public override void Execute(string[] args)
        {
            Console.WriteLine("Getting Repo");






        }
    }
}
