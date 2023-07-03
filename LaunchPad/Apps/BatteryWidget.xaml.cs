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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPad.Apps
{
    public partial class BatteryWidget : LaunchPadItem
    {
        public BatteryWidget()
        {
            InitializeComponent();
            base.InitializeControl();

            SetBatteryLevel(70);
        }
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }

        public override UIElement BaseElement => Container;


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

            if (itemBackgroundColor == null)
            {
                return;
            }
            Container.Background = itemBackgroundColor;
        }
        private void SetBatteryLevel(int batteryLevel)
        {
            if (batteryLevel < 0)
            {
                BatteryCanvas.Visibility = Visibility.Hidden;
                return;
            }


            double levelWidth = 25 * (batteryLevel / 100.0);
            RectangleGeometry levelGeometry = new RectangleGeometry(new Rect(3, 3, levelWidth, 10), 3, 3);
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
            BatteryCanvas.Children.Add(levelPath);
            BatteryLevel.Text = batteryLevel.ToString();
        }
    }
}
