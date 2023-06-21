using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
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

            var hwndSource = new HwndSource(0, 0, 0, 0, 0, "LaunchPadCore", IntPtr.Zero);
            var hotKey = new HotKey(hwndSource)
            {
                Key = Key.Tab,
                ModifierKeys = HotKey.Modifiers.Shift
            };

            hotKey.HotKeyPressed += (s, e) =>
            {
                ToggleLaunchpad();
            };
            StartSystemTrayApp();
            try
            {
                hotKey.Enabled = true;
            }
            catch (Win32Exception)
            {
                DisplayMessage("Error", "Could not register the hotkey. Most likely LaunchPad is already running.", ToolTipIcon.Info);
            }
            
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
            notifyIcon = new()
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
            notifyIcon.ContextMenuStrip.Items.Add("Settings", null, (s, e) =>
            {

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
