using LaunchPadCore.Common.Models;
using LaunchPadCore.Common.Utility;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;

namespace LaunchPadConfigurator.Views.Pages
{

    public sealed partial class HomePage : Page
    {
        private UserPreferences preferences;
        public HomePage()
        {
            preferences = UserPreferences.Load();
            this.InitializeComponent();
            this.InitializeElements();

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
        private void ListenForKeyChange(object sender, RoutedEventArgs e)
        {
            Key keyPressed = Key.Tab;
            ToggleButton btn = (ToggleButton)sender;

            btn.KeyDown += (s, e) =>
            {
                if (btn.IsChecked == false)
                {
                    return;
                }
                keyPressed = KeyInterop.KeyFromVirtualKey((int)e.Key);

                preferences.Key = keyPressed;
                preferences.Save();

                btn.IsChecked = false;
                btn.Content = keyPressed;
            };
            if (btn.IsChecked == true)
            {
                btn.Content = "Press a key";
            }
        }
        private void RestoreHotkey(object sender, RoutedEventArgs e)
        {
            preferences = UserPreferences.Load();
            preferences.Modifier = HotKey.Modifiers.Shift;
            preferences.Key = Key.Tab;
            preferences.Save();
            ModifierComboBox.SelectedItem = preferences.Modifier;
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
                ContentDialog dialog = new()
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Could not start LaunchPad.",
                    PrimaryButtonText = "Ok",
                    DefaultButton = ContentDialogButton.Primary
                };

                await dialog.ShowAsync();
            }


        }

    }
}
