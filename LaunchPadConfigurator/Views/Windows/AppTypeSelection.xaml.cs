using LaunchPadConfiguratorWPF.Views.Windows;
using LaunchPadCore.Models;
using Microsoft.CodeAnalysis;
using System.Windows;


namespace LaunchPadConfigurator.Views.Windows
{
    public partial class AppTypeSelection : Window
    {
        public AppTypeSelection()
        {
            InitializeComponent();
            Option1.MouseLeftButtonUp += (s, e) =>
            {
                AppGallery option1 = new();
                option1.Show();
                this.Close();
            };
            Option2.MouseLeftButtonUp += (s, e) =>
            {
                AppDialog option2 = new(AppShortcut.AppTypes.EXE);
                option2.Show();
                this.Close();
            };
            Option3.MouseLeftButtonUp += (s, e) =>
            {
                AppDialog option3 = new(AppShortcut.AppTypes.URL);
                option3.Show();
                this.Close();
            };
        }

    }
}
