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
            Transparent,
            FollowSystem
        }
        private LaunchPadTheme selectedTheme = LaunchPadTheme.Transparent;
        public LaunchPadTheme SelectedTheme
        {
            get { return selectedTheme; }
            set { selectedTheme = value; }
        }
    }
}
