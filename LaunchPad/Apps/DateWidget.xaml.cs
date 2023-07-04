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
    public partial class DateWidget : LaunchPadItem
    {
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => false;

        public override UIElement BaseElement => Container;

        public DateWidget()
        {
            InitializeComponent();
            base.InitializeControl();

            SetDate();

            if (SaveSystem.LoadPreferences().NameVisible)
            {
                Name.Visibility = Visibility.Visible;
                Container.Width = 80;
                Container.Height = 80;
            }
        }

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

            switch (DatePanel.Visibility)
            {
                case Visibility.Visible:
                    DatePanel.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
                    DatePanel.Visibility = Visibility.Collapsed;
                    MonthPanel.Visibility = Visibility.Visible;
                    MonthPanel.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;

                case Visibility.Collapsed:
                    MonthPanel.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
                    MonthPanel.Visibility = Visibility.Collapsed;
                    DatePanel.Visibility = Visibility.Visible;
                    DatePanel.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;
            }

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
            SolidColorBrush itemBackgroundColor = activeDictionary["LaunchPadItemBackground"] as SolidColorBrush;
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;

            if (itemBackgroundColor == null || textColor == null)
            {
                return;
            }
            Container.Background = itemBackgroundColor;
            DateNumber.Foreground = textColor;
            DateName.Foreground = textColor;
            MonthNumber.Foreground = textColor;
            MonthName.Foreground = textColor;
            Name.Foreground = textColor;
        }

    }
}
