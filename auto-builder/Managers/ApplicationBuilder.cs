using AutoBuilder.Helpers;
using AutoBuilder.Models;
using Newtonsoft.Json;
using Sakur.WebApiUtilities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBuilder.Managers
{
    public class ApplicationBuilder
    {
        public static string ConfigurationFilePath { get; set; } = "auto-builder-configuration.json";

        /// <summary>
        /// A list of applications that the application builder will support
        /// </summary>
        [JsonProperty("applications")]
        public List<Application> Applications { get; set; } = new List<Application>();

        public void Build(Application application)
        {
            try
            {
                Task.Run(async () => { await BuildTask(application); });
            }
            catch (Exception exception)
            {
                throw new ApiException("Error when starting build: " + exception.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private async Task BuildTask(Application application)
        {
            application.LastBuildTime = DateTime.Now;
            application.CreateNewLog();

            await WriteApplicationLogAsync(application, string.Format("Build started! ({0})\n", DateTime.Now.ToString()));

            foreach (Command command in application.GetCommands())
            {
                await WriteApplicationLogAsync(application, command.ToString()); //write the command
                await WriteApplicationLogAsync(application, await command.RunAsync()); //write the result of the command
            }

            await WriteApplicationLogAsync(application, string.Format("\nBuild done! ({0})\nTook {1}s\n", DateTime.Now.ToString(), (int)(DateTime.Now - application.LastBuildTime).TotalSeconds));
        }

        public async Task WriteApplicationLogAsync(Application application, string message)
        {
            application.AppendToLog(message);
            await SaveAsync();
        }

        public Application GetApplication(string name)
        {
            return Applications.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public static ApplicationBuilder FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ApplicationBuilder>(json);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static async Task<ApplicationBuilder> LoadAsync()
        {
            ConfigurationFilePath = EnvironmentHelper.GetEnvironmentVariable("CONFIG_PATH");

            if (File.Exists(ConfigurationFilePath))
                return FromJson(await File.ReadAllTextAsync(ConfigurationFilePath)); //return existing if it exists

            ApplicationBuilder result = new ApplicationBuilder(); //create new one if none exists
            await result.SaveAsync();

            return result;
        }

        public async Task SaveAsync()
        {
            await File.WriteAllTextAsync(ConfigurationFilePath, ToJson());
        }
    }
}
