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
                AppDialog option2 = new AppDialog();
                option2.Show();
                this.Close();
            };
            Option3.MouseLeftButtonUp += (s, e) =>
            {
                AppDialog option3 = new AppDialog();
                option3.Show();
                this.Close();
            };
        }

    }
}
