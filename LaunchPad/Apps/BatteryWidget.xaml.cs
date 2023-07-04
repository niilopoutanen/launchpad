using LaunchPadCore;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPad.Apps
{
    public partial class BatteryWidget : LaunchPadItem
    {
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => false;

        public override UIElement BaseElement => Container;

        public BatteryWidget()
        {
            InitializeComponent();
            base.InitializeControl();

            SetBatteryLevel(LoadBatteryLevel());
            if (SaveSystem.LoadPreferences().NameVisible)
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
            Container.Background = itemBackgroundColor;
            LevelText.Foreground = textColor;
            Name.Foreground = textColor;
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


            double levelWidth = 38 * (batteryLevel / 100.0);
            BatteryLevel.Width = levelWidth;
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
            LevelText.Text = batteryLevel.ToString() + "%";
        }
    }
}
