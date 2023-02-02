using AutoBuilder.Helpers;
using System;
using System.Threading.Tasks;

namespace AutoBuilder.Models
{
    public class TemperatureCommand
    {
        public string Error { get; set; }
        public double Temperature { get; set; }

        public static async Task<TemperatureCommand> GetCurrentAsync()
        {
            try
            {
                Command temperatureCommand = new Command("vcgencmd measure_temp", WorkingDirectory.Default);
                string result = await temperatureCommand.RunAsync();

                return new TemperatureCommand() { Temperature = double.Parse(result.Split("=")[1].Split("'")[0]) };
            }
            catch (Exception exception)
            {
                return new TemperatureCommand() { Error = exception.Message };
            }
        }
    }
}
