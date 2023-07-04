﻿using LaunchPadCore;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace LaunchPad.Apps
{
    public partial class ClockWidget : LaunchPadItem
    {
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => false;

        public override UIElement BaseElement => Container;

        private readonly DispatcherTimer clock;
        private readonly UserPreferences preferences;
        public ClockWidget()
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

            clock = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            clock.Tick += Clock_Tick;
            clock.Start();
            Clock_Tick(null, null);
        }

        public override Task OnClick()
        {
            DoubleAnimation fadeInAnimation = new()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            switch (ClockCanvas.Visibility)
            {
                case Visibility.Visible:
                    ClockCanvas.Visibility = Visibility.Collapsed;
                    TimeText.Visibility = Visibility.Visible;
                    TimeText.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;

                case Visibility.Collapsed:
                    TimeText.Visibility = Visibility.Collapsed;
                    ClockCanvas.Visibility = Visibility.Visible;
                    ClockCanvas.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;
            }
            return Task.CompletedTask;
        }
        private void Clock_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            double hourAngle = (now.Hour % 12 + now.Minute / 60.0) * 30;
            double minuteAngle = now.Minute * 6;
            double secondAngle = now.Second * 6;

            //Turn the handles the opposite way
            hourAngle -= 180;
            minuteAngle -= 180;
            secondAngle -= 180;

            HourHand.RenderTransform = new RotateTransform(hourAngle);
            MinuteHand.RenderTransform = new RotateTransform(minuteAngle);
            SecondHand.RenderTransform = new RotateTransform(secondAngle);

            TimeText.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            SolidColorBrush itemBackgroundColor = activeDictionary["LaunchPadItemBackground"] as SolidColorBrush;
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;

            if (itemBackgroundColor == null || textColor == null)
            {
                return;
            }
            if (!preferences.ThemedWidgets)
            {
                Container.Background = itemBackgroundColor;
                TimeText.Foreground = textColor;
            }
            
            Name.Foreground = textColor;
        }
    }
}
