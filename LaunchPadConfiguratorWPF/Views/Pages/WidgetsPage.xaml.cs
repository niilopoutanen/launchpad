using LaunchPadConfiguratorWPF.Views.Controls;
using LaunchPadCore.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPadConfiguratorWPF.Views.Pages
{
    public partial class WidgetsPage : Page
    {
        readonly List<Widget> widgets = new();

        public WidgetsPage()
        {
            InitializeComponent();
            widgets = Widget.LoadWidgets();
            foreach (Widget widget in widgets)
            {
                WidgetGalleryControl listItem = new(widget);
                WidgetContainer.Children.Add(listItem);
            }
        }
    }
}
