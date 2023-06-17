using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.FileProperties;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaunchPadConfigurator.Views.UIElements
{
    public sealed partial class AddAppDialog : UserControl
    {
        public string AppName { get; set; }
        public string IconPath { get; set; }
        public string ExePath { get; set; }
        public bool FullSizeIcon { get; set; }

        private readonly IntPtr hWnd;
        public AddAppDialog(IntPtr hwnd)
        {
            this.InitializeComponent();
            this.hWnd = hwnd;

            appNameInput.TextChanged += (s, e) =>
            {
                AppName = ((TextBox)s).Text;
                previewName.Text = AppName;
            };

            appIconSizeToggle.Toggled += (s, e) =>
            {
                ToggleSwitch toggle = (ToggleSwitch)s;
                FullSizeIcon = toggle.IsOn;
                switch (toggle.IsOn)
                {
                    case true:
                        previewIconBg.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
                        previewIconBg.Padding = new Thickness(0);
                        break;
                    case false:
                        previewIconBg.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 61, 61, 61));
                        previewIconBg.Padding = new Thickness(5);
                        break;
                }
            };
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
                if (Uri.TryCreate(file.Path, UriKind.Absolute, out Uri validUri))
                {
                    BitmapImage bitmapImage = new BitmapImage(validUri);
                    previewIcon.Source = bitmapImage;
                }
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
                previewPath.Text = Path.GetFileName(file.Path);
                if(IconPath == null)
                {
                    StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.SingleItem);

                    BitmapImage imageSource = new();
                    imageSource.SetSource(thumbnail);

                    previewIcon.Source = imageSource;
                }

            }
        }
    }
}
