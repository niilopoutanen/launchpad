using LaunchPadClassLibrary;
using LaunchPadConfigurator;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LaunchPad
{
    public partial class LaunchPadWindow : Window
    {
        UserPreferences preferences;
        List<ILaunchPadItem> items = new();
        public LaunchPadWindow()
        {
            preferences = SaveSystem.LoadPreferences();
            InitializeComponent();
            LoadApps();

            SetTheme();
            SystemEvents.UserPreferenceChanged += (s, e) =>
            {
                SetTheme();
            };
            HandleKeyboard(this);
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
        private void LoadApps()
        {
            List<AppShortcut> apps = SaveSystem.LoadApps();
            foreach (AppShortcut app in apps)
            {
                var icon = new Icon(app, CloseWithAnim);
                icon.Margin = new Thickness(5);
                items.Add(icon);
                appContainer.Children.Add(icon);
            }
            int itemCount = appContainer.Children.Count;
            int maxColumns = preferences.ColumnCount;
            int columns = Math.Min(itemCount, maxColumns);

            appContainer.Columns = columns;
        }

        public void CloseWithAnim()
        {
            Storyboard launchPadClose = ((Storyboard)this.FindResource("WindowExitAnimation")).Clone();
            launchPadClose.Completed += (s, e) =>
            {
                this.Close();
            };
            launchPadClose.Begin(this);
        }

        private void SetTheme()
        {
            SolidColorBrush? backgroundColor;
            SolidColorBrush? itemBackgroundColor;

            if (IsLightTheme())
            {
                var lightModeDictionary = new ResourceDictionary
                {
                    Source = new Uri("Resources/LightMode.xaml", UriKind.Relative)
                };

                backgroundColor = lightModeDictionary["LaunchPadBackground"] as SolidColorBrush;
                itemBackgroundColor = lightModeDictionary["LaunchPadItemBackground"] as SolidColorBrush;

            }
            else
            {
                var darkModeDictionary = new ResourceDictionary
                {
                    Source = new Uri("Resources/DarkMode.xaml", UriKind.Relative)
                };

                backgroundColor = darkModeDictionary["LaunchPadBackground"] as SolidColorBrush;
                itemBackgroundColor = darkModeDictionary["LaunchPadItemBackground"] as SolidColorBrush;
            }


            if(backgroundColor == null || itemBackgroundColor == null)
            {
                return;
            }
            launchPadRoot.Background = backgroundColor;
            foreach(UIElement item in appContainer.Children)
            {
                try
                {
                    Icon app = (Icon)item;
                    if(app.App.IconSize != AppShortcut.SIZE_FULL)
                    {
                        app.iconContainer.Background = itemBackgroundColor;
                    }
                    
                }
                catch (Exception) { }
            }
        }


        private static bool IsLightTheme()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var value = key?.GetValue("AppsUseLightTheme");
            return value is int i && i > 0;
        }

        private void HandleKeyboard(Window window)
        {
            window.KeyDown += (s, e) =>
            {
                int keyNumber = IsNumberKey(e.Key);

                if (keyNumber >= 0 && keyNumber < items.Count)
                {
                    items[keyNumber].OnPress();
                }
            };
            window.KeyUp += (s, e) =>
            {
                int keyNumber = IsNumberKey(e.Key);

                if (keyNumber >= 0 && keyNumber < items.Count)
                {
                    items[keyNumber].OnRelease();
                }
            };
        }

        private int IsNumberKey(Key keyPressed)
        {
            bool isNumberKey = (keyPressed >= Key.D1 && keyPressed <= Key.D9) || (keyPressed >= Key.NumPad1 && keyPressed <= Key.NumPad9);
            if (isNumberKey)
            {
                return keyPressed - Key.D1;
            }
            return -1;
        }
    }
}
