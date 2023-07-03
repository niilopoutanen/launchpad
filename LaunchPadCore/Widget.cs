using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadCore
{
    public abstract class Widget : LaunchPadItem
    {
        public abstract string WidgetName { get; }
        public abstract string Description { get; }
        public abstract string IconPath { get; }
        public abstract bool Active { get; set; }
    }
}
