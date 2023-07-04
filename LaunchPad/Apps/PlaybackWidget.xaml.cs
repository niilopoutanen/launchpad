using LaunchPadCore;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Windows.Media;
using Windows.Media.Playback;

namespace LaunchPad.Apps
{
    public partial class PlaybackWidget : LaunchPadItem
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int KEYEVENTF_KEYUP = 0x2;

        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => false;

        public override UIElement BaseElement => Container;

        public PlaybackWidget()
        {
            InitializeComponent();
            base.InitializeControl();
            if (SaveSystem.LoadPreferences().NameVisible)
            {
                Name.Visibility = Visibility.Visible;
                Container.Width = 80;
                Container.Height = 80;
            }
        }
        public override Task OnClick()
        {
            DoubleAnimation fadeInAnimation = new()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            switch (PlayButton.Visibility)
            {
                case Visibility.Visible:
                    TogglePlayBack();
                    PlayButton.Visibility = Visibility.Collapsed;
                    PauseButton.Visibility = Visibility.Visible;
                    PauseButton.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;

                case Visibility.Collapsed:
                    TogglePlayBack();
                    PauseButton.Visibility = Visibility.Collapsed;
                    PlayButton.Visibility = Visibility.Visible;
                    PlayButton.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    break;
            }

            return Task.CompletedTask;
        }

        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            SolidColorBrush itemBackgroundColor = activeDictionary["LaunchPadItemBackground"] as SolidColorBrush;
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;

            if (itemBackgroundColor == null)
            {
                return;
            }
            Container.Background = itemBackgroundColor;
            Name.Foreground = textColor;
            PlayButton.Fill = textColor;
            PauseButton.Fill = textColor;
        }


        static void TogglePlayBack()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
    }
}
