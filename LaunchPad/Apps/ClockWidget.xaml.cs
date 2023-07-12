using LaunchPadCore.Controls;
using LaunchPadCore.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace LaunchPad.Apps
{
    public partial class ClockWidget : LaunchPadWidgetControl
    {
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => false;
        public override bool HasSecondaryAction => true;

        public override FrameworkElement BaseElement => Container;
        public override TextBlock ItemName => VisualName;
        public override UserPreferences Preferences { get; set; }

        public override Widget Widget { get; set; }
        public override int Variation { get; set; }

        private readonly DispatcherTimer clock;
        public ClockWidget(Widget widget)
        {
            this.Widget = widget;
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

            Variation2.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            base.SetTheme(activeDictionary);
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;
            if (!Preferences.ThemedWidgets)
            {
                Variation2.Foreground = textColor;
            }
        }
    }
}
