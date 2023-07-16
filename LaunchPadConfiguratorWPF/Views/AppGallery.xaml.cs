using LaunchPadConfiguratorWPF.Views.Controls;
using LaunchPadCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LaunchPadConfiguratorWPF.Views
{
    public partial class AppGallery : Window
    {
        public AppGallery()
        {
            InitializeComponent();
            Container.MouseLeftButtonDown += (s, e) =>
            {
                this.DragMove();
            };

            LoadApps();
        }
        private async void LoadApps()
        {
            Dictionary<string, string> data = await DataManager.GetData();
            Dictionary<string, string> localApps = DataManager.GetApps();
            foreach (var keyValuePair in data)
            {
                if (localApps.ContainsKey(keyValuePair.Key))
                {
                    AppGalleryControl control = new(keyValuePair.Value, localApps[keyValuePair.Key]);
                    AppContainer.Children.Add(control);
                }
            }
        }

    }
}
