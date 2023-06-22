using LaunchPad.Apps;
using LaunchPadConfigurator;
using LaunchPadCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static LaunchPadCore.UserPreferences;

namespace LaunchPad
{
    public partial class LaunchPadWindow : Window
    {
        readonly UserPreferences preferences;
        readonly List<ILaunchPadItem> items = new();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var screenWorkingArea = SystemParameters.WorkArea;

            switch (preferences.SelectedAnimation)
            {
                case AnimationTypes.Center:
                    Storyboard fade = ((Storyboard)this.FindResource("LaunchPadCenterIn")).Clone();
                    fade.Begin(launchPadRoot);
                    break;

                case AnimationTypes.SlideBottom:
                    WindowStartupLocation = WindowStartupLocation.Manual;

                    Left = (screenWorkingArea.Width - Width) / 2;
                    Top = screenWorkingArea.Height - Height;

                    ThicknessAnimation slideAnimation = ((ThicknessAnimation)this.FindResource("LaunchPadSlideIn")).Clone();
                    slideAnimation.From = new Thickness(0, launchPadRoot.ActualHeight, 0, 0);

                    launchPadRoot.BeginAnimation(MarginProperty, slideAnimation);
                    break;

                case AnimationTypes.SlideTop:
                    WindowStartupLocation = WindowStartupLocation.Manual;

                    Left = (screenWorkingArea.Width - Width) / 2;
                    Top = 0;

                    ThicknessAnimation slideTopAnimation = ((ThicknessAnimation)this.FindResource("LaunchPadSlideIn")).Clone();
                    slideTopAnimation.From = new Thickness(0, -launchPadRoot.ActualHeight, 0, 0);

                    launchPadRoot.BeginAnimation(MarginProperty, slideTopAnimation);
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
            appContainer.MaxWidth = preferences.PreferredWidth;

            if (apps.Count == 0)
            {
                var suggestion = new Suggestion("No apps added. Open configurator to add some.", CloseWithAnim);
                items.Add(suggestion);
                appContainer.Children.Add(suggestion);
                return;
            }
            foreach (AppShortcut app in apps)
            {
                var icon = new Icon(app, CloseWithAnim);
                items.Add(icon);
                appContainer.Children.Add(icon);
            }

        }

        public void CloseWithAnim()
        {
            var screenWorkingArea = System.Windows.SystemParameters.WorkArea;

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

                    Left = (screenWorkingArea.Width - Width) / 2;
                    Top = screenWorkingArea.Height - Height;

                    ThicknessAnimation slideAnimation = ((ThicknessAnimation)this.FindResource("LaunchPadSlideOut")).Clone();
                    slideAnimation.To = new Thickness(0, launchPadRoot.ActualHeight + 20, 0, 0);
                    slideAnimation.Completed += (sender, e) =>
                    {
                        this.Close();
                    };

                    launchPadRoot.BeginAnimation(MarginProperty, slideAnimation);

                    break;

                case AnimationTypes.SlideTop:

                    Left = (screenWorkingArea.Width - Width) / 2;
                    Top = 0;

                    ThicknessAnimation slideAnimationTop = ((ThicknessAnimation)this.FindResource("LaunchPadSlideOut")).Clone();
                    slideAnimationTop.To = new Thickness(0, -launchPadRoot.ActualHeight - 120, 0, 0);
                    slideAnimationTop.Completed += (sender, e) =>
                    {
                        this.Close();
                    };

                    launchPadRoot.BeginAnimation(MarginProperty, slideAnimationTop);

                    break;
            }

        }


        private void SetTheme(ResourceDictionary resourceDictionary)
        {
            SolidColorBrush backgroundColor = resourceDictionary["LaunchPadBackground"] as SolidColorBrush;

            launchPadRoot.Background = backgroundColor;
            foreach (ILaunchPadItem item in appContainer.Children)
            {
                item.SetTheme(resourceDictionary);
            }
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
            window.KeyUp += (s, e) =>
            {
                int keyNumber = IsNumberKey(e.Key);

                if (keyNumber >= 0 && keyNumber < items.Count)
                {
                    items[keyNumber].OnRelease();
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
