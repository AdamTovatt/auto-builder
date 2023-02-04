using AutoBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuilder.Models
{
    public class SystemctlListCommand
    {
        public class ApplicationRow
        {
            public string Status { get; set; }
            public string SubStatus { get; set; }
            public string Name { get; set; }
        }

        public string Error { get; set; }
        public List<ApplicationRow> ApplicationRows { get { return applicationRows.Values.ToList(); } }

        private Dictionary<string, ApplicationRow> applicationRows;

        public static async Task<SystemctlListCommand> GetCurrentAsync()
        {
            try
            {
                string commandResult = await (new Command("sudo systemctl --no-pager --type=service", WorkingDirectory.Default)).RunAsync();
                return new SystemctlListCommand(commandResult);
            }
            catch(Exception exception)
            {
                return new SystemctlListCommand() { Error = exception.Message };
            }
        }

        public SystemctlListCommand() { }

        public SystemctlListCommand(string commandResult)
        {
            applicationRows = new Dictionary<string, ApplicationRow>();

            string rows = commandResult.Split("DESCRIPTION")[1].Split("LOAD   = Reflects")[0].Trim();

            foreach(string row in rows.Split("\n"))
            {
                if (string.IsNullOrEmpty(row.Trim()))
                    continue;

                string processedRow = row.Trim();
                ApplicationRow application = new ApplicationRow();
                application.Name = processedRow.Split(" ")[0];

                for (int i = application.Name.Length; i < processedRow.Length; i++)
                {
                    if (char.IsLetter(processedRow[i]))
                    {
                        processedRow = processedRow.Substring(i);
                        break;
                    }
                }

                application.Status = processedRow.Split(" ")[1];
                application.SubStatus = processedRow.Split(" ")[2];

                applicationRows.Add(application.Name, application);
            }
        }

        public ApplicationRow GetApplication(string name)
        {
            if (applicationRows.TryGetValue(string.Format("{0}.service", name), out ApplicationRow row))
                return row;
            return null;
        }
    }
}
