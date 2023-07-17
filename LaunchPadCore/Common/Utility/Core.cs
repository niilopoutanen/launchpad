﻿using LaunchPadCore.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadCore.Utility
{
    public class Core
    {
        public const int APP_LAUNCHPAD = 0;
        public const int APP_LAUNCHPADCONFIG = 1;

        public const string APPID_LAUNCHPAD = "NiiloPoutanen.LaunchPad_16tzdb5rranka!App";
        public const string APPID_LAUNCHPADCONF = "NiiloPoutanen.LaunchPadConfigurator_g37tm3x42n8em!App";
        public static void LaunchApp(int appType)
        {
            switch (appType)
            {
                case APP_LAUNCHPAD:
                    Process.Start("explorer.exe", "shell:appsfolder\\" + APPID_LAUNCHPAD);
                    break;

                case APP_LAUNCHPADCONFIG:
                    Process.Start("explorer.exe", "shell:appsfolder\\" + APPID_LAUNCHPADCONF);
                    break;
            }
        }
        public static bool LaunchApp(string appId)
        {
            try
            {
                Process.Start("explorer.exe", "shell:appsfolder\\" + appId);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async void UpdateData()
        {
            if (await DataManager.IsLatestData())
            {
                return;
            }

            Dictionary<string[], string> apps = await DataManager.GetData();
            await DataManager.ProcessData(apps);

        }
        public static bool IsLightTheme()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var value = key?.GetValue("AppsUseLightTheme");
            return value is int i && i > 0;
        }
    }
}
