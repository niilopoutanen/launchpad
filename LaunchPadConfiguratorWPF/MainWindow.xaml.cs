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

            HomeTab.MouseLeftButtonUp += (s, e) =>
            {
                ChangeTab(0);
            };
            GeneralTab.MouseLeftButtonUp += (s, e) =>
            {
                ChangeTab(1);
            };
            AppsTab.MouseLeftButtonUp += (s, e) =>
            {
                ChangeTab(2);
            };
            WidgetsTab.MouseLeftButtonUp += (s, e) =>
            {
                ChangeTab(3);
            };
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Width = Math.Max(MinWidth, Width + e.HorizontalChange);
            Height = Math.Max(MinHeight, Height + e.VerticalChange);
        }

        private void ChangeTab(int index)
        {
            HomeTab.Tag = "";
            GeneralTab.Tag = "";
            AppsTab.Tag = "";
            WidgetsTab.Tag = "";
            switch(index)
            {
                case 0:
                    HomeTab.Tag = "active";
                    Header.Text = "Home";
                    break;

                case 1:
                    GeneralTab.Tag = "active";
                    Header.Text = "General";
                    break;

                case 2:
                    AppsTab.Tag = "active";
                    Header.Text = "Apps";
                    break; 
                case 3:
                    WidgetsTab.Tag = "active";
                    Header.Text = "Widgets";
                    break;
            }
        }

    }
}
