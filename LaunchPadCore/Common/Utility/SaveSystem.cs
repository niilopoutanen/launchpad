using LaunchPadCore.Models;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;

namespace LaunchPadCore.Utility
{
    public class SaveSystem
    {
        //local
        private static readonly string saveFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NiiloPoutanen", "LaunchPad");
        public static readonly string iconsDirectory = Path.Combine(saveFileLocation, "icons");
        private static readonly string apps = Path.Combine(saveFileLocation, "apps.json");
        public static readonly string preferences = Path.Combine(saveFileLocation, "launchpad.prefs");

        //server
        public static readonly string predefinedIconsDirectory = Path.Combine(saveFileLocation, "icons", "predefined");
        public static readonly string predefinedAppsList = Path.Combine(saveFileLocation, "icons", "predefined", "predefined-apps.json");


        public static void SaveApps(List<AppShortcut> apps)
        {
            foreach (AppShortcut app in apps)
            {
                if (string.IsNullOrEmpty(app.IconFileName)) { continue; }
                string filename = Path.GetFileName(app.IconFileName);
                if (app.IconFileName != filename)
                {
                    // Icon has not yet been moved
                    if (File.Exists(app.IconFileName))
                    {
                        app.IconFileName = SaveIcon(app.IconFileName, false);
                    }
                    else
                    {
                        //Icon lost
                        app.IconFileName = null;
                    }
                }
            }
            string jsonString = JsonSerializer.Serialize(apps);
            VerifyPathIntegrity();
            using (StreamWriter streamWriter = new(SaveSystem.apps))
            {
                streamWriter.Write(jsonString);
            }
        }
        public static void SaveApp(AppShortcut app)
        {
            VerifyPathIntegrity();
            List<AppShortcut> existingApps = LoadApps();

            AppShortcut? existingApp = existingApps.FirstOrDefault(a => a.Name == app.Name);

            if (existingApp != null)
            {
                existingApps.Remove(existingApp);
                existingApp.Name = app.Name;
                existingApp.IconFileName = app.IconFileName;
                existingApp.ExeUri = app.ExeUri;
                existingApps.Add(existingApp);
            }
            else
            {
                existingApps.Add(app);
            }

            // Save the updated list back to storage
            SaveApps(existingApps);
        }

        /// <returns>Final name of the file</returns>
        public static string SaveIcon(string fileName, byte[] data, bool overWrite)
        {
            VerifyPathIntegrity();
            string filePath = Path.Combine(iconsDirectory, fileName);
            if (!overWrite)
            {
                if (File.Exists(filePath))
                {
                    fileName += DateTime.Now.ToShortTimeString();
                    filePath = Path.Combine(iconsDirectory, fileName);
                }
            }
            File.WriteAllBytes(filePath, data);

            return fileName;
        }
        public static string SaveIcon(string currentPath, bool overWrite)
        {
            VerifyPathIntegrity();
            string fileName = Path.GetFileName(currentPath);
            string filePath = Path.Combine(iconsDirectory, fileName);
            if (!overWrite)
            {
                if (File.Exists(filePath))
                {
                    fileName = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToShortTimeString() + Path.GetExtension(fileName);
                    filePath = Path.Combine(iconsDirectory, fileName);
                }
            }
            File.Copy(currentPath, filePath, overWrite);

            return fileName;
        }

        public static List<AppShortcut> LoadApps()
        {
            List<AppShortcut> apps = new();

            VerifyPathIntegrity();
            if (File.Exists(SaveSystem.apps))
            {
                string jsonString = File.ReadAllText(SaveSystem.apps) ?? throw new FileLoadException("File is empty");
                apps = JsonSerializer.Deserialize<List<AppShortcut>>(jsonString);
                apps.Sort((app1, app2) => app1.Position.CompareTo(app2.Position));
            }

            return apps;
        }
        public static void VerifyPathIntegrity()
        {
            Directory.CreateDirectory(iconsDirectory);
            Directory.CreateDirectory(predefinedIconsDirectory);
            Directory.CreateDirectory(saveFileLocation);
        }

        public static void DeleteUnusedIcons()
        {
            List<AppShortcut> apps = LoadApps();
            string[] files = Directory.GetFiles(iconsDirectory);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                bool isUsed = apps.Any(app => !string.IsNullOrEmpty(app.IconFileName) && Path.GetFileName(app.IconFileName) == fileName);

                if (!isUsed)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception)
                    {
                        //Could not delete file.
                    }
                }
            }
        }

        public static ResourceDictionary LoadTheme()
        {
            UserPreferences preferences = UserPreferences.Load();
            string themePath;

            switch (preferences.SelectedTheme)
            {
                case UserPreferences.LaunchPadTheme.Dark:
                    themePath = preferences.TransparentTheme ? "Resources/TransparentDark.xaml" : "Resources/DarkMode.xaml";
                    break;

                case UserPreferences.LaunchPadTheme.Light:
                    themePath = preferences.TransparentTheme ? "Resources/TransparentLight.xaml" : "Resources/LightMode.xaml";
                    break;

                default:
                    themePath = preferences.TransparentTheme
                        ? Core.IsLightTheme() ? "Resources/TransparentLight.xaml" : "Resources/TransparentDark.xaml"
                        : Core.IsLightTheme() ? "Resources/LightMode.xaml" : "Resources/DarkMode.xaml";
                    break;
            }

            return new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };
        }
    }
}
