using Newtonsoft.Json;
using System;

namespace auto_builder.Models
{
    public class Application
    {
        /// <summary>
        /// A human readable name for the application
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The name of the build script that should run when the application needs to be built
        /// </summary>
        [JsonProperty("buildScriptName")]
        public string BuildScriptName { get; set; }

        /// <summary>
        /// The path to folder with the source code, used for git
        /// </summary>
        [JsonProperty("sourceFolderPath")]
        public string SourceFolderPath { get; set; }

        /// <summary>
        /// The commit hash of the last commit that was built
        /// </summary>
        [JsonProperty("lastBuiltCommit")]
        public string LastBuiltCommit { get; set; }

        /// <summary>
        /// The last time the application was built
        /// </summary>
        [JsonProperty("lastBuildTime")]
        public DateTime LastBuildTime { get; set; }
    }
}
