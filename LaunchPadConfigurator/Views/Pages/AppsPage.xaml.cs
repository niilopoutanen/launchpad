using LaunchPadConfigurator.Views.Dialogs;
using LaunchPadCore.Common.Models;
using LaunchPadCore.Common.Utility;
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
            };
        }

        private void RefreshAppList()
        {
            appsList.Children.Clear();
            List<AppShortcut> apps = SaveSystem.LoadApps();

            foreach (AppShortcut app in apps)
            {
                AppListItem listItem = new(app, RefreshAppList, ShowUpdateInput);
                appsList.Children.Add(listItem);
            }

            UserPreferences preferences = UserPreferences.Load();
            List<Widget> widgets = Widget.LoadWidgets();
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

        private async Task ShowInput()
        {
            AppInputDialog inputDialog = new();
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


            await selectionDialog.ShowAsync();
        }
        private async Task ShowUpdateInput(AppShortcut appToUpdate)
        {
            AppInputDialog inputDialog = new(appToUpdate);
            ContentDialog selectionDialog = new()
            {
                Title = "Update the app",
                XamlRoot = (Application.Current as App)?.Window.Content.XamlRoot,
                PrimaryButtonText = "Update",
                SecondaryButtonText = "Cancel",
                Content = inputDialog
            };
            selectionDialog.PrimaryButtonClick += (s, e) =>
            {
                if (inputDialog.ValidInputs())
                {
                    inputDialog.Update();
                    RefreshAppList();
                }
                else
                {
                    e.Cancel = true;
                }
            };


            await selectionDialog.ShowAsync();
        }

    }
}
