using LaunchPadCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LaunchPad.Apps
{
    public partial class BatteryWidget : LaunchPadWidgetControl
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

        public BatteryWidget(Widget widget)
        {
            this.Widget = widget;
            InitializeComponent();
            base.InitializeControl();

            SetBatteryLevel(LoadBatteryLevel());
        }
        public override Task OnClick()
        {
            Process process = new();
            process.StartInfo.FileName = "ms-settings:batterysaver-settings";
            process.StartInfo.UseShellExecute = true;
            process.Start();
            ((App)System.Windows.Application.Current).ToggleLaunchpad();
            return Task.CompletedTask;
        }

        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            base.SetTheme(activeDictionary);
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;
            if (!Preferences.ThemedWidgets)
            {
                LevelText.Foreground = textColor;
            }
            
        }

        private int LoadBatteryLevel()
        {
            PowerStatus pwr = SystemInformation.PowerStatus;
            float batteryLifePercent = pwr.BatteryLifePercent;
            int batteryLevel = (int)((batteryLifePercent * 100) + 0.5f);
            return Math.Clamp(batteryLevel, 0, 100);
        }
        private void SetBatteryLevel(int batteryLevel)
        {
            if (SystemInformation.PowerStatus.PowerLineStatus == System.Windows.Forms.PowerLineStatus.Online)
            {
                BatteryCharging.Visibility = Visibility.Visible;
            }
            if (batteryLevel < 0)
            {
                BatteryCanvas.Visibility = Visibility.Hidden;
                return;
            }


            double levelWidth = 35.5 * (batteryLevel / 100.0);
            BatteryLevel.Width = levelWidth;
            if (Preferences.ThemedWidgets)
            {
                BatteryLevel.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                switch (batteryLevel)
                {
                    case < 20:
                        BatteryLevel.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        break;
                    case < 50:
                        BatteryLevel.Background = new SolidColorBrush(Color.FromRgb(255, 204, 10));
                        break;
                    case < 101:
                        BatteryLevel.Background = new SolidColorBrush(Color.FromRgb(101, 196, 102));
                        break;
                }
                ((Path)BatteryCharging.Children[0]).StrokeThickness = 0;
            }

            LevelText.Text = batteryLevel.ToString() + "%";
        }

        public override void SetVariation(int variation)
        {
            DoubleAnimation fadeInAnimation = new()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };
            switch (variation)
            {
                case 1:
                    LevelText.Visibility = Visibility.Collapsed;
                    BatteryCanvas.Visibility = Visibility.Visible;
                    BatteryCanvas.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;
                case 2:
                    BatteryCanvas.Visibility = Visibility.Collapsed;
                    LevelText.Visibility = Visibility.Visible;
                    LevelText.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;
            }
            Variation = variation;
        }
    }
}
