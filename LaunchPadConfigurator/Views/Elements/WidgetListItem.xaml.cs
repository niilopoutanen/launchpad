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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaunchPadConfigurator.Views.Elements
{
    public sealed partial class WidgetListItem : UserControl
    {
        private Widget Widget { get; set; }
        public WidgetListItem(Widget widget)
        {
            this.InitializeComponent();
            this.Widget = widget;

            InitializeElement();
        }
        private void InitializeElement()
        {
            if (this.Widget == null) 
            {
                return;
            }

            WidgetName.Text = Widget.WidgetName;
            WidgetDescription.Text = Widget.Description;
            WidgetActive.Checked += (s, e) =>
            {
                Widget.Active = true;
                WidgetActive.Content = "Active";
                SaveSystem.SaveWidget(Widget);
            };
            WidgetActive.Unchecked += (s, e) =>
            {
                Widget.Active = false;
                WidgetActive.Content = "Inactive";
                SaveSystem.SaveWidget(Widget);
            };
            WidgetActive.IsChecked = Widget.Active;
        }

        private void WidgetActive_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
