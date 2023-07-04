using LaunchPadConfigurator.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Diagnostics;
using System.Linq;

namespace LaunchPadConfigurator
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            settingsMenu.SelectionChanged += (s, e) =>
            {
                ChangeSelection(e.SelectedItem as NavigationViewItem);
            };

            Title = "LaunchPad settings";
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            ChangeSelection(settingsMenu.MenuItems.FirstOrDefault() as NavigationViewItem);
        }

        private void ChangeSelection(NavigationViewItem itemSelected)
        {
            if (itemSelected == null || itemSelected.Tag == null)
            {
                return;
            }
            switch (itemSelected.Content.ToString())
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
                case "Widgets":
                    ContentFrame.Navigate(typeof(WidgetsPage));
                    break;
            }
            settingsMenu.Header = itemSelected.Content;
            settingsMenu.SelectedItem = itemSelected;
        }


        private void OpenGithub(object sender, TappedRoutedEventArgs e)
        {
            ProcessStartInfo git = new()
            {
                UseShellExecute = true,
                FileName = "https://github.com/niilopoutanen/LaunchPad"
            };
            Process.Start(git);
        }
    }
}
