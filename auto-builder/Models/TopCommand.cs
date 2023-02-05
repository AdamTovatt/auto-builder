using AutoBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoBuilder.Models
{
    public class TopCommand
    {
        public class ApplicationRow
        {
            public double CpuUsage { get; set; }
            public double MemoryUsage { get; set; }
            public double CpuTime { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Path;
            }
        }

        public string Error { get; set; }
        public string Raw { get; set; }
        public double LoadAverage1Minute { get; set; }
        public double LoadAverage5Minute { get; set; }
        public double LoadAverage15Minute { get; set; }
        public DateTime Time { get; set; }
        public string Uptime { get; set; }
        public double TotalCpuUsage { get; set; }

        [JsonIgnore]
        public List<ApplicationRow> ApplicatonRows { get; set; }

        public TopCommand() { }

        public TopCommand(string commandResult)
        {
            ApplicatonRows = new List<ApplicationRow>();

            Raw = commandResult;

            string loadAverages = commandResult.Split("load average:")[1].Split("\n")[0];
            LoadAverage1Minute = (double.Parse(loadAverages.Split(",")[0].Trim(), CultureInfo.InvariantCulture) / 4.0) * 100;
            LoadAverage5Minute = (double.Parse(loadAverages.Split(",")[1].Trim(), CultureInfo.InvariantCulture) / 4.0) * 100;
            LoadAverage15Minute = (double.Parse(loadAverages.Split(",")[2].Trim(), CultureInfo.InvariantCulture) / 4.0) * 100;

            Time = DateTimeOffset.Parse(commandResult.Split("top -")[1].Split("up")[0].Trim()).DateTime;
            TotalCpuUsage = 100.0 - double.Parse(commandResult.Split("%Cpu(s):")[1].Split("ni,")[1].Split("id")[0].Trim(), CultureInfo.InvariantCulture);

            Uptime = commandResult.Split("load average:")[0].Split("up")[1].Split(",")[0].Trim();

            foreach (string row in commandResult.Split("PID")[1].Split("COMMAND")[1].Split("\n"))
            {
                if (string.IsNullOrEmpty(row.Trim()))
                    continue;

                ApplicationRow applicationRow = new ApplicationRow();
                List<string> values = row.Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();

                applicationRow.CpuUsage = double.Parse(values[8].Trim(), CultureInfo.InvariantCulture);
                applicationRow.MemoryUsage = double.Parse(values[9].Trim(), CultureInfo.InvariantCulture);
                applicationRow.Path = values[11].Trim();

                if (applicationRow.Path.Contains("/"))
                    applicationRow.Name = applicationRow.Path.Split("/").Last();
                else
                    applicationRow.Name = applicationRow.Path;

                string cpuTimeString = values[10].Trim();
                double minutes = double.Parse(cpuTimeString.Split(":")[0].Trim(), CultureInfo.InvariantCulture);
                double seconds = double.Parse(cpuTimeString.Split(":")[1].Trim(), CultureInfo.InvariantCulture);
                applicationRow.CpuTime = minutes * 60 + seconds;

                ApplicatonRows.Add(applicationRow);
            }
        }

        public ApplicationRow GetApplicationRow(string name)
        {
            string kebabVariation = name.ToKebabCasing();
            string pascalVariation = name.ToPascalCasing();

            foreach(ApplicationRow row in ApplicatonRows)
            {
                if (row.Name == kebabVariation || row.Name == pascalVariation)
                    return row;
            }

            return null;
        }

        public static async Task<TopCommand> GetCurrentAsync()
        {
            try
            {
                Command topCommand = new Command("top -n 1 -b -u pi -c -w512", WorkingDirectory.Default);
                return new TopCommand(await topCommand.RunAsync());
            }
            catch (Exception exception)
            {
                return new TopCommand() { Error = exception.Message };
            }
        }
    }
}
