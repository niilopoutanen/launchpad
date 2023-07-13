using LaunchPadCore.Models;
using LaunchPadCore.Utility;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LaunchPadConfigurator.Views.Dialogs
{
    public sealed partial class AppListItem : UserControl
    {
        public AppShortcut App { get; set; }
        public Widget Widget { get; set; }

        private readonly Action updateHandler;

        public AppListItem(AppShortcut app, Action updateHandler, Func<AppShortcut, Task> editCallBack)
        {
            this.InitializeComponent();
            this.App = app;
            Name.Text = App.Name;
            if (Uri.TryCreate(app.GetIconFullPath(), UriKind.Absolute, out Uri validUri))
            {
                BitmapImage bitmapImage = new(validUri);
                Icon.Source = bitmapImage;
            }

            this.updateHandler = updateHandler;
            editButton.Click += (s, e) =>
            {
                editCallBack(App);
            };


            posUp.Click += (s, e) =>
            {
                App.IncreasePos();
                updateHandler.Invoke();
            };
            posDown.Click += (s, e) =>
            {
                App.DecreasePos();
                updateHandler.Invoke();
            };
        }
        public AppListItem(Widget widget, Action updateHandler)
        {
            this.InitializeComponent();
            this.Widget = widget;
            this.updateHandler = updateHandler;
            Name.Text = Widget.WidgetName;
            Icon.Source = new BitmapImage(new Uri("ms-appx:///Assets/Widgets/" + Widget.IconFile));


            editButton.Visibility = Visibility.Collapsed;
            posDown.Visibility = Visibility.Collapsed;
            posUp.Visibility = Visibility.Collapsed;
        }
        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (App != null)
            {
                List<AppShortcut> existingApps = SaveSystem.LoadApps();

                int indexToRemove = existingApps.FindIndex(app => app.ID == App.ID);

                if (indexToRemove != -1)
                {
                    existingApps.RemoveAt(indexToRemove);

                    for (int i = indexToRemove; i < existingApps.Count; i++)
                    {
                        existingApps[i].Position--;
                    }

                    SaveSystem.SaveApps(existingApps);
                    updateHandler.Invoke();
                    SaveSystem.DeleteUnusedIcons();
                }
            }
            else if (Widget != null)
            {
                List<Widget> widgets = Widget.LoadWidgets();

                int indexToRemove = widgets.FindIndex(widget => widget.ID == Widget.ID);

                if (indexToRemove != -1)
                {
                    UserPreferences preferences = UserPreferences.Load();
                    preferences.ActiveWidgets[Widget.ID] = false;

                    preferences.Save();
                    updateHandler.Invoke();
                }
            }
        }


    }

}
