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
                commands.Add(new Command(command, SourceFolderPath));
            }

            return commands;
        }
    }
}
