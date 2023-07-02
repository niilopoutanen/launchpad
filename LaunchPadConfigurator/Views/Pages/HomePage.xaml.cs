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

namespace LaunchPadConfigurator.Views.Pages
{

    public sealed partial class HomePage : Page
    {
        private Process launchPadProcess;
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
            versionNumber.Text = SaveSystem.launchPadVersion;


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
                    launchPadProcess = process;
                    break;
                }
            }

            if (isRunning)
            {
                launchPadStatus.Text = "LaunchPad is currently running.";
                launchPadManageButton.Content = "Close Launchpad";
                launchPadManageButton.Click += (s, e) =>
                {
                    TryCloseLaunchPad();
                };
            }
            else
            {
                launchPadStatus.Text = "LaunchPad is not currently running.";
                launchPadManageButton.Content = "Start Launchpad";
                launchPadManageButton.Click += async (s, e) =>
                {
                    await TryStartLaunchPad();
                    GetLaunchPadStatus();
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
        private void TryCloseLaunchPad()
        {
            try
            {
                if (launchPadProcess != null)
                {
                    launchPadProcess.Kill();
                    launchPadProcess.WaitForExit();
                    launchPadProcess.Dispose();
                    launchPadProcess = null;
                    GetLaunchPadStatus();
                }
                else
                {
                    throw new Exception("Could not shut down the process");
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
