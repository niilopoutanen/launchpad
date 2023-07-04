﻿using LaunchPadCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPad.Apps
{
    public partial class PowerWidget : LaunchPadItem
    {
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => false;

        public override UIElement BaseElement => Container;
        private readonly UserPreferences preferences;
        public PowerWidget()
        {
            InitializeComponent();
            base.InitializeControl();
            preferences = SaveSystem.LoadPreferences();
            if (preferences.NameVisible)
            {
                Name.Visibility = Visibility.Visible;
                Container.Width = 80;
                Container.Height = 80;
            }
        }
        public override Task OnClick()
        {
            DoubleAnimation fadeInAnimation = new()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            switch (PowerButton.Visibility)
            {
                case Visibility.Visible:
                    PowerButton.Visibility = Visibility.Collapsed;
                    PowerConfirmation.Visibility = Visibility.Visible;
                    PowerConfirmation.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;

                case Visibility.Collapsed:
                    var shutdownProcess = new ProcessStartInfo("shutdown", "/s /t 0");
                    shutdownProcess.CreateNoWindow = true;
                    shutdownProcess.UseShellExecute = false;
                    Process.Start(shutdownProcess);
                    break;
            }

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
            if (!preferences.ThemedWidgets)
            {
                Container.Background = itemBackgroundColor;
                PowerConfirmation.Foreground = textColor;
            }
            Name.Foreground = textColor;
        }
    }
}
