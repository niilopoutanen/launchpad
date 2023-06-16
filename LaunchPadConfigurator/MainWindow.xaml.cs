using LaunchPadConfigurator.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Display.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaunchPadConfigurator
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            settingsMenu.SelectionChanged += SettingsMenu_SelectionChanged;

            SaveSystem.SaveApps(new List<LaunchPadClassLibrary.AppShortcut>());
        }

        private void SettingsMenu_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItemContainer as NavigationViewItem;
            if (item == null)
            {
                return;
            }

            if (item.Tag == null)
            {
                return;
            }
            switch (item.Content.ToString())
            {
                case "General":
                    ContentFrame.Navigate(typeof(GeneralPage));
                    break;
                case "Apps":
                    ContentFrame.Navigate(typeof(AppsPage));
                    break;
                case "Home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
            }
            settingsMenu.Header = item.Content;
            settingsMenu.SelectedItem = item;
        }
    }
}
