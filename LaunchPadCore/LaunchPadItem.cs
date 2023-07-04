using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPadCore
{
    public abstract class LaunchPadItem : System.Windows.Controls.UserControl
    {
        public const float SIZE_FOCUS = 1.05f;
        public const float SIZE_STATIC = 1f;
        public const float SIZE_PRESSED = 0.9f;

        public abstract bool Pressed { get; set; }
        public abstract bool Focused { get; set; }
        public abstract bool WaitForAnim { get; }

        public abstract UIElement BaseElement { get; }


        public void InitializeControl()
        {
            BaseElement.MouseLeftButtonDown += (s, e) =>
            {
                OnPress();
            };
            BaseElement.MouseLeftButtonUp += (s, e) =>
            {
                OnRelease();
            };
            BaseElement.MouseEnter += (s, e) =>
            {
                OnFocusEnter();
            };
            BaseElement.MouseLeave += (s, e) =>
            {
                OnFocusLeave();
            };
        }


        public virtual void OnFocusEnter()
        {
            if (Pressed || Focused)
            {
                return;
            }
            Focused = true;

            ScaleTransform scaleTransform = new(SIZE_STATIC, SIZE_STATIC);

            DoubleAnimation scaleAnimation = new()
            {
                To = SIZE_FOCUS,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            BaseElement.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
        public virtual void OnFocusLeave()
        {
            if (!Focused || Pressed)
            {
                return;
            }
            Focused = false;

            ScaleTransform scaleTransform = new(SIZE_FOCUS, SIZE_FOCUS);

            DoubleAnimation scaleAnimation = new()
            {
                To = SIZE_STATIC,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            BaseElement.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public virtual void OnPress()
        {
            if (Pressed)
            {
                return;
            }
            Pressed = true;

            ScaleTransform scaleTransform = new(SIZE_STATIC, SIZE_STATIC);

            DoubleAnimation scaleAnimation = new()
            {
                To = SIZE_PRESSED,
                Duration = TimeSpan.FromSeconds(0.05)
            };


            BaseElement.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
        public virtual async void OnRelease()
        {
            if (!Pressed)
            {
                return;
            }

            Pressed = false;

            float finalValue = SIZE_STATIC;
            if (Focused)
            {
                finalValue = SIZE_FOCUS;
            }
            ScaleTransform scaleTransform = new(SIZE_PRESSED, SIZE_PRESSED);

            DoubleAnimationUsingKeyFrames scaleAnimation = new()
            {
                Duration = TimeSpan.FromSeconds(0.5)
            };

            scaleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(SIZE_PRESSED, TimeSpan.Zero));

            scaleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(finalValue * 1.05, TimeSpan.FromSeconds(0.1)));
            scaleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(finalValue, TimeSpan.FromSeconds(0.25)));

            BaseElement.RenderTransform = scaleTransform;

            if (WaitForAnim)
            {
                bool animationCompleted = false;
                scaleAnimation.Completed += async (s, e) =>
                {
                    if (!animationCompleted)
                    {
                        animationCompleted = true;
                        await OnClick();
                    }
                };
            }
            else
            {
                await OnClick();
            }

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public abstract Task OnClick();

        public abstract void SetTheme(ResourceDictionary activeDictionary);

    }
}
