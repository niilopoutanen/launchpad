using LaunchPadCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
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
            InitializeEvents();

        }
        public AddAppDialog(IntPtr hwnd, AppShortcut appToUpdate)
        {
            this.InitializeComponent();
            this.hWnd = hwnd;
            InitializeEvents();
            AppName = appToUpdate.Name;
            IconPath = appToUpdate.GetIconFullPath();
            ExePath = appToUpdate.ExeUri;

            SetFields();
        }
        private async void SetFields()
        {
            appNameInput.Text = AppName;
            previewName.Text = AppName;

            previewPath.Text = Path.GetFileName(ExePath);
            if(IconPath != null)
            {
                if (Uri.TryCreate(IconPath, UriKind.Absolute, out Uri validUri))
                {
                    BitmapImage bitmapImage = new(validUri);
                    previewIcon.Source = bitmapImage;
                }
            }
            else
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(ExePath);
                StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.SingleItem);

                BitmapImage imageSource = new();
                imageSource.SetSource(thumbnail);

                previewIcon.Source = imageSource;
            }

        }
        private void InitializeEvents()
        {
            appNameInput.TextChanged += (s, e) =>
            {
                AppName = ((TextBox)s).Text;
                previewName.Text = AppName;
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
                    BitmapImage bitmapImage = new(validUri);
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
