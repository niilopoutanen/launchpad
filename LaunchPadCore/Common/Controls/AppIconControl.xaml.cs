using System;
using System.Collections.Generic;
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

namespace LaunchPadCore.Common.Controls
{
    public partial class AppIconControl : UserControl
    {
        private const float SIZE_FOCUS = 1.05f;
        private const float SIZE_STATIC = 1f;
        private const float SIZE_PRESSED = 0.9f;

        public new string Name 
        {
            get { return VisualName.Text; }
            set { VisualName.Text = value; }
        }
        public TextBlock NameElement
        {
            get => VisualName;
            set => VisualName = value;
        }
        public new ImageSource Foreground
        {
            get { return ForegroundImage.Source; }
            set { ForegroundImage.Source = value; }
        }
        public bool IsPressed = false;
        public new bool IsFocused = false;
        public EventHandler<bool> Pressed;
        public EventHandler<bool>? Focused;
        public EventHandler? OnClick;

        public AppIconControl()
        {
            InitializeComponent();
            Base.MouseLeftButtonDown += (s, e) =>
            {
                IsPressed = true;
                Pressed?.Invoke(this, IsPressed);
            };
            Base.MouseLeftButtonUp += (s, e) =>
            {
                IsPressed = false;
                Pressed?.Invoke(this, IsPressed);
            };
            Base.MouseEnter += (s, e) =>
            {
                IsFocused = true;
                Focused?.Invoke(this, IsFocused);
            };
            Base.MouseLeave += (s, e) =>
            {
                IsFocused = false;
                Focused?.Invoke(this, IsFocused);
            };

            Pressed += (s, e) => 
            { 
                if (e) OnPress();
                else OnRelease();
            };
            Focused += (s, e) => 
            { 
                if (e) OnFocusEnter();
                else OnFocusLeave();
            };

        }
        public void SetBackground(ImageSource image)
        {
            BackgroundImage.Source = image;
        }
        public void SetBackground(SolidColorBrush color)
        {
            Container.Background = color;
        }

        public virtual void OnFocusEnter()
        {
            ScaleTransform scaleTransform = new(SIZE_STATIC, SIZE_STATIC);

            DoubleAnimation scaleAnimation = new()
            {
                To = SIZE_FOCUS,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            Container.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
        public virtual void OnFocusLeave()
        {
            ScaleTransform scaleTransform = new(SIZE_FOCUS, SIZE_FOCUS);

            DoubleAnimation scaleAnimation = new()
            {
                To = SIZE_STATIC,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            Container.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public virtual void OnPress()
        {
            ScaleTransform scaleTransform = new(SIZE_STATIC, SIZE_STATIC);

            DoubleAnimation scaleAnimation = new()
            {
                To = SIZE_PRESSED,
                Duration = TimeSpan.FromSeconds(0.05)
            };


            Container.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
        public virtual void OnRelease()
        {
            float finalValue = SIZE_STATIC;
            if (IsFocused)
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

            Container.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);

            OnClick?.Invoke(this, EventArgs.Empty);
        }

    }
}
