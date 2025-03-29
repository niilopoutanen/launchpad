using Core;
using Core.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Application = System.Windows.Application;


namespace LaunchPad
{
    public partial class Main : Window
    {
        public Main()
        {
            LoadTheme();

            InitializeComponent();
        }


        private void LoadTheme()
        {
            Theme theme = DbManager.LoadTheme();
            ResourceDictionary selectedTheme ;
            if (!theme.Rounded)
            {
                var roundedTheme = new ResourceDictionary
                {
                    Source = new Uri($"Themes/Rounded.xaml", UriKind.Relative)
                };

            }


            switch (theme.Style)
            {
                case Theme.Styles.Dark:
                    selectedTheme = new ResourceDictionary
                    {
                        Source = new Uri($"Themes/Dark.xaml", UriKind.Relative)
                    };
                    break;

                case Theme.Styles.DarkTransparent:
                default:
                    selectedTheme = new ResourceDictionary
                    {
                        Source = new Uri($"Themes/DarkTransparent.xaml", UriKind.Relative)
                    };
                    break;

                case Theme.Styles.Light:
                    selectedTheme = new ResourceDictionary
                    {
                        Source = new Uri($"Themes/Light.xaml", UriKind.Relative)
                    };
                    break;

                case Theme.Styles.LightTransparent:
                    selectedTheme = new ResourceDictionary
                    {
                        Source = new Uri($"Themes/LightTransparent.xaml", UriKind.Relative)
                    };
                    break;
            }

            this.Resources.MergedDictionaries.Add(selectedTheme);
        }
    }
}