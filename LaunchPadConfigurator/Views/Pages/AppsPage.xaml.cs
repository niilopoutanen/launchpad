using LaunchPadConfigurator.Views.Windows;
using LaunchPadConfiguratorWPF.Views.Controls;
using LaunchPadCore.Models;
using LaunchPadCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPadConfiguratorWPF.Views.Pages
{
    public partial class AppsPage : Page
    {
        public AppsPage()
        {
            InitializeComponent();
            RefreshAppList();
            AddAppButton.Click += (s, e) =>
            {
                AppTypeSelection selection = new();
                selection.Show();
            };
        }
        private void RefreshAppList()
        {
            AppsList.Children.Clear();
            List<AppShortcut> apps = SaveSystem.LoadApps();

            foreach (AppShortcut app in apps)
            {
                AppListControl listItem = new(app, RefreshAppList);
                AppsList.Children.Add(listItem);
            }

            UserPreferences preferences = UserPreferences.Load();
            List<Widget> widgets = Widget.LoadWidgets();
            foreach (Widget widget in widgets)
            {
                foreach (string key in preferences.ActiveWidgets.Keys)
                {
                    if (widget.ID == key && preferences.ActiveWidgets[key] == true)
                    {
                        AppListControl widgetListItem = new(widget, RefreshAppList);
                        AppsList.Children.Add(widgetListItem);
                        break;
                    }
                }
            }
        }

    }
}
