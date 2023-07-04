﻿using LaunchPadCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace LaunchPad.Apps
{
    public partial class PowerWidget : LaunchPadItem
    {
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => true;

        public override UIElement BaseElement => Container;

        public PowerWidget()
        {
            InitializeComponent();
            base.InitializeControl();
            if (SaveSystem.LoadPreferences().NameVisible)
            {
                Name.Visibility = Visibility.Visible;
                Container.Width = 80;
                Container.Height = 80;
            }
        }
        public override Task OnClick()
        {
            var shutdownProcess = new ProcessStartInfo("shutdown", "/s /t 0");
            shutdownProcess.CreateNoWindow = true;
            shutdownProcess.UseShellExecute = false;
            Process.Start(shutdownProcess);
            return Task.CompletedTask;
        }

        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            SolidColorBrush itemBackgroundColor = activeDictionary["LaunchPadItemBackground"] as SolidColorBrush;
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;

            if (itemBackgroundColor == null)
            {
                return;
            }
            Container.Background = itemBackgroundColor;
            Name.Foreground = textColor;
        }
    }
}
