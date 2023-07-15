using LaunchPadCore.Common.Controls;
using LaunchPadCore.Common.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LaunchPad
{
    public partial class Icon : LaunchPadItemControl
    {
        public AppShortcut App { get; set; }
        public override bool Pressed { get; set; }
        public override bool Focused { get; set; }
        public override bool WaitForAnim => true;
        public override bool HasSecondaryAction => false;

        public override FrameworkElement BaseElement => appIcon;
        public override TextBlock ItemName => VisualName;
        public override UserPreferences Preferences { get; set; }

        public Icon(AppShortcut app)
        {
            this.App = app;
            InitializeComponent();
            base.InitializeControl();

            InitializeIcon();
        }


        private void InitializeIcon()
        {
            var icon = AppShortcut.GetIcon(App);
            if (icon != null)
            {
                iconBitmap.Source = icon;
            }
            else
            {
                iconBitmap.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Assets/icon_null.png", UriKind.Absolute));
            }


            if (Preferences.FullSizeIcon)
            {
                appIcon.Padding = new Thickness(0);
                appIcon.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
            }

            if (Preferences.NameVisible)
            {
                VisualName.Visibility = Visibility.Visible;
                appIcon.Width = 80;
                appIcon.Height = 80;


                if (App.Name.Length > 13)
                {
                    VisualName.Text = App.Name[..13] + "..";
                }

                else
                {
                    VisualName.Text = App.Name;
                }
            }


        }

        public override async Task OnClick()
        {
            await Task.Run(() =>
            {
                try
                {
                    Process process = new();
                    if (App.AppType == AppShortcut.AppTypes.MS_STORE)
                    {
                        Process.Start("explorer.exe", "shell:appsfolder\\" + App.ExeUri + "!App");
                    }
                    else
                    {
                        process.StartInfo.FileName = App.ExeUri;
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                    }
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
            if (!Preferences.FullSizeIcon)
            {
                appIcon.Background = itemBackgroundColor;
            }
            VisualName.Foreground = textColor;

        }
    }
}
