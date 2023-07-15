using LaunchPadCore.Common.Controls;
using LaunchPadCore.Common.Models;
using LaunchPadCore.Common.Utility;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace LaunchPad.Apps
{

    public partial class Suggestion : LaunchPadItemControl
    {
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => true;
        public override bool HasSecondaryAction => false;

        public override FrameworkElement BaseElement => Container;
        public override TextBlock ItemName => new();
        public override UserPreferences Preferences { get; set; }

        private readonly string appID;
        public Suggestion(string text, string appID)
        {
            InitializeComponent();
            SuggestionText.Text = text;
            this.appID = appID;
            base.InitializeControl();
        }

        public override async Task OnClick()
        {
            await Task.Run(() =>
            {
                try
                {
                    Core.LaunchApp(Core.APP_LAUNCHPADCONFIG);
                }
                catch (Exception)
                {
                    if (System.Windows.Application.Current is App hostApp)
                    {
                        hostApp.DisplayMessage("Error", "Configurator could not be opened. Make sure LaunchPad is installed correctly.", ToolTipIcon.Error);
                    }
                }
            });
            ((App)System.Windows.Application.Current).ToggleLaunchpad();
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
