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
        public static string ConfigurationFileName { get; set; } = "auto-builder-configuration.json";

        /// <summary>
        /// A list of applications that the application builder will support
        /// </summary>
        [JsonProperty("applications")]
        public List<Application> Applications { get; set; } = new List<Application>();

        public void Build(Application application)
        {
            try
            {
                Process process = RunCommand(application.BuildCommand);
                Task.Run(() => { UpdateApplicationInfoWhileBuilding(process, application); });
            }
            catch(Exception exception)
            {
                throw new ApiException("Error when starting build: " + exception.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private Process RunCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "/bin/bash";
            startInfo.Arguments = command;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            return Process.Start(startInfo);
        }

        private void UpdateApplicationInfoWhileBuilding(Process process, Application application)
        {
            StringBuilder buildLog = new StringBuilder();
            buildLog.AppendLine("build started");

            Save();

            DataReceivedEventHandler buildOutputHandler = (sender, eventData) => { 
                string data = eventData.Data;

                if (!string.IsNullOrEmpty(data))
                {
                    buildLog.Append(data);
                    application.BuildLog = buildLog.ToString();
                    Save();
                }
            };

            process.OutputDataReceived += buildOutputHandler;

            process.WaitForExit();
            
            application.LastBuiltCommit = application.LastCommit.Sha;
            application.BuildLog = process.StandardOutput.ReadToEnd();
            Save();
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
            return JsonConvert.SerializeObject(this);
        }

        public static ApplicationBuilder Load()
        {
            if (File.Exists(ConfigurationFileName))
                return FromJson(File.ReadAllText(ConfigurationFileName));

            ApplicationBuilder result = new ApplicationBuilder();
            result.Save();

            return result;
        }

        public void Save()
        {
            File.WriteAllText(ConfigurationFileName, ToJson());
        }
    }
}
