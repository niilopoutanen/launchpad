using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Executable
    {
        public string URI { get; set; }
        public string ExePath { get; set; }


        public void Launch()
        {
            System.Diagnostics.Process.Start(ExePath);
        }
    }
}
