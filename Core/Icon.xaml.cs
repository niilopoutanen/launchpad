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
using System.Windows.Media.Animation;
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



        private void MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MouseEnterAnimation");
            sb.Begin();
        }

        private void MouseLeave(object sender, MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MouseLeaveAnimation");
            sb.Begin();
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MouseDownAnimation");
            sb.Begin();
        }

        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MouseUpAnimation");
            sb.Completed += (s, ev) =>
            {
                Click(sender, e);
            };
            sb.Begin();

        }
    }
}
