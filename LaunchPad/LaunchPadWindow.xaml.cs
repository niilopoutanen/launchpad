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
using System.Windows.Shapes;

namespace LaunchPad
{
    public partial class LaunchPadWindow : Window
    {
        public LaunchPadWindow()
        {
            InitializeComponent();

            AddApp();
            AddApp();
            AddApp();
            AddApp();
            RemoveExessGap();
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void AddApp()
        {
            var icon = new Icon();
            var gap = new Border();
            gap.Width = 10;
            appContainer.Children.Add(icon);
            appContainer.Children.Add(gap);
        }
        private void RemoveExessGap()
        {
            appContainer.Children.RemoveAt(appContainer.Children.Count  - 1);
        }

    }
}
