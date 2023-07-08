using LaunchPadConfigurator.Views.Dialogs;
using LaunchPadCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaunchPadConfigurator.Views.Pages
{
    public sealed partial class AppsPage : Page
    {
        public AppsPage()
        {
            this.InitializeComponent();
            RefreshAppList();

            addAppButton.Click += async (s, e) =>
            {
                await ShowInput();
                //await ShowDialog(null);
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

            UserPreferences preferences = SaveSystem.LoadPreferences();
            List<Widget> widgets = SaveSystem.LoadWidgets();
            foreach (Widget widget in widgets)
            {
                foreach (string key in preferences.ActiveWidgets.Keys)
                {
                    if (widget.ID == key && preferences.ActiveWidgets[key] == true)
                    {
                        AppListItem widgetListItem = new(widget, RefreshAppList);
                        appsList.Children.Add(widgetListItem);
                        break;
                    }
                }
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


            AddAppDialog dialogContent = new(hWnd);
            dialog.Content = dialogContent;
            dialog.PrimaryButtonClick += (s, e) =>
            {
                if (dialogContent.InputsAreValid())
                {
                    string name = dialogContent.AppName;
                    string exepath = dialogContent.ExePath;
                    string iconpath = dialogContent.IconPath;
                    AppShortcut.AppTypes appType = dialogContent.AppType;

                    AppShortcut app = new(name, exepath, iconpath, appType);
                    SaveSystem.SaveApp(app);
                    RefreshAppList();
                }
                else
                {
                    e.Cancel = true; // Prevent dialog from closing
                }

            };


            //User is updating an app
            if (existingApp != null)
            {
                dialogContent.UpdateApp(existingApp);
                dialog.Title = "Update app details";
                dialog.PrimaryButtonClick += (s, e) =>
                {
                    if (dialogContent.InputsAreValid())
                    {
                        string name = dialogContent.AppName;
                        string exepath = dialogContent.ExePath;
                        string iconpath = dialogContent.IconPath;
                        AppShortcut.AppTypes appType = dialogContent.AppType;

                        List<AppShortcut> existingApps = SaveSystem.LoadApps();
                        foreach (AppShortcut appToCheck in existingApps)
                        {
                            if (appToCheck.ID == existingApp.ID)
                            {
                                existingApps.Remove(appToCheck);
                                break;
                            }
                        }
                        AppShortcut app = new(name, exepath, iconpath, appType)
                        {
                            Position = existingApp.Position,
                            ID = existingApp.ID
                        };
                        existingApps.Add(app);
                        SaveSystem.SaveApps(existingApps);

                        RefreshAppList();
                    }
                    else
                    {
                        e.Cancel = true; // Prevent dialog from closing
                    }
                };
            }

            await dialog.ShowAsync();
        }


        private async Task ShowInput()
        {
            AppInputDialog inputDialog = new AppInputDialog();
            ContentDialog selectionDialog = new()
            {
                Title = "Add a new app",
                XamlRoot = (Application.Current as App)?.Window.Content.XamlRoot,
                PrimaryButtonText = "Add",
                SecondaryButtonText = "Cancel",
                Content = inputDialog
            };
            selectionDialog.PrimaryButtonClick += (s, e) =>
            {
                if (inputDialog.ValidInputs())
                {
                    inputDialog.Save();
                    RefreshAppList();
                }
                else
                {
                    e.Cancel = true;
                }
            };


            ContentDialogResult result = await selectionDialog.ShowAsync();
        }

    }
}
