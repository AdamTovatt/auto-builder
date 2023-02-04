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
        private List<ApplicationConfiguration> Applications { get; set; }

        [JsonIgnore]
        public int ApplicationCount { get { return Applications.Count; } }

        public List<Application> GetApplications(TopCommand topCommand, SystemctlListCommand listCommand)
        {
            List<Application> result = new List<Application>();

            foreach(ApplicationConfiguration configuration in Applications)
            {
                Application application = new Application(configuration);

                TopCommand.ApplicationRow topCommandEntry = topCommand.ApplicatonRows.Where(x => x.Path.Contains(configuration.Name)).FirstOrDefault();
                SystemctlListCommand.ApplicationRow listCommandEntry = listCommand.GetApplication(configuration.Name);

                if(topCommandEntry != null)
                {
                    application.CpuTime = topCommandEntry.CpuTime;
                    application.CpuUsage = topCommandEntry.CpuUsage;
                    application.MemoryUsage = topCommandEntry.MemoryUsage;
                }

                if(listCommandEntry != null)
                {
                    application.Status = listCommandEntry.Status;
                    application.SubStatus = listCommandEntry.SubStatus;
                }

                result.Add(application);
            }

            return result;
        }

        public void Build(ApplicationConfiguration application)
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

        private async Task BuildTask(ApplicationConfiguration application)
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

        public async Task WriteApplicationLogAsync(ApplicationConfiguration application, string message)
        {
            application.AppendToLog(message);
            await SaveAsync();
        }

        public ApplicationConfiguration GetApplication(string name)
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
