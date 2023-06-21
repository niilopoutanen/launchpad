using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

namespace LaunchPadConfigurator.Views.Pages
{

    public sealed partial class HomePage : Page
    {
        private Process launchPadProcess;
        public HomePage()
        {
            this.InitializeComponent();
            this.InitializeElements();

        }
        private void InitializeElements()
        {
            GetLaunchPadStatus();
            versionNumber.Text = SaveSystem.launchPadVersion;
            settingsVersionNumber.Text = SaveSystem.launchPadSettingsVersion;
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
                launchPadManageButton.Click += (s, e) =>
                {
                    TryStartLaunchPad();
                    GetLaunchPadStatus();
                };
            }
        }
        private static void TryStartLaunchPad()
        {
            try
            {
                Process process = Process.Start(SaveSystem.LaunchPadExecutable);
                if (process == null)
                {
                    throw new Exception("Process did not start");
                }
            }
            catch (Exception)
            {

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
