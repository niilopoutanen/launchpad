using LaunchPadConfiguratorWPF.Views.Windows;
using LaunchPadCore.Models;
using LaunchPadCore.Utility;
using Microsoft.CodeAnalysis;
using System;
using System.Windows;


namespace LaunchPadConfigurator.Views.Windows
{
    public partial class AppTypeSelection : Window
    {
        public AppTypeSelection(Action refresher)
        {
            InitializeComponent();
            Option1.MouseLeftButtonUp += (s, e) =>
            {
                AppGallery option1 = new(refresher);
                option1.Show();
                this.Close();
            };
            Option2.MouseLeftButtonUp += (s, e) =>
            {
                AppDialog option2 = new(AppShortcut.AppTypes.EXE);
                option2.Show();

                option2.onCompleted += (s, e) =>
                {
                    if(e != null)
                    {
                        SaveSystem.SaveApp(e);
                        refresher.Invoke();
                    }
                };
                this.Close();
            };
            Option3.MouseLeftButtonUp += (s, e) =>
            {
                AppDialog option3 = new(AppShortcut.AppTypes.URL);
                option3.Show();

                option3.onCompleted += (s, e) =>
                {
                    if (e != null)
                    {
                        SaveSystem.SaveApp(e);
                        refresher.Invoke();
                    }
                };
                this.Close();
            };
        }

    }
}
