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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPadCoreControls
{
    public partial class ToggleSwitch : UserControl
    {
        public bool IsChecked { get; set; }
        public EventHandler<bool> Checked;

        public ToggleSwitch()
        {
            InitializeComponent();
            InitializeControl();
        }
        private void InitializeControl()
        {
            Container.MouseLeftButtonUp += (s, e) =>
            {
                SwitchState();
            };
        }
        private void SwitchState()
        {
            ResourceDictionary resourceDict = this.Resources;

            Style toggleInactive = resourceDict["ToggleInactive"] as Style;
            Style toggleActive = resourceDict["ToggleActive"] as Style;

            if (IsChecked)
            {
                Container.Style = toggleInactive;
                Content.Text = "Off";
                IsChecked = false;
            }
            else if (!IsChecked)
            {
                Container.Style = toggleActive;
                Content.Text = "On";
                IsChecked = true;
            }

            Checked?.Invoke(this, IsChecked);
        }
    }
}
