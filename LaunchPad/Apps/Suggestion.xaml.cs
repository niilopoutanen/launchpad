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

        private Action closeHandler;

        public Suggestion(string text, Action closeHandler)
        {
            InitializeComponent();
            InitializeSuggestion();
            SuggestionText.Text = text;
            this.closeHandler = closeHandler;
        }

        private void InitializeSuggestion()
        {
            Container.MouseLeftButtonDown += (s, e) =>
            {
                OnPress();
            };
            Container.MouseLeftButtonUp += (s, e) =>
            {
                OnRelease();
            };
            Container.MouseEnter += (s, e) =>
            {
                OnFocusEnter();
            };
            Container.MouseLeave += (s, e) =>
            {
                OnFocusLeave();
            };
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
            var scaleTransformAndAnimation = GlobalAppActions.GetFocusEnterAnim();
            ScaleTransform scaleTransform = scaleTransformAndAnimation.Item1;
            DoubleAnimation scaleAnimation = scaleTransformAndAnimation.Item2;
            Container.RenderTransform = scaleTransform;

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

            var scaleTransformAndAnimation = GlobalAppActions.GetFocusLeaveAnim();
            ScaleTransform scaleTransform = scaleTransformAndAnimation.Item1;
            DoubleAnimation scaleAnimation = scaleTransformAndAnimation.Item2;
            Container.RenderTransform = scaleTransform;

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
            var scaleTransformAndAnimation = GlobalAppActions.GetPressAnim();
            ScaleTransform scaleTransform = scaleTransformAndAnimation.Item1;
            DoubleAnimation scaleAnimation = scaleTransformAndAnimation.Item2;

            Container.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public void OnRelease()
        {
            if (!Pressed)
            {
                return;
            }

            Pressed = false;
            var scaleTransformAndAnimation = GlobalAppActions.GetReleaseAnim(Focused);
            ScaleTransform scaleTransform = scaleTransformAndAnimation.Item1;
            DoubleAnimation scaleAnimation = scaleTransformAndAnimation.Item2;

            Container.RenderTransform = scaleTransform;

            bool animationCompleted = false;
            scaleAnimation.Completed += async (s, e) =>
            {
                if (!animationCompleted)
                {
                    animationCompleted = true;
                    //await OnClick(closeHandler);
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
