using LaunchPadClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LaunchPadConfigurator
{
    public class SaveSystem
    {
        private static readonly string saveFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NiiloPoutanen", "LaunchPad");
        public static readonly string iconsDirectory = Path.Combine(saveFileLocation, "Icons");
        private static readonly string apps = Path.Combine(saveFileLocation, "apps.json");
        private static readonly string preferences = Path.Combine(saveFileLocation, "LaunchPad.prefs");

        public static readonly string LaunchPadExecutable = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NiiloPoutanen", "LaunchPad", "LaunchPad.exe");

        public static void SaveApps(List<AppShortcut> apps)
        {
            foreach (AppShortcut app in apps)
            {
                if(string.IsNullOrEmpty(app.IconFileName)) { continue; }
                string filename = Path.GetFileName(app.IconFileName);
                if (app.IconFileName != filename)
                {
                    // Icon has not yet been moved
                    if (File.Exists(app.IconFileName))
                    {
                        CopyIconToAppData(app.IconFileName);
                        app.IconFileName = filename;
                    }
                    else
                    {
                        //Icon lost
                        app.IconFileName = null;
                    }
                }
            }
            string jsonString = JsonSerializer.Serialize(apps);
            EnsureSaveFolderExists();
            using (StreamWriter streamWriter = new StreamWriter(SaveSystem.apps))
            {
                streamWriter.Write(jsonString);
            }
        }
        public static void SaveApp(AppShortcut app)
        {
            EnsureSaveFolderExists();
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


        private static void CopyIconToAppData(string currentPath)
        {
            string finalPath = Path.Combine(iconsDirectory, Path.GetFileName(currentPath));
            if(finalPath != currentPath)
            {
                File.Copy(currentPath, finalPath, true);
            }
            
        }
        public static List<AppShortcut> LoadApps()
        {
            List<AppShortcut> apps = new();
            
            EnsureSaveFolderExists();
            if (File.Exists(SaveSystem.apps))
            {
                string jsonString = File.ReadAllText(SaveSystem.apps) ?? throw new FileLoadException("File is empty");
                apps = JsonSerializer.Deserialize<List<AppShortcut>>(jsonString);
                apps.Sort((app1, app2) => app1.Order.CompareTo(app2.Order));
            }

            return apps;
        }
        static void EnsureSaveFolderExists()
        {
            if (!Directory.Exists(saveFileLocation))
            {
                Directory.CreateDirectory(saveFileLocation);
            }
            if (!Directory.Exists(iconsDirectory))
            {
                Directory.CreateDirectory(iconsDirectory);
            }
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


        public static void SavePreferences(UserPreferences prefs)
        {
            string jsonString = JsonSerializer.Serialize(prefs);
            EnsureSaveFolderExists();
            using (StreamWriter streamWriter = new(preferences))
            {
                streamWriter.Write(jsonString);
            }
        }
        public static UserPreferences LoadPreferences()
        {
            UserPreferences prefs = new();
            EnsureSaveFolderExists();
            if (File.Exists(preferences))
            {
                string jsonString = File.ReadAllText(preferences) ?? throw new FileLoadException("File is empty");
                prefs = JsonSerializer.Deserialize<UserPreferences>(jsonString);
            }
            if(prefs != null)
            {
                return prefs;
            }
            else
            {
                return new UserPreferences();
            }
            
        }
    }
}
