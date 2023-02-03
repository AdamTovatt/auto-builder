using AutoBuilder.Helpers;
using System;
using System.Collections.Generic;
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
        public List<ApplicationRow> ApplicationRows { get; set; }

        public static async Task<SystemctlListCommand> GetCurrentAsync()
        {
            try
            {
                string commandResult = await (new Command("", WorkingDirectory.Default)).RunAsync();
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
            ApplicationRows = new List<ApplicationRow>();
        }
    }
}
