using LaunchPadCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPad.Apps
{
    public partial class PowerWidget : Widget
    {
        public PowerWidget()
        {
            InitializeComponent();
            base.InitializeControl();
        }
        public override string WidgetName => "PowerWidget";
        public override string Description => "Turns off the computer";
        public override string IconPath => "Resources/Assets/Icons/PowerWidget.png";

        public override bool Active { get; set; }
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }

        public override UIElement BaseElement => Container;


        public override Task OnClick()
        {
            var shutdownProcess = new ProcessStartInfo("shutdown", "/s /t 0");
            shutdownProcess.CreateNoWindow = true;
            shutdownProcess.UseShellExecute = false;
            Process.Start(shutdownProcess);
            return Task.CompletedTask;
        }

        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            SolidColorBrush itemBackgroundColor = activeDictionary["LaunchPadItemBackground"] as SolidColorBrush;

            if (itemBackgroundColor == null)
            {
                return;
            }
            Container.Background = itemBackgroundColor;
        }
    }
}
