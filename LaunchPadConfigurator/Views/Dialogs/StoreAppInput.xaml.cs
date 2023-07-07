using LaunchPadCore;
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
        readonly List<Package> storeApps = new();
        AppShortcut Input { get; set; }
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
        
        public AppShortcut Get()
        {
            return Input;
        }

        private void AppSelected(object sender, SelectionChangedEventArgs e)
        {
            Package selectedPackage = AppListView.SelectedItem as Package;
            Input = new()
            {
                Name = selectedPackage.DisplayName,
                IconFileName = selectedPackage.Logo.AbsolutePath,
                ExeUri = selectedPackage.Id.FamilyName
            };
        }
    }
}
