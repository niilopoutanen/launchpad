using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadClassLibrary
{
    public class AppShortcut
    {
        public string Name {  get; set; }
        public string ExeUri { get; set; }
        public string? IconFileName { get; set; }

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
        public AppShortcut() { }
    }
}
