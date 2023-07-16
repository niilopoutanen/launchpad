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

namespace LaunchPadCore.Common.Controls
{
    public partial class AppIconControl : UserControl
    {
        public new string Name 
        {
            get { return VisualName.Text; }
            set { VisualName.Text = value; }
        }
        public bool NameVisible
        {
            get => VisualName.Visibility != Visibility.Collapsed;
            set => VisualName.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }
        public new ImageSource Foreground
        {
            get { return ForegroundImage.Source; }
            set { ForegroundImage.Source = value; }
        }
        public EventHandler? OnRelease;
        public EventHandler? OnPress;

        public AppIconControl()
        {
            InitializeComponent();
            Base.MouseLeftButtonDown += (s, e) =>
            {
                OnPress?.Invoke(this, EventArgs.Empty);
            };
            Base.MouseLeftButtonUp += (s, e) =>
            {
                OnRelease?.Invoke(this, EventArgs.Empty);
            };
        }
        public void SetBackground(ImageSource image)
        {
            BackgroundImage.Source = image;
        }
        public void SetBackground(SolidColorBrush color)
        {
            Container.Background = color;
        }
    }
}
