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
    public partial class DateWidget : LaunchPadWidgetControl
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

        public DateWidget(Widget widget)
        {
            this.Widget = widget;
            InitializeComponent();
            base.InitializeControl();
            SetDate();
        }
        public override Task OnClick()
        {
            Process.Start("explorer.exe", @"shell:Appsfolder\microsoft.windowscommunicationsapps_8wekyb3d8bbwe!microsoft.windowslive.calendar");
            ((App)Application.Current).ToggleLaunchpad();
            return Task.CompletedTask;
        }
        private void SetDate()
        {
            string month = DateTime.Now.Month.ToString("D2");
            string date = DateTime.Now.Day.ToString();

            DateNumber.Text = date;
            MonthNumber.Text = month;

            DateName.Text = DateTime.Today.ToString("ddd");
            MonthName.Text = DateTime.Today.ToString("MMM");
        }
        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            base.SetTheme(activeDictionary);
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;
            if (!Preferences.ThemedWidgets)
            {
                DateNumber.Foreground = textColor;
                DateName.Foreground = textColor;
                MonthNumber.Foreground = textColor;
                MonthName.Foreground = textColor;
            }
        }

        public override void SetVariation(int variation, bool animationDisabled)
        {
            DoubleAnimation fadeInAnimation = new()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };
            if (animationDisabled)
            {
                fadeInAnimation.From = 1.0;
            }
            switch (variation)
            {
                case 1:
                    DatePanel.Visibility = Visibility.Collapsed;
                    MonthPanel.Visibility = Visibility.Visible;
                    MonthPanel.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;
                case 2:
                    MonthPanel.Visibility = Visibility.Collapsed;
                    DatePanel.Visibility = Visibility.Visible;
                    DatePanel.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;
            }
            Variation = variation;
        }
    }
}
