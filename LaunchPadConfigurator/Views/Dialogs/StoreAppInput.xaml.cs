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
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Management.Deployment;

namespace LaunchPadConfigurator.Views.Dialogs
{
    public sealed partial class StoreAppInput : UserControl
    {
        List<Package> storeApps = new();

        public StoreAppInput()
        {
            this.InitializeComponent();
            LoadStoreApps();
        }
        private void LoadStoreApps()
        {
            PackageManager packageManager = new PackageManager();
            IEnumerable<Package> rawData = packageManager.FindPackagesForUser("");
            foreach (Package package in rawData)
            {
                if(!package.IsFramework && !package.IsResourcePackage && !package.IsBundle)
                {
                    storeApps.Add(package);
                }
            }
            AppListView.ItemsSource = storeApps;
        }
    }
}
