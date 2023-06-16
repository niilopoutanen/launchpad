using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace LaunchPad
{
    /// <summary>
    /// Interaction logic for Icon.xaml
    /// </summary>
    public partial class Icon : UserControl
    {
        private string appURI = "";
        private Action handler;
        public Icon()
        {
            InitializeComponent();
        }

        public Icon(string appURI, Action handler)
        {
            this.appURI = appURI;
            this.handler = handler;
            InitializeComponent();

            iconContainer.MouseLeftButtonUp += IconClick;
        }

        private void IconClick(object sender, MouseButtonEventArgs e)
        {
            Process process = new();
            process.StartInfo.FileName = appURI;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            handler.Invoke();
        }
    }
}
