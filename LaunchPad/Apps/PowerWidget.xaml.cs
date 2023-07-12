using LaunchPadCore.Controls;
using LaunchPadCore.Models;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPad.Apps
{
    public partial class PowerWidget : LaunchPadWidgetControl
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

        [DllImport("PowrProf.dll", SetLastError = true)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        public PowerWidget(Widget widget)
        {
            this.Widget = widget;
            InitializeComponent();
            base.InitializeControl();
        }
        public override Task OnClick()
        {
            DoubleAnimation fadeInAnimation = new()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };
            if(ActionConfirmation.Visibility == Visibility.Collapsed)
            {
                Variation1.Visibility = Visibility.Collapsed;
                Variation2.Visibility = Visibility.Collapsed;
                Variation3.Visibility = Visibility.Collapsed;

                ActionConfirmation.Visibility = Visibility.Visible;
                ActionConfirmation.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                return Task.CompletedTask;
            }
            var powerProcess = new ProcessStartInfo("shutdown", "/s /t 0");
            switch (Variation)
            {
                case 1:
                    powerProcess = new ProcessStartInfo("shutdown", "/s /t 0");
                    break;

                case 2:
                    powerProcess = new ProcessStartInfo("shutdown", "/r /t 0");
                    break;

                case 3:
                    SetSuspendState(false, false, false);
                    break;
            }
            powerProcess.CreateNoWindow = true;
            powerProcess.UseShellExecute = false;
            Process.Start(powerProcess);
            ((App)Application.Current).ToggleLaunchpad();
            return Task.CompletedTask;
        }
        public override Task OnSecondaryClick()
        {
            if(ActionConfirmation.Visibility == Visibility.Visible)
            {
                return Task.CompletedTask;
            }
            return base.OnSecondaryClick();
        }

        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            base.SetTheme(activeDictionary);
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;
            if (!Preferences.ThemedWidgets)
            {
                ActionConfirmation.Foreground = textColor;
            }
        }
    }
}
