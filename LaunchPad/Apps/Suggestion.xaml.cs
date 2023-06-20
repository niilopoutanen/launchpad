using LaunchPadCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LaunchPad.Apps
{
    /// <summary>
    /// Interaction logic for Suggestion.xaml
    /// </summary>
    public partial class Suggestion : UserControl, ILaunchPadItem
    {
        public AppShortcut App { get; set; }
        public bool Pressed { get; set; }
        public bool Focused { get; set; }

        private const float SIZE_FOCUS = 1.05f;
        private const float SIZE_STATIC = 1f;
        private const float SIZE_PRESSED = 0.9f;

        private Action closeHandler;

        public Suggestion(string text, Action closeHandler)
        {
            InitializeComponent();
            SuggestionText.Text = text;
            this.closeHandler = closeHandler;
        }


        public Task OnClick(Action closeHandler)
        {

            return null;
        }

        public void OnFocusEnter()
        {
            if (Pressed || Focused)
            {
                return;
            }
            Focused = true;
            ScaleTransform scaleTransform = new ScaleTransform(SIZE_STATIC, SIZE_STATIC);
            Container.RenderTransform = scaleTransform;

            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                To = SIZE_FOCUS,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public void OnFocusLeave()
        {
            if (!Focused || Pressed)
            {
                return;
            }
            Focused = false;

            ScaleTransform scaleTransform = new ScaleTransform(SIZE_FOCUS, SIZE_FOCUS);

            Container.RenderTransform = scaleTransform;
            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                To = SIZE_STATIC,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public void OnPress()
        {
            if (Pressed)
            {
                return;
            }
            Pressed = true;
            ScaleTransform scaleTransform = new ScaleTransform(SIZE_STATIC, SIZE_STATIC);

            Container.RenderTransform = scaleTransform;
            DoubleAnimation scaleAnimation = new()
            {
                To = SIZE_PRESSED,
                Duration = TimeSpan.FromSeconds(0.1)
            };


            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public void OnRelease()
        {
            if (!Pressed)
            {
                return;
            }
            float finalValue = SIZE_STATIC;
            if (Focused)
            {
                finalValue = SIZE_FOCUS;
            }
            Pressed = false;
            ScaleTransform scaleTransform = new(SIZE_PRESSED, SIZE_PRESSED);

            Container.RenderTransform = scaleTransform;
            DoubleAnimation scaleAnimation = new()
            {
                To = finalValue,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            bool animationCompleted = false;

            scaleAnimation.Completed += async (s, e) =>
            {
                if (!animationCompleted)
                {
                    animationCompleted = true;
                }
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public void SetTheme(ResourceDictionary activeDictionary)
        {
            SolidColorBrush itemBackgroundColor = activeDictionary["LaunchPadItemBackground"] as SolidColorBrush;
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;


            if (itemBackgroundColor == null || textColor == null)
            {
                return;
            }
            SuggestionText.Foreground = textColor;
        }
    }
}
