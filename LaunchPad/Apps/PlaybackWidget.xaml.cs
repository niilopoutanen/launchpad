using LaunchPadCore;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Windows.Media;
using Windows.Media.Playback;

namespace LaunchPad.Apps
{
    public partial class PlaybackWidget : LaunchPadWidgetControl
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int KEYEVENTF_KEYUP = 0x2;

        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => false;
        public override bool HasSecondaryAction => false;

        public override FrameworkElement BaseElement => Container;
        public override TextBlock ItemName => VisualName;
        public override UserPreferences Preferences { get; set; }

        public override Widget Widget { get; set; }
        public override int Variation { get; set; }

        public PlaybackWidget(Widget widget)
        {
            this.Widget = widget;
            InitializeComponent();
            base.InitializeControl();
        }
        public override Task OnClick()
        {
            TogglePlayBack();
            return Task.CompletedTask;
        }

        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            base.SetTheme(activeDictionary);
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;
            if (!Preferences.ThemedWidgets)
            {
                PlaybackButton.Fill = textColor;
            }
            
        }


        static void TogglePlayBack()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public override void SetVariation(int variation) { }
    }
}
