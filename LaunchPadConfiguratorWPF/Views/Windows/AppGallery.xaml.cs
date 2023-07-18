using ABI.System.Collections.Generic;
using LaunchPadConfiguratorWPF.Views.Controls;
using LaunchPadCore.Common.Controls;
using LaunchPadCore.Utility;
using System;
using System.Collections.Generic;
using System.IO;
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
            Dictionary<string[], string> predefinedApps = await DataManager.LoadPredefinedApps();
            Dictionary<string, string> localApps = DataManager.LoadInstalledApps();

            await DataManager.ProcessData(predefinedApps);

            List<Tuple<string,string,string>> mergedData = DataManager.MergeData(localApps, predefinedApps);
            foreach(var tuple in mergedData)
            {
                AppIconControl control = new()
                {
                    Name = tuple.Item1,
                    Foreground = new BitmapImage(new Uri(Path.Combine(SaveSystem.predefinedIconsDirectory, tuple.Item2)))
                };
                control.OnClick += (s, e) =>
                {
                    Core.LaunchApp(tuple.Item3);
                };
                control.Container.Margin = new Thickness(0,10,0,10);
                control.Container.Width = 110;
                control.Container.MaxWidth = 110;
                control.Container.MinWidth = 110;
                control.NameElement.TextWrapping = TextWrapping.Wrap;
                AppContainer.Children.Add(control);
            }
        }
    }
}
