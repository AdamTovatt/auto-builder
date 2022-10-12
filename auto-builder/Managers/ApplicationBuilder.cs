using Newtonsoft.Json;
using System.Collections.Generic;

namespace auto_builder.Managers
{
    public class ApplicationBuilder
    {
        public static string ConfigurationFileName { get; set; } = "auto-builder-configuration.json";

        /// <summary>
        /// A list of applications that the application builder will support
        /// </summary>
        [JsonProperty("applications")]
        public List<string> Applications { get; set; } = new List<string>();

        public static ApplicationBuilder FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ApplicationBuilder>(json);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
