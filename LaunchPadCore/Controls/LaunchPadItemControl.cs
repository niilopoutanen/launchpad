﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LaunchPadCore.Models;

namespace LaunchPadCore.Controls
{
    public abstract class LaunchPadItemControl : UserControl
    {
        public const float SIZE_FOCUS = 1.05f;
        public const float SIZE_STATIC = 1f;
        public const float SIZE_PRESSED = 0.9f;

        public virtual bool Pressed { get; set; }
        public virtual bool Focused { get; set; }
        public abstract bool WaitForAnim { get; }
        public virtual bool HasSecondaryAction { get; }

        public virtual FrameworkElement BaseElement { get; }
        public virtual TextBlock ItemName { get; }
        public virtual UserPreferences Preferences { get; set; }

        public virtual void InitializeControl()
        {
            Preferences = UserPreferences.Load();

            BaseElement.MouseLeftButtonDown += (s, e) =>
            {
                OnPress();
            };
            BaseElement.MouseLeftButtonUp += async (s, e) =>
            {
                await OnRelease(true);
            };
            if (HasSecondaryAction)
            {
                BaseElement.MouseRightButtonDown += (s, e) =>
                {
                    OnPress();
                };
                BaseElement.MouseRightButtonUp += async (s, e) =>
                {
                    await OnRelease(false);
                };
            }


            BaseElement.MouseEnter += (s, e) =>
            {
                OnFocusEnter();
            };
            BaseElement.MouseLeave += (s, e) =>
            {
                OnFocusLeave();
            };

            if (Preferences.NameVisible)
            {
                ItemName.Visibility = Visibility.Visible;
                BaseElement.Width = 80;
                BaseElement.Height = 80;
            }
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
        public virtual async Task OnRelease(bool isPrimary)
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
                        if (isPrimary)
                        {
                            await OnClick();
                        }
                        else
                        {
                            await OnSecondaryClick();
                        }
                    }
                };
            }
            else
            {
                if (isPrimary)
                {
                    await OnClick();
                }
                else
                {
                    await OnSecondaryClick();
                }
            }

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public virtual Task OnClick()
        {
            return Task.CompletedTask;
        }
        public virtual Task OnSecondaryClick()
        {
            return Task.CompletedTask;
        }
        public virtual void SetTheme(ResourceDictionary activeDictionary)
        {

        }

    }
}
