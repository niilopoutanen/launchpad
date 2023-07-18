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
        private readonly AppShortcut.AppTypes type;
        public AppDialog(AppShortcut.AppTypes type)
        {
            this.type = type;
            InitializeComponent();
            InitializeInput();
            Cancel.Click += (s, e) =>
            {
                this.Close();
            };
            this.MouseLeftButtonDown += (s, e) =>
            {
                this.DragMove();
            };
        }
        public void InitializeInput()
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
        public bool InputsAreValid()
        {
            return false;
        }
        public AppShortcut? Get()
        {
            if (!InputsAreValid())
            {
                return null;
            }
            AppShortcut returnvalue = new()
            {
                Name = AppName.Text,
                AppType = type
            };

            return returnvalue;
        }
    }
}
