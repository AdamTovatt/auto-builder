using System.Threading.Tasks;

namespace AutoBuilder.Models
{
    public class SystemctlListCommand
    {
        public static async Task<SystemctlListCommand> GetCurrentAsync()
        {
            return new SystemctlListCommand();
        }
    }
}
