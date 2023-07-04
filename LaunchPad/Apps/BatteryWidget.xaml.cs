using LaunchPadCore;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LaunchPad.Apps
{
    public partial class BatteryWidget : LaunchPadItem
    {
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => false;

        public override FrameworkElement BaseElement => Container;
        public override TextBlock ItemName => VisualName;
        public override UserPreferences Preferences { get; set; }

        public BatteryWidget()
        {
            InitializeComponent();
            base.InitializeControl();

            SetBatteryLevel(LoadBatteryLevel());
        }

        public override Task OnClick()
        {
            DoubleAnimation fadeInAnimation = new()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            switch (LevelText.Visibility)
            {
                case Visibility.Visible:
                    LevelText.Visibility = Visibility.Collapsed;
                    BatteryCanvas.Visibility = Visibility.Visible;
                    BatteryCanvas.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;

                case Visibility.Collapsed:
                    BatteryCanvas.Visibility = Visibility.Collapsed;
                    LevelText.Visibility = Visibility.Visible;
                    LevelText.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;
            }

            return Task.CompletedTask;
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
                LevelText.Foreground = textColor;
            }
            VisualName.Foreground = textColor;
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
    }
}
