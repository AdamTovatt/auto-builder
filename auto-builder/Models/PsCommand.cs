using AutoBuilder.Helpers;
using System.Threading.Tasks;
using System;

namespace AutoBuilder.Models
{
    public class PsCommand
    {
        public string Error { get; set; }
        public string CommandResult { get; set; }

        public PsCommand() { }
        public PsCommand(string commandResult)
        {
            CommandResult = commandResult;
        }

        public static async Task<PsCommand> GetCurrentAsync()
        {
            try
            {
                Command psCommand = new Command("ps -e -o cmd:512,pcpu,%mem,time --sort=-%mem | head -n 1000", WorkingDirectory.Default);
                return new PsCommand(await psCommand.RunAsync());
            }
            catch (Exception exception)
            {
                return new PsCommand() { Error = exception.Message };
            }
        }
    }
}
