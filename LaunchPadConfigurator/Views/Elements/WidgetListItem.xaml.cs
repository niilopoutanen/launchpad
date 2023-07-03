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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
            WidgetIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/Widgets/" + Widget.IconFile));

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
    }
}
