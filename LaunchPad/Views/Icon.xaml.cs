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
    public partial class Icon : UserControl
    {
        public Icon(string appURI, string iconPath, Action<string> handler)
        {
            InitializeComponent();

            iconContainer.MouseLeftButtonUp += async (s,e) =>
            {
                await Task.Delay(200); //Wait for the animation
                handler(appURI);
            };
            InitializeIcon(iconPath, appURI);
        }


        /// <summary>
        /// Displays the app icon based on the values provided
        /// </summary>
        /// <param name="iconPath">Path to the app icon</param>
        /// <param name="appURI">If no app icon is present, falling back to executable icon</param>
        private void InitializeIcon(string iconPath, string appURI)
        {
            if(iconPath == null)
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
                if (Uri.TryCreate(iconPath, UriKind.Absolute, out Uri validUri))
                {
                    BitmapImage bitmapImage = new BitmapImage(validUri);
                    iconBitmap.Source = bitmapImage;
                }
            }
        }
    }
}
