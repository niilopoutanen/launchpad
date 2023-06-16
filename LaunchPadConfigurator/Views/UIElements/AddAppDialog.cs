using LaunchPadClassLibrary;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace LaunchPadConfigurator.Views.UIElements
{
    internal class AddAppDialog
    {
        public string IconPath { get; set; }
        public string ExePath { get; set; }
        public string Name { get; set; }

        private XamlRoot xamlRoot;
        private Action updateHandler;
        public AddAppDialog(XamlRoot xamlRoot, Action updateHandler) 
        {
            this.xamlRoot = xamlRoot;
            this.updateHandler = updateHandler;
        }

        public async Task Show()
        {
            var window = (Application.Current as App)?.Window;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            var dialog = new ContentDialog();
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Add a new app";
            dialog.XamlRoot = xamlRoot;
            dialog.PrimaryButtonText = "Save";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;

            var stackPanel = new StackPanel();

            var appNameLabel = new TextBlock()
            {
                Text = "App Name:",
                Margin = new Thickness(0,10,0,5)
            };

            var appNameTextBox = new TextBox()
            {
                PlaceholderText = "Enter the app name"
            };

            stackPanel.Children.Add(appNameLabel);
            stackPanel.Children.Add(appNameTextBox);

            var executableFileLabel = new TextBlock()
            {
                Text = "App executable / shortcut:",
                Margin = new Thickness(0, 10, 0, 5)
            };

            var executableFilePicker = new FileOpenPicker();
            executableFilePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            executableFilePicker.FileTypeFilter.Add("*");

            var executableFileButton = new Button()
            {
                Content = "Select app"
            };

            executableFileButton.Click += async (s, args) =>
            {
                // Retrieve the window handle (HWND) of the current WinUI 3 window.
                var window = (Application.Current as App)?.Window;
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

                // Initialize the file picker with the window handle (HWND).
                WinRT.Interop.InitializeWithWindow.Initialize(executableFilePicker, hWnd);

                var file = await executableFilePicker.PickSingleFileAsync();
                if (file != null)
                {
                    executableFileButton.Content = file.DisplayName;
                    ExePath = file.Path;
                }
            };

            stackPanel.Children.Add(executableFileLabel);
            stackPanel.Children.Add(executableFileButton);

            var appIconLabel = new TextBlock()
            {
                Text = "App icon. Leave empty for autofill",
                Margin = new Thickness(0, 10, 0, 5)
            };

            var appIconPicker = new FileOpenPicker();
            appIconPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            appIconPicker.FileTypeFilter.Add("*");

            var appIconButton = new Button()
            {
                Content = "Select icon"
            };

            appIconButton.Click += async (s, args) =>
            {
                // Retrieve the window handle (HWND) of the current WinUI 3 window.


                // Initialize the file picker with the window handle (HWND).
                WinRT.Interop.InitializeWithWindow.Initialize(appIconPicker, hWnd);

                var file = await appIconPicker.PickSingleFileAsync();
                if (file != null)
                {
                    appIconButton.Content = file.DisplayName;
                    IconPath = file.Path;
                }
            };

            stackPanel.Children.Add(appIconLabel);
            stackPanel.Children.Add(appIconButton);

            dialog.Content = stackPanel;

            dialog.PrimaryButtonClick += (_s, _e) =>
            {
                Name = appNameTextBox.Text;

                AppShortcut app = new AppShortcut(Name, ExePath, IconPath);
                SaveSystem.SaveApp(app);
                updateHandler.Invoke();
            };

            await dialog.ShowAsync();
        }
    }
}
