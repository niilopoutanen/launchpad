using LaunchPadConfigurator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadClassLibrary
{
    public class AppShortcut
    {
        public const int SIZE_FULL = 1;
        public const int SIZE_CROPPED = 0;


        public string Name {  get; set; }
        public string ExeUri { get; set; }
        public string? IconFileName { get; set; }

        public int IconSize { get; set; }

        public AppShortcut(string name, string exeUri, string? iconFileName, int iconSize)
        {
            Name = name;
            ExeUri = exeUri;
            if(iconFileName != null)
            {
                IconFileName = iconFileName;
            }
            IconSize = iconSize;
        }
        public AppShortcut() { }


        public string GetIconFullPath()
        {
            if(IconFileName == null)
            {
                return ExeUri;
            }
            string filename = Path.GetFileName(IconFileName);
            if (IconFileName != filename)
            {
                return IconFileName;
            }
            return Path.Combine(SaveSystem.iconsDirectory, IconFileName);
        }
    }
}
