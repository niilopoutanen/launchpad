﻿using LaunchPadCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LaunchPadCore.Utility
{
    public class DataManager
    {
        private const string baseUrl = "https://raw.githubusercontent.com/niilopoutanen/LaunchPad/data/";
        private const string iconsUrl = baseUrl + "icons/";
        private const string appList = baseUrl + "app-list.json";

        public static async Task<List<AppTemplate>> GetData()
        {
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
        private static async Task DownloadIcon(string fileName)
        {
            string path = iconsUrl + fileName;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(path);

                    Directory.CreateDirectory(SaveSystem.iconsDirectory);

                    string file = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}.jpg";

                    string filePath = Path.Combine(SaveSystem.iconsDirectory, file);
                    File.WriteAllBytes(filePath, imageBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
