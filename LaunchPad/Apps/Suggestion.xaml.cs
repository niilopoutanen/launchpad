using LaunchPadCore;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPad.Apps
{

    public partial class Suggestion : LaunchPadItem
    {
        public override AppShortcut App { get; set; }
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }

        public override UIElement BaseElement { get => Container; }

        public Suggestion(string text)
        {
            InitializeComponent();
            SuggestionText.Text = text;

            base.InitializeControl();
        }

        public override Task OnClick()
        {
            return Task.Delay(1);
        }


        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            SolidColorBrush itemBackgroundColor = activeDictionary["LaunchPadItemBackground"] as SolidColorBrush;
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;


            if (itemBackgroundColor == null || textColor == null)
            {
                return;
            }
            SuggestionText.Foreground = textColor;
        }
    }
}
