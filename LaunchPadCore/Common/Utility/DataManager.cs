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

        public const string PATTERN_FILE = "<f>";
        private static async Task<Dictionary<string[], string>> LoadDataFromServer()
        {
            Dictionary<string[], string> templateApps = new();

            using (HttpClient client = new())
            {
                try
                {
                    string json = await client.GetStringAsync(appList);
                    string latestUpdate = await client.GetStringAsync(latestUpdateUrl);

                    templateApps = ParseDictionary(JsonSerializer.Deserialize<Dictionary<string, string>>(json));
                    SaveSystem.VerifyPathIntegrity();
                    File.WriteAllText(SaveSystem.predefinedAppsList, json);
                    File.WriteAllText(SaveSystem.latestUpdate, latestUpdate);
                }
                catch { }
            }

            if (templateApps != null)
            {
                return templateApps;
            }
            else
            {
                return new Dictionary<string[], string>();
            }
        }
        public static async Task<Dictionary<string[], string>> LoadPredefinedApps()
        {
            if (await IsLatestData())
            {
                string json = File.ReadAllText(SaveSystem.predefinedAppsList);
                return ParseDictionary(JsonSerializer.Deserialize<Dictionary<string, string>>(json));
            }
            else
            {
                return await LoadDataFromServer();
            }
        }
        public static List<Tuple<string, string, string>> MergeData(Dictionary<string,string> localApps, Dictionary<string[], string> serverApps)
        {
            //app name, icon filename, appID
            List<Tuple<string, string, string>> merges = new();
            foreach (var keyValuePair in serverApps)
            {
                foreach (string key in keyValuePair.Key)
                {
                    if (IsAppInstalled(localApps, key))
                    {
                        string appID = DecryptPattern(localApps, key);
                        merges.Add(new Tuple<string, string, string>(localApps[appID], keyValuePair.Value, appID));
                    }
                }

            }
            return merges;
        }
        private static Dictionary<string[], string> ParseDictionary(Dictionary<string,string>? rawData)
        {
            Dictionary<string[], string> parsedData = new();
            if(rawData == null)
            {
                return parsedData;
            }
            foreach (var kvp in rawData)
            {
                string[] keys = kvp.Key.Trim('[', ']').Split(new[] { "', '", "'," }, StringSplitOptions.RemoveEmptyEntries);
                parsedData[keys] = kvp.Value;
            }
            return parsedData;
        }
        public static string DecryptPattern(Dictionary<string,string> localApps, string pattern)
        {
            string result = pattern;
            if (pattern.StartsWith(PATTERN_FILE))
            {
                pattern = pattern[3..];
                foreach(var kvp in localApps)
                {
                    if (kvp.Key.Contains(pattern))
                    {
                        result = kvp.Key;
                        break;
                    }
                }
            }

            return result;
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
            if (!File.Exists(SaveSystem.latestUpdate))
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
                string json = File.ReadAllText(SaveSystem.predefinedAppsList);
                await ProcessData(ParseDictionary(JsonSerializer.Deserialize<Dictionary<string, string>>(json))).ConfigureAwait(false);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task ProcessData(Dictionary<string[], string> data)
        {
            Dictionary<string, string> localApps = LoadInstalledApps();
            foreach (var keyValuePair in data)
            {
                foreach (string key in keyValuePair.Key)
                {
                    if(IsAppInstalled(localApps, key))
                    {
                        await DownloadIcon(keyValuePair.Value);
                    }
                }

            }

        }
        public static bool IsAppInstalled(Dictionary<string, string> localApps, string serverAppID)
        {
            if (localApps.ContainsKey(serverAppID))
            {
                return true;
            }

            if (serverAppID.StartsWith(PATTERN_FILE))
            {
                if (localApps.Any(kvp => kvp.Key.Contains(serverAppID[3..])))
                {
                    return true;
                }
            }



            return false;
        }

        public static Dictionary<string, string> LoadInstalledApps()
        {
            Dictionary<string, string> apps = new();
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
                    string name = obj.Properties["Name"].Value.ToString();
                    if (!apps.ContainsKey(appId))
                    {
                        apps.Add(appId, name);
                    }
                    
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
