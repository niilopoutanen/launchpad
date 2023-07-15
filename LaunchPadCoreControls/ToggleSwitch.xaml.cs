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
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    SetState();
                }
            }
        }
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
                isChecked = !isChecked;
                SetState();
            };
        }
        private void SetState()
        {
            ResourceDictionary resourceDict = this.Resources;

            Style toggleInactive = resourceDict["ToggleInactive"] as Style;
            Style toggleActive = resourceDict["ToggleActive"] as Style;

            if (isChecked)
            {
                Container.Style = toggleActive;
                Content.Text = "On";
                isChecked = true;
            }
            else if (!isChecked)
            {
                Container.Style = toggleInactive;
                Content.Text = "Off";
                isChecked = false;
            }
            Checked?.Invoke(this, isChecked);
        }
    }
}
