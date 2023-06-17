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
        private string iconFile = "";

        public Icon(string appURI, string iconFile, Action<string> handler)
        {
            this.appURI = appURI;
            this.iconFile = iconFile;
            InitializeComponent();

            iconContainer.MouseLeftButtonUp += async (s,e) =>
            {
                await Task.Delay(200); //Wait for the animation
                handler(appURI);
            };
            InitializeIcon();
        }

        private void InitializeIcon()
        {
            if(iconFile == null)
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
                    appIcon.Dispose();
                }
            }
            else
            {
                if (Uri.TryCreate(iconFile, UriKind.Absolute, out Uri validUri))
                {
                    BitmapImage bitmapImage = new BitmapImage(validUri);
                    iconBitmap.Source = bitmapImage;
                }
            }
        }
    }
}
