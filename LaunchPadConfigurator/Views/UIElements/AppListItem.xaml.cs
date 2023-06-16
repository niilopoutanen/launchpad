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
        public string Name { get; }
        public string IconFileName { get; }

        private Action updateHandler;

        public AppListItem(string name, string iconUri, Action updateHandler)
        {
            this.InitializeComponent();
            appName.Text = name;
            if (Uri.TryCreate(iconUri, UriKind.Absolute, out Uri validUri))
            {
                BitmapImage bitmapImage = new BitmapImage(validUri);
                appIcon.Source = bitmapImage;
            }

            Name = name;
            IconFileName = iconUri;

            this.updateHandler = updateHandler;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            List<AppShortcut> existingApps = SaveSystem.LoadApps();

            foreach (AppShortcut app in existingApps)
            {
                if (app.Name == Name && app.IconFileName == IconFileName)
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
