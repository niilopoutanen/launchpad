using LaunchPadCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadConfigurator.Views.Dialogs
{
    public interface IAppDialog
    {
        public AppShortcut Input { get; set; }
        public AppShortcut Get();
    }
}
