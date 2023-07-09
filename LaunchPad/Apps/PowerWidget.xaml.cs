using LaunchPadCore.Controls;
using LaunchPadCore.Models;
using System;
using System.Diagnostics;
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
        public override bool HasSecondaryAction => false;

        public override FrameworkElement BaseElement => Container;
        public override TextBlock ItemName => VisualName;
        public override UserPreferences Preferences { get; set; }

        public override Widget Widget { get; set; }
        public override int Variation { get; set; }

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
            base.SetTheme(activeDictionary);
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;
            if (!Preferences.ThemedWidgets)
            {
                PowerConfirmation.Foreground = textColor;
            }
        }
    }
}
