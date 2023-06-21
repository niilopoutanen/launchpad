using LaunchPadConfigurator;
using LaunchPadCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPad
{
    public partial class Icon : System.Windows.Controls.UserControl, ILaunchPadItem
    {
        public AppShortcut App { get; set; }
        public bool Pressed { get; set; }
        public bool Focused { get; set; }


        private Action closeHandler;
        private UserPreferences preferences;
        public Icon(AppShortcut app, Action handler)
        {
            this.App = app;
            this.closeHandler = handler;
            preferences = SaveSystem.LoadPreferences();
            InitializeComponent();

            appIcon.MouseLeftButtonDown += (s, e) =>
            {
                OnPress();
            };
            appIcon.MouseLeftButtonUp += (s, e) =>
            {
                OnRelease();
            };
            appIcon.MouseEnter += (s, e) =>
            {
                OnFocusEnter();
            };
            appIcon.MouseLeave += (s, e) =>
            {
                OnFocusLeave();
            };
            InitializeIcon();

        }


        private void InitializeIcon()
        {
            iconBitmap.Source = AppShortcut.GetIcon(App);


            if (preferences.FullSizeIcon)
            {
                appIcon.Padding = new Thickness(0);
                appIcon.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
            }

            if (preferences.NameVisible)
            {
                appName.Visibility = Visibility.Visible;
                appIcon.Width = 70;
                appIcon.Height = 70;


                if (App.Name.Length > 13)
                {
                    appName.Text = App.Name[..13] + "..";
                }

                else
                {
                    appName.Text = App.Name;
                }
            }


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
            appIcon.RenderTransform = scaleTransform;

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
            appIcon.RenderTransform = scaleTransform;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public async Task OnClick(Action closeHandler)
        {
            await Task.Run(() =>
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = App.ExeUri;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                catch (Exception)
                {
                    if (System.Windows.Application.Current is App hostApp)
                    {
                        hostApp.DisplayMessage("Error", "LaunchPad could not open the app you selected. Verify that the app/file exists.", ToolTipIcon.Error);
                    }
                }
            });
            closeHandler.Invoke();

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

            appIcon.RenderTransform = scaleTransform;

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
            var scaleAnimation = scaleTransformAndAnimation.Item2;

            appIcon.RenderTransform = scaleTransform;

            bool animationCompleted = false;
            scaleAnimation.Completed += async (s, e) =>
            {
                if (!animationCompleted)
                {
                    animationCompleted = true;
                    await OnClick(closeHandler);
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
            if (!preferences.FullSizeIcon)
            {
                appIcon.Background = itemBackgroundColor;
            }
            appName.Foreground = textColor;

        }
    }
}
