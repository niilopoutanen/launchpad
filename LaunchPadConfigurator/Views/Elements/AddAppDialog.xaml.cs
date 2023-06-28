using LaunchPadCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;

namespace LaunchPadConfigurator.Views.UIElements
{
    public sealed partial class AddAppDialog : UserControl
    {
        public string AppName { get; set; }
        public string IconPath { get; set; }
        public string ExePath { get; set; }
        public AppShortcut.AppTypes AppType { get; set; }

        private readonly IntPtr hWnd;
        public AddAppDialog(IntPtr hwnd)
        {
            this.InitializeComponent();
            this.hWnd = hwnd;

            NameField.TextChanged += (s, e) =>
            {
                AppName = ((TextBox)s).Text;
            };
            URLField.TextChanged += (s, e) =>
            {
                ExePath = ((TextBox)s).Text;
            };

            AppTypeComboBox.SelectionChanged += (s, e) =>
            {
                ExePath = null;
                PathInputError.Visibility = Visibility.Collapsed;

                switch (AppTypeComboBox.SelectedIndex)
                {
                    case 0:
                        URLInput.Visibility = Visibility.Collapsed;
                        ExeInput.Visibility = Visibility.Visible;
                        AppType = AppShortcut.AppTypes.EXE;
                        break;

                    case 1:
                        URLInput.Visibility = Visibility.Visible;
                        ExeInput.Visibility = Visibility.Collapsed;
                        AppType = AppShortcut.AppTypes.URL;
                        break;
                }
            };
            AppTypeComboBox.SelectedIndex = 0;
            AppType = AppShortcut.AppTypes.EXE;
        }

        public void UpdateApp(AppShortcut app)
        {
            AppName = app.Name;
            IconPath = app.GetIconFullPath();
            ExePath = app.ExeUri;
            AppType = app.AppType;
            NameField.Text = AppName;
            AppTypeComboBox.SelectedItem = AppType;
            if(AppType == AppShortcut.AppTypes.URL)
            {
                URLInput.Visibility = Visibility.Visible;
                ExeInput.Visibility = Visibility.Collapsed;
                URLField.Text = ExePath;
            }
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
            PathInputError.Visibility = Visibility.Collapsed;
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
            NameInputError.Visibility = Visibility.Collapsed;
            PathInputError.Visibility = Visibility.Collapsed;

            if (AppName != null && ExePath != null)
            {
                return true;
            }
            else
            {
                if (string.IsNullOrEmpty(AppName))
                {
                    NameInputError.Visibility = Visibility.Visible;
                }
                if (string.IsNullOrEmpty(ExePath))
                {
                    PathInputError.Visibility = Visibility.Visible;
                }
            }
            return false;
        }

    }
}
