using Newtonsoft.Json;
using Sakur.WebApiUtilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoBuilder.Models
{
    public class Application
    {
        /// <summary>
        /// A human readable name for the application
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The path to folder with the source code, used for git
        /// </summary>
        [JsonProperty("sourceFolderPath")]
        public string SourceFolderPath { get; set; }

        /// <summary>
        /// The list of commands to run when building
        /// </summary>
        [JsonProperty("buildCommands")]
        public List<string> BuildCommands { get; set; }

        /// <summary>
        /// The last time the application was built
        /// </summary>
        [JsonProperty("lastBuildTime")]
        public DateTime LastBuildTime { get; set; }

        /// <summary>
        /// The last build log for this application
        /// </summary>
        [JsonProperty("buildLog")]
        public string BuildLog { get; set; }

        private StringBuilder currentLog;

        public void CreateNewLog()
        {
            currentLog = new StringBuilder();
        }

        public void AppendToLog(string logMessage)
        {
            if (currentLog == null)
                CreateNewLog();

            currentLog.AppendLine(logMessage);

            BuildLog = currentLog.ToString();
        }

        public List<Command> GetCommands()
        {
            List<Command> commands = new List<Command>();

            foreach (string command in BuildCommands)
            {
                StringBuilder argument = new StringBuilder();
                string[] parts = command.Split();

                if (parts.Length > 1)
                {
                    for (int i = 1; i < parts.Length; i++)
                    {
                        argument.Append(string.Format("{0} ", parts[i]));
                    }
                }

                commands.Add(new Command()
                {
                    WorkingDirectory = SourceFolderPath,
                    FileName = parts[0],
                    Arguments = argument.ToString().Trim()
                });
            }

            return commands;
        }
    }
}
