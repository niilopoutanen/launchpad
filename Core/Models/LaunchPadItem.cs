using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public abstract class LaunchPadItem
    {
        public string Name { get; set; }
        public Executable Executable { get; set; }
        public string IconPath { get; set; }
        public int Index { get; set; }
    }
}
