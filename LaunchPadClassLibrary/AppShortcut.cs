using LaunchPadConfigurator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace LaunchPadClassLibrary
{
    public class AppShortcut
    {
        public const int SIZE_FULL = 1;
        public const int SIZE_CROPPED = 0;


        public string Name {  get; set; }
        public string ExeUri { get; set; }
        public string? IconFileName { get; set; }
        public int ID { get; set; }

        public int IconSize { get; set; }

        public AppShortcut(string name, string exeUri, string? iconFileName, int iconSize)
        {
            Name = name;
            ExeUri = exeUri;
            if(iconFileName != null)
            {
                IconFileName = iconFileName;
            }
            IconSize = iconSize;
            ID = GetId();
        }
        public AppShortcut() { }


        public string GetIconFullPath()
        {
            if(IconFileName == null)
            {
                return ExeUri;
            }
            string filename = Path.GetFileName(IconFileName);
            if (IconFileName != filename)
            {
                return IconFileName;
            }
            return Path.Combine(SaveSystem.iconsDirectory, IconFileName);
        }

        private static int GetId()
        {
            var usedValues = new List<int>();
            foreach (AppShortcut app in SaveSystem.LoadApps())
            {
                usedValues.Add(app.ID);
            }

            if (usedValues.Count == 0)
            {
                return 1;
            }

            usedValues.Sort();
            return usedValues.Last() + 1;
        }


        public static ImageSource GetIcon(AppShortcut app)
        {
            if (app.IconFileName == null)
            {
                Icon appIcon = Icon.ExtractAssociatedIcon(app.ExeUri);

                if (appIcon != null)
                {
                    ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                        appIcon.Handle,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions()
                    );

                    return imageSource;
                }
                return null;
            }
            else
            {
                if (Uri.TryCreate(app.GetIconFullPath(), UriKind.Absolute, out Uri validUri))
                {
                    if (Path.GetExtension(validUri.LocalPath) == ".ico")
                    {
                        BitmapDecoder decoder = BitmapDecoder.Create(validUri, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

                        int maxWidth = 0;
                        int maxHeight = 0;
                        BitmapFrame highestResFrame = null;

                        foreach (BitmapFrame frame in decoder.Frames)
                        {
                            if (frame.PixelWidth > maxWidth && frame.PixelHeight > maxHeight)
                            {
                                maxWidth = frame.PixelWidth;
                                maxHeight = frame.PixelHeight;
                                highestResFrame = frame;
                            }
                        }

                        if (highestResFrame != null)
                        {
                            return highestResFrame;
                        }
                    }
                    else
                    {
                        return new BitmapImage(validUri);
                    }
                }
                return null;
            }
        }

    }
}
