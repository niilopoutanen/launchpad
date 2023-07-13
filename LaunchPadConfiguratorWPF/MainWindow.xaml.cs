using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPadConfiguratorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeElements();
        }
        private void InitializeElements()
        {
            AppTitleBar.MouseLeftButtonDown += (s, e) =>
            {
                this.DragMove();
            };
            Close.MouseLeftButtonUp += (s, e) =>
            {
                Application.Current.Shutdown();
            };
            Minimize.MouseLeftButtonUp += (s, e) =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            };
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Width = Math.Max(MinWidth, Width + e.HorizontalChange);
            Height = Math.Max(MinHeight, Height + e.VerticalChange);
        }

    }
}
