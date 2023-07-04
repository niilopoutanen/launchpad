using System.Windows.Input;

namespace LaunchPadCore
{
    public class UserPreferences
    {
        private int preferredWidth = 600;
        public int PreferredWidth
        {
            get => preferredWidth;
            set => preferredWidth = value;
        }


        private bool nameVisible = false;
        public bool NameVisible
        {
            get => nameVisible;
            set => nameVisible = value;
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
            get => selectedTheme;
            set => selectedTheme = value;
        }


        private bool transparentTheme = true;
        public bool TransparentTheme
        {
            get => transparentTheme;
            set => transparentTheme = value;
        }

        private bool fullSizeIcon = false;
        public bool FullSizeIcon
        {
            get => fullSizeIcon;
            set => fullSizeIcon = value;
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
            get => selectedAnimation;
            set => selectedAnimation = value;
        }


        private HotKey.Modifiers modifier = HotKey.Modifiers.Shift;
        public HotKey.Modifiers Modifier
        {
            get => modifier;
            set => modifier = value;
        }

        private Key key = Key.Tab;
        public Key Key
        {
            get => key;
            set => key = value;
        }


        private bool useSystemAccent = false;
        public bool UseSystemAccent
        {
            get => useSystemAccent;
            set => useSystemAccent = value;
        }

        private bool themedWidgets = true;
        public bool ThemedWidgets
        {
            get => themedWidgets;
            set => themedWidgets = value; 
        }

        public Dictionary<string, bool>? ActiveWidgets { get; set; }
    }
}
