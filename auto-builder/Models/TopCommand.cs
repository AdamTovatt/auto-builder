using AutoBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuilder.Models
{
    public class TopCommand
    {
        public class TopCommandApplicationRow
        {
            public double CpuUsage { get; set; }
            public double MemoryUsage { get; set; }
            public double CpuTime { get; set; }
            public string Name { get; set; }
        }

        public string Error { get; set; }
        public double LoadAverage1Minute { get; set; }
        public double LoadAverage5Minute { get; set; }
        public double LoadAverage15Minute { get; set; }
        public List<TopCommandApplicationRow> ApplicatonRows { get; set; }

        public TopCommand() { }

        public TopCommand(string commandResult)
        {
            ApplicatonRows = new List<TopCommandApplicationRow>();

            string loadAverages = commandResult.Split("load average:")[1].Split("\n")[0];

            LoadAverage1Minute = double.Parse(loadAverages.Split(",")[0].Trim(), CultureInfo.InvariantCulture);
            LoadAverage5Minute = double.Parse(loadAverages.Split(",")[1].Trim(), CultureInfo.InvariantCulture);
            LoadAverage15Minute = double.Parse(loadAverages.Split(",")[2].Trim(), CultureInfo.InvariantCulture);

            foreach (string row in commandResult.Split("PID")[1].Split("COMMAND")[1].Split("\n"))
            {
                if (string.IsNullOrEmpty(row.Trim()))
                    continue;

                TopCommandApplicationRow applicationRow = new TopCommandApplicationRow();
                List<string> values = row.Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();

                applicationRow.CpuUsage = double.Parse(values[8].Trim(), CultureInfo.InvariantCulture);
                applicationRow.MemoryUsage = double.Parse(values[9].Trim(), CultureInfo.InvariantCulture);
                applicationRow.Name = values[11].Trim();
                ApplicatonRows.Add(applicationRow);
            }
        }

        public static async Task<TopCommand> GetCurrentAsync()
        {
            try
            {
                Command topCommand = new Command("top -n 1 -b -u pi", WorkingDirectory.Default);
                return new TopCommand(await topCommand.RunAsync());
            }
            catch (Exception exception)
            {
                return new TopCommand() { Error = exception.Message };
            }
        }
    }
}
