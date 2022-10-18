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

        public override string ToString()
        {
            return string.Format("{0} {1}", FileName, Arguments);
        }
    }
}
