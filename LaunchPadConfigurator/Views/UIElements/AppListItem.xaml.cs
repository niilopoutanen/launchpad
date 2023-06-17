using LaunchPadClassLibrary;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace LaunchPadConfigurator.Views.UIElements
{
    public sealed partial class AppListItem : UserControl
    {
        public AppShortcut App {  get; set; }

        private readonly Action updateHandler;

        public AppListItem(AppShortcut app, Action updateHandler)
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
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            List<AppShortcut> existingApps = SaveSystem.LoadApps();

            foreach (AppShortcut app in existingApps)
            {
                if (app.Name == App.Name && app.IconFileName == App.IconFileName)
                {
                    existingApps.Remove(app);
                    break;
                }
            }
            SaveSystem.SaveApps(existingApps);
            updateHandler.Invoke();
        }
    }

}
