using LaunchPadClassLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPad
{
    public partial class Icon : System.Windows.Controls.UserControl, ILaunchPadItem
    {
        public AppShortcut App { get; set; }
        public bool Pressed { get; set; }
        public bool Focused { get; set; }

        private const float SIZE_FOCUS = 1.05f;
        private const float SIZE_STATIC = 1f;
        private const float SIZE_PRESSED = 0.9f;

        private Action closeHandler;
        public Icon(AppShortcut app, Action handler)
        {
            this.App = app;
            this.closeHandler = handler;
            InitializeComponent();

            iconContainer.MouseLeftButtonDown += (s, e) =>
            {
                OnPress();
            };
            iconContainer.MouseLeftButtonUp += (s, e) =>
            {
                OnRelease();
            };
            iconContainer.MouseEnter += (s, e) =>
            {
                OnFocusEnter();
            };
            iconContainer.MouseLeave += (s, e) =>
            {
                OnFocusLeave();
            };
            InitializeIcon();
            
        }


        /// <summary>
        /// Displays the app icon based on the values provided
        /// </summary>
        /// <param name="iconPath">Path to the app icon</param>
        /// <param name="appURI">If no app icon is present, falling back to executable icon</param>
        private void InitializeIcon()
        {
            iconBitmap.Source = AppShortcut.GetIcon(App);


            if(App.IconSize == AppShortcut.SIZE_FULL)
            {
                iconContainer.Padding = new Thickness(0);
                iconContainer.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0,0,0,0));
            }
        }

        public void OnFocusEnter()
        {
            if (Pressed || Focused)
            {
                return;
            }
            Focused = true;
            ScaleTransform scaleTransform = new ScaleTransform(SIZE_STATIC, SIZE_STATIC);
            iconContainer.RenderTransform = scaleTransform;

            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                To = SIZE_FOCUS,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            // Start the animation
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

            iconContainer.RenderTransform = scaleTransform;
            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                To = SIZE_STATIC,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            // Start the animation
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public void OnClick(Action closeHandler)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = App.ExeUri;
                process.StartInfo.UseShellExecute = true;

                process.Start();
                closeHandler.Invoke();
            }
            catch(Exception)
            {
                System.Windows.MessageBox.Show("LaunchPad could not open the app. Verify that the app exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void OnPress()
        {
            if (Pressed)
            {
                return;
            }
            Pressed = true;
            ScaleTransform scaleTransform = new ScaleTransform(SIZE_STATIC, SIZE_STATIC);

            iconContainer.RenderTransform = scaleTransform;
            DoubleAnimation scaleAnimation = new DoubleAnimation
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
            ScaleTransform scaleTransform = new ScaleTransform(SIZE_PRESSED, SIZE_PRESSED);

            iconContainer.RenderTransform = scaleTransform;
            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                To = finalValue,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            bool animationCompleted = false;

            scaleAnimation.Completed += (s, e) =>
            {
                if (!animationCompleted)
                {
                    animationCompleted = true;
                    OnClick(closeHandler);
                }
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

    }
}
