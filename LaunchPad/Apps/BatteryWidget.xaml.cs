using LaunchPadCore;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using System.Windows.Forms;

namespace LaunchPad.Apps
{
    public partial class BatteryWidget : LaunchPadItem
    {
        public BatteryWidget()
        {
            InitializeComponent();
            base.InitializeControl();

            SetBatteryLevel(LoadBatteryLevel());
        }
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => false;

        public override UIElement BaseElement => Container;


        public override Task OnClick()
        {
            DoubleAnimation fadeOutAnimation = new()
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            DoubleAnimation fadeInAnimation = new()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            switch (BatteryLevel.Visibility)
            {
                case Visibility.Visible:
                    BatteryLevel.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
                    BatteryLevel.Visibility = Visibility.Collapsed;
                    BatteryCanvas.Visibility = Visibility.Visible;
                    BatteryCanvas.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;

                case Visibility.Collapsed:
                    BatteryCanvas.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
                    BatteryCanvas.Visibility = Visibility.Collapsed;
                    BatteryLevel.Visibility = Visibility.Visible;
                    BatteryLevel.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
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
            Container.Background = itemBackgroundColor;
            BatteryLevel.Foreground = textColor;
        }

        private int LoadBatteryLevel()
        {
            PowerStatus pwr = SystemInformation.PowerStatus;
            float batteryLifePercent = pwr.BatteryLifePercent;
            int batteryLevel = (int)(batteryLifePercent * 100 + 0.5f);
            return Math.Clamp(batteryLevel, 0, 100);
        }
        private void SetBatteryLevel(int batteryLevel)
        {
            if(SystemInformation.PowerStatus.PowerLineStatus == System.Windows.Forms.PowerLineStatus.Online)
            {
                BatteryCharging.Visibility = Visibility.Visible;
            }
            if (batteryLevel < 0)
            {
                BatteryCanvas.Visibility = Visibility.Hidden;
                return;
            }


            double levelWidth = 38 * (batteryLevel / 100.0);
            RectangleGeometry levelGeometry = new RectangleGeometry(new Rect(3, 3, levelWidth, 17), 3, 3);
            Path levelPath = new Path();
            switch (batteryLevel)
            {
                case < 20:
                    levelPath.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    break;
                case < 50:
                    levelPath.Fill = new SolidColorBrush(Color.FromRgb(255, 204, 10));
                    break;
                case < 100:
                    levelPath.Fill = new SolidColorBrush(Color.FromRgb(101, 196, 102));
                    break;
            }

            levelPath.Data = levelGeometry;
            BatteryCanvas.Children.Insert(0,levelPath);
            BatteryLevel.Text = batteryLevel.ToString() + "%";
        }
    }
}
