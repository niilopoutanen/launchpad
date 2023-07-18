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
        }
    }
}
