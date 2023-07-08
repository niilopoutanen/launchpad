using LaunchPadCore;
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
using System.Collections.ObjectModel;
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
        public AppShortcut Input { get; set; }
        private Action<string, string, string> dataChanged;
        public StoreAppInput(Action<string,string,string> dataChanged)
        {
            this.dataChanged = dataChanged;
            this.InitializeComponent();
            LoadStoreApps();
        }
        private void LoadStoreApps()
        {
            PackageManager packageManager = new PackageManager();
            IEnumerable<Package> rawData = packageManager.FindPackagesForUser("");

            foreach (Package package in rawData)
            {
                if(package == null) continue;
                try
                {
                    //checking for invalid values
                    var logo = package.Logo;
                    var name = package.DisplayName;
                }
                catch { }

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
            dataChanged.Invoke(Input.Name, Input.ExeUri, Input.IconFileName);
        }
    }
}
