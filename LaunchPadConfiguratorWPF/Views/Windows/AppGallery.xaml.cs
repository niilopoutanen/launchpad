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
            Dictionary<string[], string> data = await DataManager.GetData();
            Dictionary<string, string> localApps = DataManager.GetApps();
            foreach (var keyValuePair in data)
            {
                foreach (string key in keyValuePair.Key)
                {
                    if(DataManager.DoesAppExist(localApps, key))
                    {
                        var tempKey = key;
                        if (key.StartsWith(DataManager.PATTERN_FILE))
                        {
                            tempKey = tempKey[3..];
                            foreach(string s in localApps.Keys)
                            {
                                if (s.Contains(tempKey))
                                {
                                    tempKey = s;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        AppIconControl control = new()
                        {
                            Name = localApps[tempKey],
                            Foreground = new BitmapImage(new Uri(Path.Combine(SaveSystem.predefinedIconsDirectory, keyValuePair.Value)))
                        };
                        AppContainer.Children.Add(control);
                    }
                }
            }
        }
    }
}
