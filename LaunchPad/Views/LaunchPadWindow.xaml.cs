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
        List<AppShortcut> apps = new();
        public LaunchPadWindow()
        {
            InitializeComponent();
            LoadApps();

            SetTheme();
            SystemEvents.UserPreferenceChanged += (s, e) =>
            {
                SetTheme();
            };
            HandleClicks(this);
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
        private void LoadApps()
        {
            apps = SaveSystem.LoadApps();
            foreach (AppShortcut app in apps)
            {
                AddApp(app);
            }
            if(apps.Count > 0)
            {
                //Remove last exess gap
                appContainer.Children.RemoveAt(appContainer.Children.Count - 1);
            }

        }
        private void AddApp(AppShortcut app)
        {
            var icon = new Icon(app, OpenApp);
            var gap = new Border
            {
                Width = 10
            };
            appContainer.Children.Add(icon);
            appContainer.Children.Add(gap);
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
                    if(app.app.IconSize != AppShortcut.SIZE_FULL)
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


        private void HandleClicks(Window window)
        {
            window.KeyUp += (s, e) =>
            {
                bool isNumberKey = (e.Key >= Key.D1 && e.Key <= Key.D9) || (e.Key >= Key.NumPad1 && e.Key <= Key.NumPad9);
                if (isNumberKey)
                {
                    int keyNumber = e.Key - Key.D1;

                    if (keyNumber >= 0 && keyNumber < apps.Count)
                    {
                        OpenApp(apps[keyNumber].ExeUri);
                    }
                }
            };
        }

        private void OpenApp(string appURI)
        {
            Process process = new Process();
            process.StartInfo.FileName = appURI;
            process.StartInfo.UseShellExecute = true;

            process.Start();
            CloseWithAnim();
        }
    }
}
