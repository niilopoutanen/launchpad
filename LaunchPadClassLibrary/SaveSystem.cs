using LaunchPadClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LaunchPadConfigurator
{
    public class SaveSystem
    {
        private static string saveFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NiiloPoutanen", "LaunchPad");
        private static string iconsDirectory = Path.Combine(saveFileLocation, "Icons");
        private static string appsList = Path.Combine(saveFileLocation, "apps.json");

        public static void SaveApps(List<AppShortcut> apps)
        {
            string jsonString = JsonSerializer.Serialize(apps);
            EnsureSaveFolderExists();
            File.WriteAllText(appsList, jsonString);
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

        public static List<AppShortcut> LoadApps()
        {
            List<AppShortcut> apps = new();
            EnsureSaveFolderExists();
            if (File.Exists(appsList))
            {
                string jsonString = File.ReadAllText(appsList) ?? throw new FileLoadException("File is empty");
                apps = JsonSerializer.Deserialize<List<AppShortcut>>(jsonString);
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
    }
}
