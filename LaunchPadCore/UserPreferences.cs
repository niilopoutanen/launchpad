using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadCore
{
    public class UserPreferences
    {
        private int preferredWidth = 600;

        public int PreferredWidth
        {
            get { return preferredWidth; }
            set { preferredWidth = value; }
        }


        private bool nameVisible = false;
        public bool NameVisible 
        { 
            get { return nameVisible; }
            set { nameVisible = value; }
        }

        public enum LaunchPadTheme
        {
            Dark,
            Light,
            System
        }
        private LaunchPadTheme selectedTheme = LaunchPadTheme.System;
        public LaunchPadTheme SelectedTheme
        {
            get { return selectedTheme; }
            set { selectedTheme = value; }
        }


        private bool transparentTheme = true;
        public bool TransparentTheme
        {
            get { return transparentTheme; }
            set { transparentTheme = value; }
        }
    }
}
