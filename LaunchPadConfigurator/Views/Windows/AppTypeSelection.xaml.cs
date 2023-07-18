using LaunchPadConfiguratorWPF.Views.Windows;
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
                this.Close();
            };
            Option3.MouseLeftButtonUp += (s, e) =>
            {
                this.Close();
            };
        }

    }
}
