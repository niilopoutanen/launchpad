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

namespace LaunchPadCore.Utility
{
    public class DataManager
    {
        private const string baseUrl = "https://raw.githubusercontent.com/niilopoutanen/LaunchPad/data/";
        private const string iconsUrl = baseUrl + "icons/";
        private const string appList = baseUrl + "app-list.json";

        public static async Task<List<AppTemplate>> GetData()
        {
            GetApps();
            List<AppTemplate> templateApps = new();

            using (HttpClient client = new())
            {
                try
                {
                    string json = await client.GetStringAsync(appList);
                    templateApps = JsonSerializer.Deserialize<List<AppTemplate>>(json);
                }
                catch{}
            }

            if(templateApps != null)
            {
                await ProcessData(templateApps);
                return templateApps;
            }
            else
            {
                return new List<AppTemplate>();
            }
        }
        private static async Task ProcessData(List<AppTemplate> data)
        {
            foreach(AppTemplate template in data)
            {
                await DownloadIcon(template.IconFileName);
            }
        }

        public static Dictionary<string,string> GetApps()
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
                    string name = obj.Properties["Name"].Value.ToString();
                    string appId = obj.Properties["AppID"].Value.ToString();

                    if(!apps.ContainsKey(appId))
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
            if (File.Exists(Path.Combine(SaveSystem.iconsDirectory, fileName)))
            {
                return;
            }
            string path = iconsUrl + fileName;
            using (HttpClient client = new())
            {
                try
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(path);
                    SaveSystem.SaveIcon(fileName,imageBytes, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
