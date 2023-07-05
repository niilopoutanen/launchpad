using LaunchPadCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public override FrameworkElement BaseElement => Container;
        public override TextBlock ItemName => VisualName;
        public override UserPreferences Preferences { get; set; }

        private readonly DispatcherTimer clock;

        public ClockWidget()
        {
            InitializeComponent();
            base.InitializeControl();

            clock = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            clock.Tick += Clock_Tick;
            clock.Start();
            Clock_Tick(null, null);
        }
        public override Task OnSecondaryClick()
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
        public override Task OnClick()
        {
            Process.Start("explorer.exe", @"shell:Appsfolder\Microsoft.WindowsAlarms_8wekyb3d8bbwe!App");
            ((App)System.Windows.Application.Current).ToggleLaunchpad();
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
            if (!Preferences.ThemedWidgets)
            {
                Container.Background = itemBackgroundColor;
                TimeText.Foreground = textColor;
            }
            
            VisualName.Foreground = textColor;
        }
    }
}
