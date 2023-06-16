using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadClassLibrary
{
    public class AppShortcut
    {
        private string Name {  get; set; }
        private string ExeUri { get; set; }
        private string? IconFileName { get; set; }

        public AppShortcut(string name, string exeUri)
        {
            Name = name;
            ExeUri = exeUri;
        }
        public AppShortcut(string name, string exeUri, string iconFileName)
        {
            Name = name;
            ExeUri = exeUri;
            IconFileName = iconFileName;
        }
    }
}
