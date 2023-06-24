﻿using LaunchPadConfigurator;
using LaunchPadCore;
using System;
using System.Diagnostics;
using System.Runtime;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Windows.UI.ViewManagement;

namespace LaunchPad
{
    public partial class Icon : LaunchPadItem
    {
        public override AppShortcut App { get; set; }
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override UIElement BaseElement { get => appIcon; }

        private UserPreferences preferences;

        public Icon(AppShortcut app)
        {
            this.App = app;
            preferences = SaveSystem.LoadPreferences();
            InitializeComponent();
            InitializeIcon();

            base.InitializeControl();

        }


        private void InitializeIcon()
        {
            iconBitmap.Source = AppShortcut.GetIcon(App);


            if (preferences.FullSizeIcon)
            {
                appIcon.Padding = new Thickness(0);
                appIcon.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
            }

            if (preferences.NameVisible)
            {
                appName.Visibility = Visibility.Visible;
                appIcon.Width = 80;
                appIcon.Height = 80;


                if (App.Name.Length > 13)
                {
                    appName.Text = App.Name[..13] + "..";
                }

                else
                {
                    appName.Text = App.Name;
                }
            }


        }

        public override async Task OnClick()
        {
            await Task.Run(() =>
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = App.ExeUri;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                catch (Exception)
                {
                    if (System.Windows.Application.Current is App hostApp)
                    {
                        hostApp.DisplayMessage("Error", "LaunchPad could not open the app you selected. Verify that the app/file exists.", ToolTipIcon.Error);
                    }
                }
            });
            ((App)System.Windows.Application.Current).ToggleLaunchpad();

        }



        public override void SetTheme(ResourceDictionary activeDictionary)
        {
            SolidColorBrush itemBackgroundColor = activeDictionary["LaunchPadItemBackground"] as SolidColorBrush;
            SolidColorBrush textColor = activeDictionary["LaunchPadTextColor"] as SolidColorBrush;


            if (itemBackgroundColor == null || textColor == null)
            {
                return;
            }
            if (!preferences.FullSizeIcon)
            {
                appIcon.Background = itemBackgroundColor;
            }
            appName.Foreground = textColor;

        }
    }
}
