using Core.Models;
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

namespace Core
{
    public partial class Icon : UserControl
    {
        private LaunchPadItem item;
        public Icon()
        {
            InitializeComponent();
        }
        public Icon(LaunchPadItem item)
        {
            InitializeComponent();
            this.item = item;
            InitializeIcon();
        }


        private void InitializeIcon()
        {
            if (item == null) return;
        }


        private void Click(object sender, MouseButtonEventArgs e)
        {
            if (item == null) return;
            item.Executable.Launch();
        }



        private new void MouseEnter(object sender, MouseEventArgs e)
        {
            root.RenderTransform = new ScaleTransform(1.1, 1.1, 0.5, 0.5);
        }

        private new void MouseLeave(object sender, MouseEventArgs e)
        {
            root.RenderTransform = new ScaleTransform(1, 1, 0.5, 0.5);
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            root.RenderTransform = new ScaleTransform(0.95, 0.95, 0.5, 0.5);
        }

        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            root.RenderTransform = new ScaleTransform(1.1, 1.1, 0.5, 0.5);
            Click(sender, e);
        }
    }
}
