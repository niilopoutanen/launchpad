using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace LaunchPad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private LaunchPadWindow launchPadWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create an instance of HwndSource for the main window
            var hwndSource = new HwndSource(0, 0, 0, 0, 0, "LaunchPadClass", IntPtr.Zero);

            // Create an instance of CustomHotKey and register the Shift + Tab hotkey
            var hotKey = new HotKey(hwndSource)
            {
                Key = Key.Tab,
                ModifierKeys = HotKey.Modifiers.Shift
            };

            hotKey.HotKeyPressed += HotKey_HotKeyPressed;
            hotKey.Enabled = true;
        }

        private void HotKey_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            if (launchPadWindow != null && launchPadWindow.IsVisible)
            {
                launchPadWindow.CloseWithAnim();
                return;
            }

            launchPadWindow = new LaunchPadWindow();
            launchPadWindow.Show();
        }

    }
}
