using LaunchPadCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LaunchPadConfigurator.Views.UIElements
{
    public sealed partial class AppListItem : UserControl
    {
        public AppShortcut App { get; set; }

        private readonly Action updateHandler;

        public AppListItem(AppShortcut app, Action updateHandler, Func<AppShortcut, Task> editCallBack)
        {
            this.InitializeComponent();
            this.App = app;
            appName.Text = App.Name;
            if (Uri.TryCreate(app.GetIconFullPath(), UriKind.Absolute, out Uri validUri))
            {
                BitmapImage bitmapImage = new BitmapImage(validUri);
                appIcon.Source = bitmapImage;
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

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
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


    }

}
