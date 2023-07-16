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
using System.Windows.Navigation;

namespace LaunchPadConfiguratorWPF.Views.Controls
{
    public partial class AppGalleryControl : UserControl
    {
        public AppGalleryControl(string icon, string name)
        {
            InitializeComponent();
            Uri iconPath = new Uri(Path.Combine(SaveSystem.predefinedIconsDirectory, icon));
            Icon.Source = new BitmapImage(iconPath);
            Name.Text = name;
        }
    }
}
