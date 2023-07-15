using LaunchPadCore.Models;
using LaunchPadCore.Utility;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.ApplicationModel;

namespace LaunchPadConfiguratorWPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private UserPreferences preferences;
        public HomePage()
        {
            preferences = UserPreferences.Load();
            InitializeComponent();
        }

        private void InitializeElements()
        {
            GetLaunchPadStatus();

            PackageVersion version = Package.Current.Id.Version;
            versionNumber.Text = string.Format("v{0}.{1}.{2}", version.Major, version.Minor, version.Build);

            ModifierComboBox.ItemsSource = Enum.GetValues(typeof(HotKey.Modifiers));
            ModifierComboBox.SelectedItem = preferences.Modifier;
            ModifierComboBox.SelectionChanged += (s, e) =>
            {
                preferences = UserPreferences.Load();
                if (Enum.TryParse(ModifierComboBox.SelectedValue.ToString(), out HotKey.Modifiers selectedModifier))
                {
                    preferences.Modifier = selectedModifier;
                    preferences.Save();
                }
            };

            KeyButton.Content = preferences.Key;
        }
        private void GetLaunchPadStatus()
        {
            string appName = "LaunchPad";
            string publisher = "Niilo Poutanen";

            Process[] processes = Process.GetProcessesByName(appName);

            bool isRunning = false;

            foreach (Process process in processes)
            {
                if (process.MainModule.FileVersionInfo.CompanyName == publisher)
                {
                    isRunning = true;
                    break;
                }
            }

            if (isRunning)
            {
                launchPadStatus.Text = "LaunchPad is currently running.";
                launchPadManageButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                launchPadManageButton.Visibility = Visibility.Visible;
                launchPadStatus.Text = "LaunchPad is not currently running.";
                launchPadManageButton.Content = "Start Launchpad";
                launchPadManageButton.Click += async (s, e) =>
                {
                    launchPadManageButton.IsEnabled = false;
                    await TryStartLaunchPad();
                    await Task.Delay(500);
                    GetLaunchPadStatus();
                    launchPadManageButton.IsEnabled = true;
                };
            }
        }

        private async Task TryStartLaunchPad()
        {
            try
            {
                Core.LaunchApp(Core.APP_LAUNCHPAD);
            }
            catch (Exception)
            {
                throw new NotImplementedException("Custom alert here");
            }


        }

    }
}
