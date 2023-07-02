﻿using LaunchPadConfigurator;
using LaunchPadCore;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Windows.ApplicationModel;
using System.Windows.Interop;

namespace LaunchPad
{
    public partial class App : System.Windows.Application
    {
        private LaunchPadWindow? launchPadWindow;
        private NotifyIcon? notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EngageHotKey();
            StartSystemTrayApp();
        }
        private void EngageHotKey()
        {
            UserPreferences preferences = SaveSystem.LoadPreferences();
            var hwndSource = new HwndSource(0, 0, 0, 0, 0, "LaunchPadCore", IntPtr.Zero);
            var hotKey = new HotKey(hwndSource)
            {
                Key = preferences.Key,
                ModifierKeys = preferences.Modifier
            };

            hotKey.HotKeyPressed += (s, e) =>
            {
                ToggleLaunchpad();
            };
            try
            {
                hotKey.Enabled = true;
            }
            catch (Win32Exception)
            {
                DisplayMessage("Error", "Could not register the hotkey. Most likely LaunchPad is already running.", ToolTipIcon.Info);
            }
        }
        public void ToggleLaunchpad()
        {
            if (launchPadWindow != null && launchPadWindow.IsVisible)
            {
                launchPadWindow.Terminate();
                return;
            }

            launchPadWindow = new LaunchPadWindow();
            launchPadWindow.Show();
            launchPadWindow.Activate();
        }
        private void StartSystemTrayApp()
        {
            Uri iconUri = new Uri("pack://application:,,,/Resources/Assets/icon.ico");
            System.IO.Stream iconStream = System.Windows.Application.GetResourceStream(iconUri).Stream;

            notifyIcon = new()
            {
                Icon = new System.Drawing.Icon(iconStream),
                Visible = true
            };

            notifyIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ToggleLaunchpad();
                }
            };

            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Settings", null, async (s, e) =>
            {
                try
                {
                    Package package = Package.Current;

                    var appListEntries = await package.GetAppListEntriesAsync();
                    string path = System.IO.Path.Combine(appListEntries[0].AppInfo.Package.InstalledPath, "LaunchPadConfigurator", "LaunchPadConfigurator.exe");

                    Process.Start(path);
                }
                catch
                {
                    DisplayMessage("Error", "Could not start LaunchPad configurator. Make sure the app is installed correctly.", ToolTipIcon.Error);
                }
            });
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) =>
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                Current.Shutdown();
            });
        }
        public void DisplayMessage(string title, string msg, ToolTipIcon icon)
        {
            if(notifyIcon == null)
            {
                System.Windows.MessageBox.Show(title, msg);
                return;
            }
            notifyIcon.ShowBalloonTip(3000, title, msg, icon);
        }
    }
}
