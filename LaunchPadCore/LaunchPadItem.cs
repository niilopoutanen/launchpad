using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPadCore
{
    public abstract class LaunchPadItem : System.Windows.Controls.UserControl
    {
        public abstract AppShortcut App { get; set; }
        public abstract bool Pressed { get; set; }
        public abstract bool Focused { get; set; }

        public abstract UIElement BaseElement { get; }
        public abstract Action CloseHander { get; set; }


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
            var scaleTransformAndAnimation = GlobalAppActions.GetFocusEnterAnim();
            ScaleTransform scaleTransform = scaleTransformAndAnimation.Item1;
            DoubleAnimation scaleAnimation = scaleTransformAndAnimation.Item2;
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

            var scaleTransformAndAnimation = GlobalAppActions.GetFocusLeaveAnim();
            ScaleTransform scaleTransform = scaleTransformAndAnimation.Item1;
            DoubleAnimation scaleAnimation = scaleTransformAndAnimation.Item2;
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
            var scaleTransformAndAnimation = GlobalAppActions.GetPressAnim();
            ScaleTransform scaleTransform = scaleTransformAndAnimation.Item1;
            DoubleAnimation scaleAnimation = scaleTransformAndAnimation.Item2;

            BaseElement.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
        public virtual void OnRelease()
        {
            if (!Pressed)
            {
                return;
            }

            Pressed = false;
            var scaleTransformAndAnimation = GlobalAppActions.GetReleaseAnim(Focused);
            ScaleTransform scaleTransform = scaleTransformAndAnimation.Item1;
            var scaleAnimation = scaleTransformAndAnimation.Item2;

            BaseElement.RenderTransform = scaleTransform;

            bool animationCompleted = false;
            scaleAnimation.Completed += async (s, e) =>
            {
                if (!animationCompleted)
                {
                    animationCompleted = true;
                    await OnClick(CloseHander);
                }
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public abstract Task OnClick(Action closeHandler);

        public abstract void SetTheme(ResourceDictionary activeDictionary);

    }
}
