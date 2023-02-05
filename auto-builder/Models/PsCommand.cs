using AutoBuilder.Helpers;
using System.Threading.Tasks;
using System;

namespace AutoBuilder.Models
{
    public class PsCommand
    {
        public string CommandResult { get; set; }

        public PsCommand() { }
        public PsCommand(string commandResult)
        {
            CommandResult = commandResult;
        }

        public static async Task<TopCommand> GetCurrentAsync()
        {
            try
            {
                Command topCommand = new Command("ps -e -o cmd:512,pcpu,%mem,time --sort=-%mem | head -n 1000", WorkingDirectory.Default);
                return new TopCommand(await topCommand.RunAsync());
            }
            catch (Exception exception)
            {
                return new TopCommand() { Error = exception.Message };
            }
        }
    }
}
