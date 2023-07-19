using LaunchPadCore.Utility;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LaunchPadCore.Models
{
    public class AppShortcut
    {
        public string Name { get; set; }
        public string ExeUri { get; set; }
        public string? IconFileName { get; set; }
        public int ID { get; set; }
        public int Position { get; set; }
        public AppTypes AppType { get; set; }

        public enum AppTypes
        {
            MS_STORE,
            EXE,
            URL
        }
        private enum Browsers
        {
            Chrome,
            Edge,
            Brave,
            Firefox,
            None
        }

        public AppShortcut(string name, string exeUri, string? iconFileName, AppTypes appType)
        {
            Name = name;
            ExeUri = exeUri;
            if (iconFileName != null)
            {
                IconFileName = iconFileName;
            }
            ID = GetId();
            Position = GetPosition();
            AppType = appType;
        }
        public AppShortcut() { }

        public void Initialize()
        {
            ID = GetId();
            Position = GetPosition();
        }
        public string GetIconFullPath()
        {
            if (IconFileName == null)
            {
                return ExeUri;
            }
            string filename = Path.GetFileName(IconFileName);
            if (IconFileName != filename)
            {
                IconFileName = IconFileName.Replace("%20", " ");
                return IconFileName;
            }
            if(File.Exists(Path.Combine(SaveSystem.iconsDirectory, IconFileName)))
            {
                return Path.Combine(SaveSystem.iconsDirectory, IconFileName);
            }
            else if(File.Exists(Path.Combine(SaveSystem.predefinedIconsDirectory, IconFileName)))
            {
                return Path.Combine(SaveSystem.predefinedIconsDirectory, IconFileName);
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

        static Browsers GetDefaultBrowser()
        {
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
            {
                if (userChoiceKey != null)
                {
                    object progIdValue = userChoiceKey.GetValue("Progid");

                    if (progIdValue != null)
                    {
                        string progId = progIdValue.ToString();

                        if (progId.Contains("ChromeHTML"))
                        {
                            return Browsers.Chrome;
                        }
                        else if (progId.Contains("MSEdgeHTM"))
                        {
                            return Browsers.Edge;
                        }
                        else if (progId.Contains("BraveHTML"))
                        {
                            return Browsers.Brave;
                        }
                        else if (progId.Contains("FirefoxHTML"))
                        {
                            return Browsers.Firefox;
                        }
                    }
                }
            }

            return Browsers.None; // Default if none found
        }
        public static ImageSource GetIcon(AppShortcut app)
        {
            try
            {
                if (app.AppType == AppTypes.URL)
                {
                    string imagePath = string.Empty;

                    switch (GetDefaultBrowser())
                    {
                        case Browsers.Chrome:
                            imagePath = "pack://application:,,,/Resources/Assets/browser_chrome.png";
                            break;

                        case Browsers.Edge:
                            imagePath = "pack://application:,,,/Resources/Assets/browser_edge.png";
                            break;

                        case Browsers.Brave:
                            imagePath = "pack://application:,,,/Resources/Assets/browser_brave.png";
                            break;

                        case Browsers.Firefox:
                            imagePath = "pack://application:,,,/Resources/Assets/browser_firefox.png";
                            break;

                        case Browsers.None:
                            imagePath = "pack://application:,,,/Resources/Assets/browser_none.png";
                            break;
                    }

                    BitmapImage imageSource = new(new Uri(imagePath, UriKind.Absolute));

                    return imageSource;
                }
                if (app.AppType == AppTypes.EXE || app.AppType == AppTypes.MS_STORE)
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
                else
                {
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Assets/icon_null.png", UriKind.Absolute));
                }
            }
            catch
            {
                return new BitmapImage(new Uri("pack://application:,,,/Resources/Assets/icon_null.png", UriKind.Absolute));
            }
        }

        public void IncreasePos()
        {
            List<AppShortcut> appShortcuts = SaveSystem.LoadApps();
            int currentIndex = appShortcuts.FindIndex(a => a.ID == ID);

            if (currentIndex == appShortcuts.Count - 1)
                return;

            SwapPositions(appShortcuts, currentIndex, currentIndex + 1);
            SaveSystem.SaveApps(appShortcuts);
        }

        public void DecreasePos()
        {
            List<AppShortcut> appShortcuts = SaveSystem.LoadApps();
            int currentIndex = appShortcuts.FindIndex(a => a.ID == ID);

            if (currentIndex == 0)
                return;

            SwapPositions(appShortcuts, currentIndex, currentIndex - 1);
            SaveSystem.SaveApps(appShortcuts);
        }

        private static void SwapPositions(List<AppShortcut> apps, int index1, int index2)
        {
            (apps[index2], apps[index1]) = (apps[index1], apps[index2]);

            apps[index1].Position = index1;
            apps[index2].Position = index2;
        }

        public static int GetPosition()
        {
            List<AppShortcut> appShortcuts = SaveSystem.LoadApps();

            appShortcuts.Sort((a1, a2) => a1.Position.CompareTo(a2.Position));

            // Find the next available position
            int position = 0;
            foreach (AppShortcut appShortcut in appShortcuts)
            {
                if (appShortcut.Position == position)
                    position++;
                else
                    break;
            }

            return position;
        }

    }
}
