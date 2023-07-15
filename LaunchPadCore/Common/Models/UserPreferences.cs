using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using System.Xml.Linq;
using LaunchPadCore.Utility;

namespace LaunchPadCore.Models
{
    public class UserPreferences
    {
        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(this);
            SaveSystem.VerifyPathIntegrity();
            using (StreamWriter streamWriter = new(SaveSystem.preferences))
            {
                streamWriter.Write(jsonString);
            }
        }
        public static UserPreferences Load()
        {
            UserPreferences prefs = new();
            SaveSystem.VerifyPathIntegrity();
            if (File.Exists(SaveSystem.preferences))
            {
                string jsonString = File.ReadAllText(SaveSystem.preferences) ?? throw new FileLoadException("File is empty");
                prefs = JsonSerializer.Deserialize<UserPreferences>(jsonString);
            }
            if (prefs != null)
            {
                return prefs;
            }
            else
            {
                return new UserPreferences();
            }
        }


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
        private double animationSpeed = 1;
        public double AnimationSpeed
        {
            get => animationSpeed;
            set
            {
                double min = 0.5;
                double max = 3;
                animationSpeed = value < min ? min : value > max ? max : value;
            }
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

        private bool rememberWidgetVariation = true;
        public bool RememberWidgetVariation
        {
            get => rememberWidgetVariation;
            set => rememberWidgetVariation = value;
        }

        private Dictionary<string, bool>? activeWidgets = new();
        public Dictionary<string, bool> ActiveWidgets
        {
            get { return activeWidgets ?? new Dictionary<string, bool>(); }
            set { activeWidgets = value; }
        }

        private Dictionary<string, int>? widgetVariations = new();
        public Dictionary<string, int>? WidgetVariations
        {
            get { return widgetVariations ?? new Dictionary<string, int>(); }
            set { widgetVariations = value; }
        }

    }
}
