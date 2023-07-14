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
        public bool Checked { get; set; }
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
            if (Checked)
            {
                Content.Text = "Off";
                Checked = false;
            }
            else if (!Checked)
            {
                Content.Text = "On";
                Checked = true;
            }
        }
    }
}
