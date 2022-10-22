using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuilder.Models
{
    public class Command
    {
        public string WorkingDirectory { get; set; }
        public string FileName { get; set; }
        public string Arguments { get; set; }

        public Command(string workingDirectory, string fileName, string arguments)
        {
            WorkingDirectory = workingDirectory;
            FileName = fileName;
            Arguments = arguments;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", FileName, Arguments);
        }
    }
}
