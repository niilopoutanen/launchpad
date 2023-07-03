using LaunchPadCore;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.UI.Core;
using Windows.System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using System.Security.Policy;
using Windows.ApplicationModel;

namespace LaunchPadConfigurator.Views.Pages
{

    public sealed partial class HomePage : Page
    {
        private UserPreferences preferences;
        public HomePage()
        {
            preferences = SaveSystem.LoadPreferences();
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
                preferences = SaveSystem.LoadPreferences();
                if (Enum.TryParse(ModifierComboBox.SelectedValue.ToString(), out HotKey.Modifiers selectedModifier))
                {
                    preferences.Modifier = selectedModifier;
                    SaveSystem.SavePreferences(preferences);
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
                if(btn.IsChecked == false)
                {
                    return;
                }
                keyPressed = KeyInterop.KeyFromVirtualKey((int)e.Key);

                preferences.Key = keyPressed;
                SaveSystem.SavePreferences(preferences);

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
            preferences = SaveSystem.LoadPreferences();
            preferences.Modifier = HotKey.Modifiers.Shift;
            preferences.Key = Key.Tab;
            SaveSystem.SavePreferences(preferences);
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
                Process.Start("explorer.exe", "shell:appsfolder\\923NiiloPoutanen.364392126B592_5y1c2t4szcgd8!App");
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
