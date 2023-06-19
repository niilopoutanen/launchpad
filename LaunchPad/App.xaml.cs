using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;


namespace LaunchPad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private LaunchPadWindow launchPadWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var hwndSource = new HwndSource(0, 0, 0, 0, 0, "LaunchPadClass", IntPtr.Zero);
            var hotKey = new HotKey(hwndSource)
            {
                Key = Key.Tab,
                ModifierKeys = HotKey.Modifiers.Shift
            };

            hotKey.HotKeyPressed += (s,e) =>
            {
                ToggleLaunchpad();
            };
            hotKey.Enabled = true;
            StartSystemTrayApp();
        }

        private void ToggleLaunchpad()
        {
            if (launchPadWindow != null && launchPadWindow.IsVisible)
            {
                launchPadWindow.CloseWithAnim();
                return;
            }

            launchPadWindow = new LaunchPadWindow();
            launchPadWindow.Show();
            launchPadWindow.Activate();
        }
        private void StartSystemTrayApp()
        {
            NotifyIcon notifyIcon = new()
            {
                Icon = new System.Drawing.Icon("Resources/Assets/icon.ico"),
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
            notifyIcon.ContextMenuStrip.Items.Add("Settings", null, (s,e) =>
            {

            });
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) =>
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                Current.Shutdown();
            });
        }
    }
}
