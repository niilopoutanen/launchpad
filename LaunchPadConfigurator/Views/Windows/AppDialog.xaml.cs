using LaunchPadCore.Models;
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

namespace LaunchPadConfigurator.Views.Windows
{
    public partial class AppDialog : Window
    {
        public AppDialog()
        {
            InitializeComponent();
            Cancel.Click += (s, e) =>
            {
                this.Close();
            };
            this.MouseLeftButtonDown += (s, e) =>
            {
                this.DragMove();
            };
        }
        public void InitializeInput(AppShortcut.AppTypes type)
        {
            if(type == AppShortcut.AppTypes.EXE)
            {
                LocalFileInput.Visibility = Visibility.Visible;
                UrlInput.Visibility = Visibility.Collapsed;
            }
            else if (type == AppShortcut.AppTypes.URL)
            {
                UrlInput.Visibility = Visibility.Visible;
                LocalFileInput.Visibility = Visibility.Collapsed;
            }
        }
    }
}
