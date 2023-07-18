using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPadCore.Common.Controls
{
    public class BaseWindow : Window
    {
        public new void Close()
        {
            DoubleAnimation fadeOutAnimation = new(1, 0, TimeSpan.FromSeconds(0.2));

            fadeOutAnimation.Completed += (s, e) => base.Close();
            BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }
    }
}
