using LaunchPadCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace LaunchPadConfigurator.Views.Dialogs
{
    public sealed partial class AppInputDialog : UserControl
    {
        private const int TYPE_MSSTORE = 0;
        private const int TYPE_EXE = 1;
        private const int TYPE_URL = 2;

        public AppShortcut Input { get; set; }
        public AppInputDialog()
        {
            this.InitializeComponent();
            InputTypeComboBox.SelectedIndex = 0;
            AppNameInput.TextChanged += (s, e) =>
            {
                InputChanged(AppNameInput.Text, null, null);
            };
        }
        private void InitializeInputControl(int type)
        {
            StoreFrame.Visibility = Visibility.Collapsed;
            WebAppInput.Visibility = Visibility.Collapsed;
            LocalAppInput.Visibility = Visibility.Collapsed;

            switch (type)
            {
                default:
                    StoreFrame.Content = new StoreAppInput(InputChanged);
                    StoreFrame.Visibility = Visibility.Visible;
                    break;
                case TYPE_EXE:
                    LocalAppInput.Visibility = Visibility.Visible;
                    break;
                case TYPE_URL:
                    WebAppInput.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void AppTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeInputControl(InputTypeComboBox.SelectedIndex);
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
                Input.ExeUri = name;
            }
            if (iconPath != null)
            {
                Input.IconFileName = name;
            }
        }
    }
}
