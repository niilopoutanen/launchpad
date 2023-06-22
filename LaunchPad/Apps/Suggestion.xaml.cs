using LaunchPadCore;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPad.Apps
{

    public partial class Suggestion : LaunchPadControl
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
        public override Task OnClick(Action closeHandler)
        {

            return null;
        }

        public override void OnFocusEnter()
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

        public override void OnFocusLeave()
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

        public override void OnPress()
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

        public override void OnRelease()
        {
            if (!Pressed)
            {
                return;
            }

            Pressed = false;
            var scaleTransformAndAnimation = GlobalAppActions.GetReleaseAnim(Focused);
            ScaleTransform scaleTransform = scaleTransformAndAnimation.Item1;
            var scaleAnimation = scaleTransformAndAnimation.Item2;

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

        public override void SetTheme(ResourceDictionary activeDictionary)
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
