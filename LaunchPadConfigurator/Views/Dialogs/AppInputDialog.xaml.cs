using LaunchPadCore.Models;
using LaunchPadCore.Utility;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Windows.Storage.Pickers;

namespace LaunchPadConfigurator.Views.Dialogs
{
    public sealed partial class AppInputDialog : UserControl
    {
        public const int TYPE_MSSTORE = 0;
        public const int TYPE_EXE = 1;
        public const int TYPE_URL = 2;

        public AppShortcut Input { get; set; }
        public AppInputDialog()
        {
            Input = new();
            this.InitializeComponent();
            InputTypeComboBox.SelectedIndex = 0;
            InitializeEvents();
        }

        //for updating
        public AppInputDialog(AppShortcut app)
        {
            Input = app;
            this.InitializeComponent();
            InputTypeComboBox.SelectedIndex = (int)app.AppType;
            InitializeEvents();
            InputChanged(app.Name, app.ExeUri, app.GetIconFullPath());
            
        }

        private void InitializeEvents()
        {
            AppNameInput.TextChanged += (s, e) =>
            {
                InputChanged(AppNameInput.Text, null, null);
            };
            UrlInput.TextChanged += (s, e) =>
            {
                InputChanged(null, UrlInput.Text, null);
            };
            ExeButton.Click += async (s, e) =>
            {
                await ExePathRequest();
            };

            IconButton.Click += async (s, e) =>
            {
                await IconPathRequest();
            };
        }
        private void InitializeInputControl(AppShortcut.AppTypes type)
        {
            StoreFrame.Visibility = Visibility.Collapsed;
            WebAppInput.Visibility = Visibility.Collapsed;
            LocalAppInput.Visibility = Visibility.Collapsed;

            Input.AppType = type;
            switch (type)
            {
                default:
                    StoreFrame.Content = new StoreAppInput(InputChanged);
                    StoreFrame.Visibility = Visibility.Visible;
                    Input.AppType = AppShortcut.AppTypes.MS_STORE;
                    break;
                case AppShortcut.AppTypes.EXE:
                    LocalAppInput.Visibility = Visibility.Visible;
                    break;
                case AppShortcut.AppTypes.URL:
                    WebAppInput.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void AppTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeInputControl((AppShortcut.AppTypes)InputTypeComboBox.SelectedIndex);
        }
        public void InputChanged( string? name, string? path, string? iconPath)
        {
            Input ??= new();

            if (name != null)
            {
                Input.Name = name;
                AppNameInput.Text = name;
            }
            if (path != null)
            {
                Input.ExeUri = path;
            }
            if (iconPath != null)
            {
                Input.IconFileName = iconPath.Replace("%20", " ");
                IconImage.Source = new BitmapImage(new Uri(Input.IconFileName));
            }
        }
        public void Save()
        {
            AppShortcut appToSave = new AppShortcut(Input.Name, Input.ExeUri, Input.IconFileName, Input.AppType);
            SaveSystem.SaveApp(appToSave);
        }
        public void Update()
        {
            List<AppShortcut> existingApps = SaveSystem.LoadApps();
            foreach (AppShortcut appToCheck in existingApps)
            {
                if (appToCheck.ID == Input.ID)
                {
                    existingApps.Remove(appToCheck);
                    break;
                }
            }
            existingApps.Add(Input);
            SaveSystem.SaveApps(existingApps);
        }
        public bool ValidInputs()
        {
            bool valid = true;
            NameInputError.Visibility = Visibility.Collapsed;
            UrlInputError.Visibility = Visibility.Collapsed;
            LocalFileInputError.Visibility = Visibility.Collapsed;

            if (Input == null)
            {
                valid = false;
            }
            if (String.IsNullOrEmpty(Input.Name))
            {
                NameInputError.Visibility = Visibility.Visible;
                valid = false;
            }
            if (String.IsNullOrEmpty(Input.ExeUri))
            {
                LocalFileInputError.Visibility = Visibility.Visible;
                UrlInputError.Visibility = Visibility.Visible;
                valid = false;
            }
            if (Input.AppType == AppShortcut.AppTypes.EXE && !File.Exists(Input.ExeUri))
            {
                LocalFileInputError.Visibility = Visibility.Visible;
                valid = false;
            }

            return valid;
        }


        private async Task IconPathRequest()
        {
            var window = (Application.Current as App)?.Window;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

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
                InputChanged(null, null, file.Path);
            }
        }


        private async Task ExePathRequest()
        {
            var window = (Application.Current as App)?.Window;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            LocalFileInputError.Visibility = Visibility.Collapsed;
            var appExePicker = new FileOpenPicker();
            appExePicker.FileTypeFilter.Add("*");

            WinRT.Interop.InitializeWithWindow.Initialize(appExePicker, hWnd);

            var file = await appExePicker.PickSingleFileAsync();
            if (file != null)
            {
                Input.ExeUri = file.Path;
            }
        }
    }
}
