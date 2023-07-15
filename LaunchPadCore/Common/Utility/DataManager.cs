using LaunchPadCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaunchPadCore.Utility
{
    public class DataManager
    {
        private const string baseUrl = "https://raw.githubusercontent.com/niilopoutanen/LaunchPad/data/";
        private const string latestUpdateUrl = baseUrl + "data.updated";
        private const string iconsUrl = baseUrl + "icons/";
        private const string appList = baseUrl + "app-list.json";

        public static async Task<Dictionary<string, string>> GetData()
        {
            Dictionary<string, string> templateApps = new();

            using (HttpClient client = new())
            {
                try
                {
                    string json = await client.GetStringAsync(appList);
                    templateApps = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                    SaveSystem.VerifyPathIntegrity();
                    File.WriteAllText(SaveSystem.predefinedAppsList, json);
                }
                catch { }
            }

            if (templateApps != null)
            {
                return templateApps;
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }
        public static async Task<bool> IsLatestData()
        {
            long local = 0;
            long server = 0;

            using (HttpClient client = new())
            {
                byte[] data = await client.GetByteArrayAsync(latestUpdateUrl);
                string dataString = Encoding.UTF8.GetString(data);
                if (long.TryParse(dataString, out long version))
                {
                    server = version;
                }
            }
            if (!File.Exists(Path.Combine(SaveSystem.predefinedIconsDirectory, "data.updated")))
            {
                return false;
            }
            string currentDataVersion = File.ReadAllText(Path.Combine(SaveSystem.predefinedIconsDirectory, "data.updated"));
            if (string.IsNullOrEmpty(currentDataVersion))
            {
                local = 0;
            }
            else
            {
                local = (long)Convert.ToDouble(currentDataVersion);
            }

            if (local == server)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task ProcessData(Dictionary<string, string> data)
        {
            List<string> localApps = GetApps();
            foreach (var keyValuePair in data)
            {
                if (localApps.Contains(keyValuePair.Key))
                {
                    await DownloadIcon(keyValuePair.Value);
                }
            }

        }


        public static List<string> GetApps()
        {
            List<string> apps = new();
            using (PowerShell ps = PowerShell.Create())
            {
                //Bypass the execution policy
                ps.AddCommand("Set-ExecutionPolicy")
                    .AddParameter("ExecutionPolicy", "Bypass")
                    .AddParameter("Scope", "Process")
                    .Invoke();


                ps.AddCommand("Get-StartApps");

                Collection<PSObject> results = ps.Invoke();
                foreach (PSObject obj in results)
                {
                    string appId = obj.Properties["AppID"].Value.ToString();

                    apps.Add(appId);
                }
            }
            return apps;
        }
        /// <returns>Final file name</returns>
        private static async Task DownloadIcon(string fileName)
        {
            if (File.Exists(Path.Combine(SaveSystem.predefinedIconsDirectory, fileName)))
            {
                return;
            }
            string path = iconsUrl + fileName;
            using (HttpClient client = new())
            {
                try
                {
                    byte[] data = await client.GetByteArrayAsync(path);
                    SaveSystem.VerifyPathIntegrity();
                    string filePath = Path.Combine(SaveSystem.predefinedIconsDirectory, fileName);
                    if (File.Exists(filePath))
                    {
                        return;
                    }
                    File.WriteAllBytes(filePath, data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
