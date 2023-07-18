using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LaunchPadCore.Models;

namespace LaunchPadCore.Controls
{
    public abstract class LaunchPadWidgetControl : LaunchPadItemControl
    {
        public override bool WaitForAnim => false;
        public abstract Widget Widget { get; set; }
        public abstract int Variation { get; set; }

        public override void InitializeControl()
        {
            base.InitializeControl();
            Variation = Widget.LoadSelectedVariation();
            SetVariation(Variation, true);
        }
        public override Task OnSecondaryClick()
        {
            Variation++;
            Variation = Widget.SwapWidgetVariation(Variation);
            SetVariation(Variation, false);
            return base.OnSecondaryClick();
        }
        public virtual void SetVariation(int variation, bool animationDisabled)
        {
            if (!HasSecondaryAction)
            {
                return;
            }
            for (int i = 1; i <= Widget.VariationCount; i++)
            {
                object element = BaseElement.FindName("Variation" + i);
                ((FrameworkElement)element).Visibility = Visibility.Collapsed;
            }
            object variationObject = BaseElement.FindName("Variation" + variation);

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
            ((FrameworkElement)variationObject).Visibility = Visibility.Visible;
            ((FrameworkElement)variationObject).BeginAnimation(OpacityProperty, fadeInAnimation);

            Variation = variation;
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
                ((Border)BaseElement).Background = itemBackgroundColor;
            }
            ItemName.Foreground = textColor;
        }
    }
}
