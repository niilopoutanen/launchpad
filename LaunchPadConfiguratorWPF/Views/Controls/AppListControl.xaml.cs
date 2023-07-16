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

namespace LaunchPadConfiguratorWPF.Views.Controls
{
    public partial class AppListControl : UserControl
    {
        private AppShortcut? App { get; set; }
        private Widget? Widget { get; set; }

        private Action updateCallback;

        public AppListControl(AppShortcut app, Action updateCallback)
        {
            this.App = app;
            this.updateCallback = updateCallback;
            InitializeComponent();
            InitializeControl();
        }
        public AppListControl(Widget widget, Action updateCallback)
        {
            this.Widget = widget;
            this.updateCallback = updateCallback;
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            if(Widget != null)
            {
                Name.Text = Widget.WidgetName;
                Icon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Widgets/" + Widget.IconFile));

                editButton.Visibility = Visibility.Collapsed;
                posUp.Visibility = Visibility.Collapsed;
                posDown.Visibility = Visibility.Collapsed;
            }
            if(App != null)
            {
                Name.Text = App.Name;
                if (Uri.TryCreate(App.GetIconFullPath(), UriKind.Absolute, out Uri validUri))
                {
                    try
                    {
                        BitmapImage bitmapImage = new(validUri);
                        Icon.Source = bitmapImage;
                    }
                    catch { }
                }
                posUp.Click += (s, e) =>
                {
                    App.IncreasePos();
                    updateCallback.Invoke();
                };
                posDown.Click += (s, e) =>
                {
                    App.DecreasePos();
                    updateCallback.Invoke();
                };
            }
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (App != null)
            {
                List<AppShortcut> existingApps = SaveSystem.LoadApps();

                int indexToRemove = existingApps.FindIndex(app => app.ID == App.ID);

                if (indexToRemove != -1)
                {
                    existingApps.RemoveAt(indexToRemove);

                    for (int i = indexToRemove; i < existingApps.Count; i++)
                    {
                        existingApps[i].Position--;
                    }

                    SaveSystem.SaveApps(existingApps);
                    updateCallback.Invoke();
                    SaveSystem.DeleteUnusedIcons();
                }
            }
            else if (Widget != null)
            {
                List<Widget> widgets = Widget.LoadWidgets();

                int indexToRemove = widgets.FindIndex(widget => widget.ID == Widget.ID);

                if (indexToRemove != -1)
                {
                    UserPreferences preferences = UserPreferences.Load();
                    preferences.ActiveWidgets[Widget.ID] = false;

                    preferences.Save();
                    updateCallback.Invoke();
                }
            }
        }
    }
}
