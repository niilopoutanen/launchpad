using LaunchPadConfigurator.Views.Elements;
using LaunchPadCore;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace LaunchPadConfigurator.Views.Pages
{
    public sealed partial class WidgetsPage : Page
    {
        List<Widget> widgets = new();
        public WidgetsPage()
        {
            this.InitializeComponent();
            widgets = SaveSystem.LoadWidgets();
            foreach (Widget widget in widgets)
            {
                WidgetGalleryItem listItem = new(widget);
                WidgetContainer.Children.Add(listItem);
            }
        }
    }
}
