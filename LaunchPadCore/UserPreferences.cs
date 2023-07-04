using System.Windows.Input;

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

        private bool fullSizeIcon = false;
        public bool FullSizeIcon
        {
            get { return fullSizeIcon; }
            set { fullSizeIcon = value; }
        }

        public enum AnimationTypes
        {
            SlideBottom,
            SlideTop,
            Center
        }
        private AnimationTypes selectedAnimation = AnimationTypes.SlideTop;
        public AnimationTypes SelectedAnimation
        {
            get { return selectedAnimation; }
            set { selectedAnimation = value; }
        }


        private HotKey.Modifiers modifier = HotKey.Modifiers.Shift;
        public HotKey.Modifiers Modifier
        {
            get { return modifier; }
            set { modifier = value; }
        }

        private Key key = Key.Tab;
        public Key Key
        {
            get { return key; }
            set { key = value; }
        }


        private bool useSystemAccent = false;
        public bool UseSystemAccent
        {
            get { return useSystemAccent; }
            set { useSystemAccent = value; }
        }

        public Dictionary<string, bool>? ActiveWidgets { get; set; }
    }
}
