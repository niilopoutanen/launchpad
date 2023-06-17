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
        private static readonly string saveFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NiiloPoutanen", "LaunchPad");
        public static readonly string iconsDirectory = Path.Combine(saveFileLocation, "Icons");
        private static readonly string appsList = Path.Combine(saveFileLocation, "apps.json");

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
            using (StreamWriter streamWriter = new StreamWriter(appsList))
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
