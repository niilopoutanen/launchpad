using LaunchPadCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;

namespace LaunchPadConfigurator.Views.UIElements
{
    public sealed partial class AddAppDialog : UserControl
    {
        public string AppName { get; set; }
        public string IconPath { get; set; }
        public string ExePath { get; set; }

        private readonly IntPtr hWnd;
        public AddAppDialog(IntPtr hwnd)
        {
            this.InitializeComponent();
            this.hWnd = hwnd;

            appNameInput.TextChanged += (s, e) =>
            {
                AppName = ((TextBox)s).Text;
            };
        }

        public void UpdateApp(AppShortcut app)
        {
            AppName = app.Name;
            IconPath = app.GetIconFullPath();
            ExePath = app.ExeUri;
            appNameInput.Text = AppName;
        }

        private async void IconPathProvided(object sender, RoutedEventArgs e)
        {
            var appIconPicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            appIconPicker.FileTypeFilter.Add(".jpg");
            appIconPicker.FileTypeFilter.Add(".jpeg");
            appIconPicker.FileTypeFilter.Add(".png");
            appIconPicker.FileTypeFilter.Add(".ico");

            WinRT.Interop.InitializeWithWindow.Initialize(appIconPicker, hWnd);

            var file = await appIconPicker.PickSingleFileAsync();
            if (file != null)
            {
                IconPath = file.Path;
            }
        }
        private async void ExePathProvided(object sender, RoutedEventArgs e)
        {
            var appExePicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            appExePicker.FileTypeFilter.Add("*");

            WinRT.Interop.InitializeWithWindow.Initialize(appExePicker, hWnd);

            var file = await appExePicker.PickSingleFileAsync();
            if (file != null)
            {
                ExePath = file.Path;
            }
        }

        public bool InputsAreValid()
        {
            //Clear existing messages
            appNameInputError.Visibility = Visibility.Collapsed;
            appExeInputError.Visibility = Visibility.Collapsed;

            if (AppName != null && ExePath != null)
            {
                return true;
            }
            else
            {
                if (string.IsNullOrEmpty(AppName))
                {
                    appNameInputError.Visibility = Visibility.Visible;
                }
                if (string.IsNullOrEmpty(ExePath))
                {
                    appExeInputError.Visibility = Visibility.Visible;
                }
            }
            return false;
        }
    }
}
