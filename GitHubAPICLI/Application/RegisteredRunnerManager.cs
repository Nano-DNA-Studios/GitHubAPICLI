using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitHubAPICLI.Application
{
    /// <summary>
    /// Manager for the Registered GitHub Action Runners managed by the GitHub Self Runner Application
    /// </summary>
    public class RegisteredRunnerManager
    {
        /// <summary>
        /// Path the CLI Apps Cache Directory
        /// </summary>
        private string _CachePath;

        /// <summary>
        /// Path to the Registered Runners JSON File
        /// </summary>
        private string _RegisteredRunnersPath;

        /// <summary>
        /// List of Registered Runners for the GitHub Self Runner Application
        /// </summary>
        [JsonProperty("RegisteredRunners")]
        public List<RegisteredRunner> RegisteredRunners { get; private set; } = new List<RegisteredRunner>();

        /// <summary>
        /// Initializes a new Instance of the <see cref="RegisteredRunnerManager"/> Class.
        /// </summary>
        /// <param name="cachePath">Path to the CLI Applications Settings</param>
        public RegisteredRunnerManager(string cachePath)
        {
            _CachePath = cachePath;
            Console.WriteLine($"Cache Path In Class: {_CachePath}");
            _RegisteredRunnersPath = Path.Combine(_CachePath, "RegisteredRunners.json");
            RegisteredRunners = Load();
        }

        /// <summary>
        /// Loads the Registered Runners from the JSON File
        /// </summary>
        /// <returns>List of Registered Runners</returns>
        private List<RegisteredRunner> Load()
        {
            if (!File.Exists(_RegisteredRunnersPath))
                File.WriteAllText(_RegisteredRunnersPath, JsonConvert.SerializeObject(RegisteredRunners, Formatting.Indented));

            string json = File.ReadAllText(_RegisteredRunnersPath);
            return JsonConvert.DeserializeObject<List<RegisteredRunner>>(json);
        }

        /// <summary>
        /// Saves the Registered Runners to the JSON File
        /// </summary>
        public void Save ()
        {
            string json = JsonConvert.SerializeObject(RegisteredRunners, Formatting.Indented);
            File.WriteAllText(_RegisteredRunnersPath, json);
        }

        /// <summary>
        /// Adds a Registered Action Worker that has been Spawned by the CLI Application
        /// </summary>
        /// <param name="runner"></param>
        public void AddRegisteredRunner(RegisteredRunner runner)
        {
            RegisteredRunners.Add(runner);
        }

        /// <summary>
        /// Removes a Registered Action Worker that has been Spawned by the CLI Application once its been Unregistered
        /// </summary>
        /// <param name="runner">Registered Runner Instance Info to remove</param>
        public void RemoveRegisteredRunner(RegisteredRunner runner)
        {
            if (!RegisteredRunners.Any((regRunner) => regRunner.RunnerID == runner.RunnerID && regRunner.RunnerName == runner.RunnerName))
                return;

            RegisteredRunner remRunner = RegisteredRunners.First((regRunner) => regRunner.RunnerID == runner.RunnerID && regRunner.RunnerName == runner.RunnerName);

            RegisteredRunners.Remove(remRunner);
        }
    }
}