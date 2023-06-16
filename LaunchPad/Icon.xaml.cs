using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace LaunchPad
{
    /// <summary>
    /// Interaction logic for Icon.xaml
    /// </summary>
    public partial class Icon : UserControl
    {
        private string appURI = "";
        private Action handler;

        public Icon(string appURI, Action handler)
        {
            this.appURI = appURI;
            this.handler = handler;
            InitializeComponent();

            iconContainer.MouseLeftButtonUp += IconClick;
            InitializeIcon();
        }

        private void InitializeIcon()
        {
            System.Drawing.Icon appIcon = System.Drawing.Icon.ExtractAssociatedIcon(appURI);

            if (appIcon != null)
            {
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                    appIcon.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                );

                // Set the Source property of the Image control
                iconBitmap.Source = imageSource;

                // Dispose the icon to free resources
                appIcon.Dispose();
            }
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
