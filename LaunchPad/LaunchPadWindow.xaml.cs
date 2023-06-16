using LaunchPadClassLibrary;
using LaunchPadConfigurator;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LaunchPad
{
    public partial class LaunchPadWindow : Window
    {
        public LaunchPadWindow()
        {
            InitializeComponent();
            LoadApps();

        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
        private void LoadApps()
        {
            List<AppShortcut> apps = SaveSystem.LoadApps();
            foreach (AppShortcut app in apps)
            {
                AddApp(app.ExeUri, app.IconFileName);
            }
            if(apps.Count > 0)
            {
                //Remove last exess gap
                appContainer.Children.RemoveAt(appContainer.Children.Count - 1);
            }

        }
        private void AddApp(string appURI, string iconFile)
        {
            var icon = new Icon(appURI, iconFile, CloseWithAnim);
            var gap = new Border();
            gap.Width = 10;
            appContainer.Children.Add(icon);
            appContainer.Children.Add(gap);
        }

        public void CloseWithAnim()
        {
            Storyboard launchPadClose = ((Storyboard)this.FindResource("WindowExitAnimation")).Clone();
            launchPadClose.Completed += (s, e) =>
            {
                this.Close();
                
            };
            launchPadClose.Begin(this);
           
        }

    }
}
