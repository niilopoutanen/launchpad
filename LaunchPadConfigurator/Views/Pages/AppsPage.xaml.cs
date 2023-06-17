using LaunchPadClassLibrary;
using LaunchPadConfigurator.Views.UIElements;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;

namespace LaunchPadConfigurator
{
    public sealed partial class AppsPage : Page
    {
        public AppsPage()
        {
            this.InitializeComponent();
            RefreshAppList();

            addAppButton.Click += async (s, e) =>
            {
                await ShowDialog(null);
            };
        }

        private void RefreshAppList()
        {
            appsList.Children.Clear();
            List<AppShortcut> apps = SaveSystem.LoadApps();

            foreach (AppShortcut app in apps)
            {
                AppListItem listItem = new(app, RefreshAppList, ShowDialog);
                appsList.Children.Add(listItem);
            }
        }

        private async Task ShowDialog(AppShortcut existingApp)
        {
            var window = (Application.Current as App)?.Window;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            

            var dialog = new ContentDialog
            {
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Add a new app",
                XamlRoot = (Application.Current as App)?.Window.Content.XamlRoot,
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,

                
            };
            if (existingApp == null)
            {
                AddAppDialog dialogContent = new(hWnd);
                dialog.Content = dialogContent;
                dialog.PrimaryButtonClick += (s, e) =>
                {
                    string name = dialogContent.AppName;
                    string exepath = dialogContent.ExePath;
                    string iconpath = dialogContent.IconPath;
                    int iconSize = dialogContent.FullSizeIcon ? AppShortcut.SIZE_FULL : AppShortcut.SIZE_CROPPED;
                    if (name == null || exepath == null)
                    {
                        throw new Exception("invalid inputs");
                    }

                    AppShortcut app = new(name, exepath, iconpath, iconSize);
                    SaveSystem.SaveApp(app);
                    RefreshAppList();
                };
            }
            else
            {
                AddAppDialog updateDialogContent = new(hWnd, existingApp);
                dialog.Title = "Update app details";
                dialog.Content = updateDialogContent;
                dialog.PrimaryButtonClick += (s, e) =>
                {
                    string name = updateDialogContent.AppName;
                    string exepath = updateDialogContent.ExePath;
                    string iconpath = updateDialogContent.IconPath;
                    int iconSize = updateDialogContent.FullSizeIcon ? AppShortcut.SIZE_FULL : AppShortcut.SIZE_CROPPED;
                    if (name == null || exepath == null)
                    {
                        throw new Exception("invalid inputs");
                    }

                    List<AppShortcut> existingApps = new List<AppShortcut>();
                    existingApps.Remove(existingApp);
                    AppShortcut app = new(name, exepath, iconpath, iconSize);
                    existingApps.Add(app);
                    SaveSystem.SaveApps(existingApps);

                    RefreshAppList();
                };
            }

            await dialog.ShowAsync();
        }
    }
}
