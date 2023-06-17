using LaunchPadClassLibrary;
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

namespace LaunchPad
{
    public partial class Icon : UserControl
    {
        public AppShortcut app;
        public Icon(AppShortcut app, Action<string> handler)
        {
            this.app = app;
            InitializeComponent();

            iconContainer.MouseLeftButtonUp += async (s,e) =>
            {
                await Task.Delay(200); //Wait for the animation
                handler(app.ExeUri);
            };
            InitializeIcon();
        }


        /// <summary>
        /// Displays the app icon based on the values provided
        /// </summary>
        /// <param name="iconPath">Path to the app icon</param>
        /// <param name="appURI">If no app icon is present, falling back to executable icon</param>
        private void InitializeIcon()
        {
            if(app.IconFileName == null)
            {
                System.Drawing.Icon appIcon = System.Drawing.Icon.ExtractAssociatedIcon(app.ExeUri);

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
                if (Uri.TryCreate(app.GetIconFullPath(), UriKind.Absolute, out Uri validUri))
                {
                    BitmapImage bitmapImage = new BitmapImage(validUri);
                    iconBitmap.Source = bitmapImage;
                }
            }


            if(app.IconSize == AppShortcut.SIZE_FULL)
            {
                iconContainer.Padding = new Thickness(0);
                iconContainer.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0,0,0,0));
            }
        }
    }
}
