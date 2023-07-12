using LaunchPadCore.Models;
using LaunchPadCore.Utility;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace LaunchPadConfigurator.Views.Elements
{
    public sealed partial class WidgetGalleryItem : UserControl
    {
        private Widget Widget { get; set; }
        public WidgetGalleryItem(Widget widget)
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
