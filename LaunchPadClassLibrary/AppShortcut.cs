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
        public int Order { get; set; }

        public int IconSize { get; set; }

        public AppShortcut(string name, string exeUri, string? iconFileName, int iconSize)
        {
            Name = name;
            ExeUri = exeUri;
            if (iconFileName != null)
            {
                IconFileName = iconFileName;
            }
            IconSize = iconSize;
            ID = GetId();
            Order = GetPosition();
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

        public void IncreasePos()
        {
            // Load the list of AppShortcuts from JSON
            List<AppShortcut> appShortcuts = SaveSystem.LoadApps();

            // Find the current position of the AppShortcut
            int currentIndex = appShortcuts.FindIndex(a => a.ID == ID);

            // If the current position is the last item, no need to increase the position
            if (currentIndex == appShortcuts.Count - 1)
                return;

            // Swap the AppShortcut with the next one
            SwapPositions(appShortcuts, currentIndex, currentIndex + 1);

            // Save the updated list of AppShortcuts to JSON
            SaveSystem.SaveApps(appShortcuts);
        }

        public void DecreasePos()
        {
            // Load the list of AppShortcuts from JSON
            List<AppShortcut> appShortcuts = SaveSystem.LoadApps();

            // Find the current position of the AppShortcut
            int currentIndex = appShortcuts.FindIndex(a => a.ID == ID);

            // If the current position is the first item, no need to decrease the position
            if (currentIndex == 0)
                return;

            // Swap the AppShortcut with the previous one
            SwapPositions(appShortcuts, currentIndex, currentIndex - 1);

            // Save the updated list of AppShortcuts to JSON
            SaveSystem.SaveApps(appShortcuts);
        }

        private static void SwapPositions(List<AppShortcut> apps, int index1, int index2)
        {
            // Swap the positions of the AppShortcuts in the list
            AppShortcut temp = apps[index1];
            apps[index1] = apps[index2];
            apps[index2] = temp;

            // Update the Order property of the swapped AppShortcuts
            apps[index1].Order = index1;
            apps[index2].Order = index2;
        }

        public static int GetPosition()
        {
            // Load the list of AppShortcuts from JSON
            List<AppShortcut> appShortcuts = SaveSystem.LoadApps();

            // Sort the list based on the Order property
            appShortcuts.Sort((a1, a2) => a1.Order.CompareTo(a2.Order));

            // Find the next available position
            int position = 0;
            foreach (AppShortcut appShortcut in appShortcuts)
            {
                if (appShortcut.Order == position)
                    position++;
                else
                    break;
            }

            return position;
        }

    }
}
