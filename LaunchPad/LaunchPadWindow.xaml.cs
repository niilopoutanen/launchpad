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

            AddApp("C:/Windows/system32/notepad.exe");
            AddApp("C:/Program Files/Google/Chrome/Application/chrome.exe");
            AddApp("C:/Users/niilo/AppData/Roaming/Spotify/Spotify.exe");

            //Remove last exess gap
            appContainer.Children.RemoveAt(appContainer.Children.Count - 1);
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void AddApp(string appURI)
        {
            var icon = new Icon(appURI, CloseWithAnim);
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
