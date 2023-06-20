using LaunchPadCore;
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
using LaunchPad.Apps;

namespace LaunchPad
{
    public partial class LaunchPadWindow : Window
    {
        UserPreferences preferences;
        List<ILaunchPadItem> items = new();
        public LaunchPadWindow()
        {
            preferences = SaveSystem.LoadPreferences();
            ResourceDictionary activeTheme = SaveSystem.LoadTheme();
            InitializeComponent();
            LoadApps();

            SetTheme(activeTheme);
            SystemEvents.UserPreferenceChanged += (s, e) =>
            {
                SetTheme(activeTheme);
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
            var suggestion = new Suggestion("No apps added. Open configurator to add some.", CloseWithAnim);
            items.Add(suggestion);
            appContainer.Children.Add(suggestion);
            
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


        private void SetTheme(ResourceDictionary resourceDictionary)
        {
            SolidColorBrush backgroundColor = resourceDictionary["LaunchPadBackground"] as SolidColorBrush;

            launchPadRoot.Background = backgroundColor;
            foreach(ILaunchPadItem item in appContainer.Children)
            {
                item.SetTheme(resourceDictionary);
            }
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
