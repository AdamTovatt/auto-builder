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
            application.AppendToLog("Build started!\n");
            Save();

            foreach (Command command in application.GetCommands())
            {
                application.AppendToLog(command.ToString());
                Save();

                application.AppendToLog(await RunCommandAsync(command));
                Save();
            }

            application.AppendToLog("\nBuild done!");
            Save();
        }

        private async Task<string> RunCommandAsync(Command command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = command.WorkingDirectory;
            startInfo.FileName = command.FileName;
            startInfo.Arguments = command.Arguments;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            Process process = Process.Start(startInfo);
            await process.WaitForExitAsync();

            return await process.StandardOutput.ReadToEndAsync();
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

        public static ApplicationBuilder Load()
        {
            ConfigurationFilePath = EnvironmentHelper.GetEnvironmentVariable("CONFIG_PATH");

            if (File.Exists(ConfigurationFilePath))
                return FromJson(File.ReadAllText(ConfigurationFilePath));

            ApplicationBuilder result = new ApplicationBuilder();
            result.Save();

            return result;
        }

        public void Save()
        {
            File.WriteAllText(ConfigurationFilePath, ToJson());
        }
    }
}
