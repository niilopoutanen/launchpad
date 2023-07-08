using LaunchPadCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;

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
            AppNameInput.TextChanged += (s, e) =>
            {
                InputChanged(AppNameInput.Text, null, null);
            };
            UrlInput.TextChanged += (s, e) =>
            {
                InputChanged(null, UrlInput.Text, null);
            };
        }
        public AppInputDialog(int type)
        {
            this.InitializeComponent();
            InputTypeComboBox.SelectedIndex = type;
            AppNameInput.TextChanged += (s, e) =>
            {
                InputChanged(AppNameInput.Text, null, null);
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
                Input.IconFileName = iconPath;
            }
        }
        public void Save()
        {
            SaveSystem.SaveApp(Input);
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
            if (Input.AppType == AppShortcut.AppTypes.EXE && !Directory.Exists(Input.ExeUri))
            {
                LocalFileInputError.Visibility = Visibility.Visible;
                valid = false;
            }

            return valid;
        }
    }
}
