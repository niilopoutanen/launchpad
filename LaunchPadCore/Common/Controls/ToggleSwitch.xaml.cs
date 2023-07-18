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

namespace LaunchPadCore.Controls
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
        private string content;
        public string? Content 
        {
            get 
            {
                if(String.IsNullOrEmpty(content))
                {
                    return null;
                }
                else
                {
                    return content;
                }
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    content = value;
                    Text.Text = content ?? "Off";
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
            Text.Text = Content ?? "Off";
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
                Text.Text = Content ?? "On";


                isChecked = true;
            }
            else if (!isChecked)
            {
                Container.Style = toggleInactive;
                Text.Text = Content ?? "Off";

                isChecked = false;
            }
            Checked?.Invoke(this, isChecked);
        }
    }
}
