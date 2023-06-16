using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaunchPadConfigurator.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            GetLaunchPadStatus();
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
            }
            else
            {
                launchPadStatus.Text = "LaunchPad is not currently running.";
            }
        }
    }
}
