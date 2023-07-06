using LaunchPad.Apps;
using LaunchPadCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Windows.UI.ViewManagement;
using static LaunchPadCore.UserPreferences;

namespace LaunchPad
{
    public partial class LaunchPadWindow : Window
    {
        readonly UserPreferences preferences;
        readonly List<LaunchPadItemControl> items = new();
        public LaunchPadWindow()
        {
            preferences = SaveSystem.LoadPreferences();
            ResourceDictionary activeTheme = SaveSystem.LoadTheme();
            InitializeComponent();
            LoadApps();

            SetTheme(activeTheme);
            SystemEvents.UserPreferenceChanged += (s, e) =>
            {
                SetTheme(activeTheme);
            };
            HandleKeyboard(this);
        }
        private void Launch(object sender, RoutedEventArgs e)
        {
            switch (preferences.SelectedAnimation)
            {
                case AnimationTypes.Center:
                    Storyboard fade = ((Storyboard)this.FindResource("LaunchPadCenterIn")).Clone();
                    fade.Begin(launchPadRoot);
                    break;

                case AnimationTypes.SlideBottom:
                    PositionWindow(AnimationTypes.SlideBottom);

                    ThicknessAnimation slideAnimation = ((ThicknessAnimation)this.FindResource("LaunchPadSlideIn")).Clone();
                    slideAnimation.From = new Thickness(0, launchPadRoot.ActualHeight, 0, 0);

                    launchPadRoot.BeginAnimation(MarginProperty, slideAnimation);
                    break;

                case AnimationTypes.SlideTop:
                    PositionWindow(AnimationTypes.SlideTop);

                    ThicknessAnimation slideTopAnimation = ((ThicknessAnimation)this.FindResource("LaunchPadSlideIn")).Clone();
                    slideTopAnimation.From = new Thickness(0, -launchPadRoot.ActualHeight, 0, 0);
                    slideTopAnimation.To = new Thickness(0, 20, 0, 0);
                    launchPadRoot.BeginAnimation(MarginProperty, slideTopAnimation);
                    break;

            }

        }

        public void Terminate()
        {
            switch (preferences.SelectedAnimation)
            {
                case AnimationTypes.Center:
                    Storyboard launchPadClose = ((Storyboard)this.FindResource("LaunchPadCenterOut")).Clone();
                    launchPadClose.Completed += (s, e) =>
                    {
                        this.Close();
                    };
                    launchPadClose.Begin(launchPadRoot);
                    break;

                case AnimationTypes.SlideBottom:
                    ThicknessAnimation slideAnimation = ((ThicknessAnimation)this.FindResource("LaunchPadSlideOut")).Clone();
                    slideAnimation.To = new Thickness(0, launchPadRoot.ActualHeight * 1.5, 0, 0);
                    slideAnimation.Completed += (sender, e) =>
                    {
                        this.Close();
                    };

                    launchPadRoot.BeginAnimation(MarginProperty, slideAnimation);

                    break;

                case AnimationTypes.SlideTop:
                    ThicknessAnimation slideTopAnimation = ((ThicknessAnimation)this.FindResource("LaunchPadSlideOut")).Clone();
                    slideTopAnimation.To = new Thickness(0, -(launchPadRoot.ActualHeight * 1.5), 0, 0);
                    slideTopAnimation.From = new Thickness(0, 20, 0, 0);
                    slideTopAnimation.Completed += (sender, e) =>
                    {
                        this.Close();
                    };

                    launchPadRoot.BeginAnimation(MarginProperty, slideTopAnimation);

                    break;
            }
        }
        private void PositionWindow(AnimationTypes type)
        {
            var activeScreen = System.Windows.Forms.Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            var screenWorkingArea = activeScreen.WorkingArea;
            var dpiScaleX = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;
            var dpiScaleY = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M22;


            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = (screenWorkingArea.Left / dpiScaleX) + (((screenWorkingArea.Width / dpiScaleX) - Width) / 2);

            switch (type)
            {
                case AnimationTypes.SlideTop:
                    Top = screenWorkingArea.Top / dpiScaleY;
                    break;
                case AnimationTypes.SlideBottom:
                    Top = ((screenWorkingArea.Bottom - 20) / dpiScaleY) - Height;
                    break;
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void LoadApps()
        {

            List<AppShortcut> apps = SaveSystem.LoadApps();
            List<Widget> widgets = SaveSystem.LoadWidgets();

            int activeWidgetCount = SaveSystem.LoadPreferences().ActiveWidgets.Values.Count(value => value);

            appContainer.MaxWidth = preferences.PreferredWidth;

            if (apps.Count == 0 && activeWidgetCount == 0)
            {
                var suggestion = new Suggestion("No apps added. Open configurator to add some.", "1ebbc395-73dc-4302-b025-469cfa5bc701_g37tm3x42n8em!App");
                items.Add(suggestion);
                appContainer.Children.Add(suggestion);
                return;
            }
            foreach (AppShortcut app in apps)
            {
                var icon = new Icon(app);
                items.Add(icon);
                appContainer.Children.Add(icon);
            }
            foreach (Widget widget in widgets)
            {
                if (!widget.Active)
                {
                    continue;
                }
                switch (widget.ID)
                {
                    case "pwr_01":
                        var powerWidget = new PowerWidget();
                        items.Add(powerWidget);
                        appContainer.Children.Add(powerWidget);
                        break;

                    case "btr_02":
                        var batteryWidget = new BatteryWidget(widget);
                        items.Add(batteryWidget);
                        appContainer.Children.Add(batteryWidget);
                        break;
                    case "dt_03":
                        var dateWidget = new DateWidget();
                        items.Add(dateWidget);
                        appContainer.Children.Add(dateWidget);
                        break;
                    case "clk_04":
                        var clockWidget = new ClockWidget();
                        items.Add(clockWidget);
                        appContainer.Children.Add(clockWidget);
                        break;
                    case "plbk_05":
                        var playbackWidget = new PlaybackWidget();
                        items.Add(playbackWidget);
                        appContainer.Children.Add(playbackWidget);
                        break;
                }
            }

        }



        private void SetTheme(ResourceDictionary resourceDictionary)
        {
            foreach (LaunchPadItemControl item in appContainer.Children)
            {
                item.SetTheme(resourceDictionary);
            }


            if (preferences.UseSystemAccent)
            {
                var accentColor = new UISettings().GetColorValue(UIColorType.Accent);
                SolidColorBrush accentBrush = new(Color.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B));
                if (preferences.TransparentTheme)
                {
                    byte opacity = (byte)(accentColor.A * 0.4);
                    accentBrush.Color = Color.FromArgb(opacity, accentBrush.Color.R, accentBrush.Color.G, accentBrush.Color.B);
                }
                launchPadRoot.Background = accentBrush;

                return;
            }



            SolidColorBrush backgroundColor = resourceDictionary["LaunchPadBackground"] as SolidColorBrush;

            launchPadRoot.Background = backgroundColor;

        }

        private void HandleKeyboard(Window window)
        {
            window.KeyDown += (s, e) =>
            {
                int keyNumber = IsNumberKey(e.Key);

                if (keyNumber >= 0 && keyNumber < items.Count)
                {
                    items[keyNumber].OnPress();
                }
            };
            window.KeyUp += async (s, e) =>
            {
                int keyNumber = IsNumberKey(e.Key);

                if (keyNumber >= 0 && keyNumber < items.Count)
                {
                    await items[keyNumber].OnRelease(true);
                }
            };
        }

        private static int IsNumberKey(Key keyPressed)
        {
            bool isNumberKey = (keyPressed >= Key.D1 && keyPressed <= Key.D9) || (keyPressed >= Key.NumPad1 && keyPressed <= Key.NumPad9);
            if (isNumberKey)
            {
                return keyPressed - Key.D1;
            }
            return -1;
        }

    }
}
