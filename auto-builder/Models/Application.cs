using Newtonsoft.Json;

namespace AutoBuilder.Models
{
    public class Application
    {
        /// <summary>
        /// The configuration of this application
        /// </summary>
        [JsonProperty("configuration")]
        public ApplicationConfiguration Configuration { get; set; }

        /// <summary>
        /// The main status of the application like "active"
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The substatus of the application like "running"
        /// </summary>
        [JsonProperty("subStatus")]
        public string SubStatus { get; set; }

        /// <summary>
        /// The current cpu usage of this application
        /// </summary>
        [JsonProperty("cpuUsage")]
        public double CpuUsage { get; set; }

        /// <summary>
        /// The current cpu time of this application
        /// </summary>
        [JsonProperty("cpuTime")]
        public double CpuTime { get; set; }

        /// <summary>
        /// THe current memory usage of this application
        /// </summary>
        [JsonProperty("memoryUsage")]
        public double MemoryUsage { get; set; }

        public Application() { }

        public Application(ApplicationConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Application(ApplicationConfiguration configuration, string status, string subStatus, double cpuUsage, double cpuTime)
        {
            Configuration = configuration;
            Status = status;
            SubStatus = subStatus;
            CpuUsage = cpuUsage;
            CpuTime = cpuTime;
        }
    }
}
