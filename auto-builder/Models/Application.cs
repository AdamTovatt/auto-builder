using LibGit2Sharp;
using Newtonsoft.Json;
using Sakur.WebApiUtilities.Models;
using System;
using System.Linq;

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
        /// The name of the build script that should run when the application needs to be built
        /// </summary>
        [JsonProperty("buildScriptName")]
        public string BuildCommand { get; set; }

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

        /// <summary>
        /// The last build log for this application
        /// </summary>
        [JsonProperty("buildLog")]
        public string BuildLog { get; set; }

        /// <summary>
        /// The last commit for the repository of this application
        /// </summary>
        [JsonIgnore]
        public Commit LastCommit { get { return GetLastCommit(); } }

        /// <summary>
        /// Wether or not the application has built the last commit from the repository
        /// </summary>
        [JsonIgnore]
        public bool IsOnLastCommit { get { return CheckIfIsOnLastCommit(); } }

        private Commit GetLastCommit()
        {
            try
            {
                using (Repository repository = new Repository(SourceFolderPath))
                {
                    Commit commit = repository.Commits.FirstOrDefault();

                    if (commit == null)
                        throw new ApiException("No commits found for \"" + Name + "\"", System.Net.HttpStatusCode.InternalServerError);

                    return commit;
                }
            }
            catch (RepositoryNotFoundException)
            {
                throw new ApiException("Error in config file for \"" + Name + "\". No repository found at: " + SourceFolderPath, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private bool CheckIfIsOnLastCommit()
        {
            return LastCommit.Sha == LastBuiltCommit;
        }
    }
}
