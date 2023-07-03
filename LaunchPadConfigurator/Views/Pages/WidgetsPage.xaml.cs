using LaunchPadConfigurator.Views.Elements;
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
                WidgetListItem listItem = new(widget);
                WidgetContainer.Children.Add(listItem);
            }
        }
    }
}
