using LaunchPadCore.Models;
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
using LaunchPadCore.Common.Controls;
using Microsoft.Win32;
using System.IO;

namespace LaunchPadConfigurator.Views.Windows
{
    public partial class AppDialog : BaseWindow
    {
        private readonly AppShortcut input = new();
        public EventHandler<AppShortcut?>? completed;

        public AppDialog(AppShortcut.AppTypes type)
        {
            this.input.AppType = type;
            InitializeComponent();
            InitializeInput();
            Cancel.Click += (s, e) =>
            {
                this.Close();
            };
            this.MouseLeftButtonDown += (s, e) =>
            {
                this.DragMove();
            };
        }
        public void InitializeInput()
        {
            AppName.TextChanged += (s, e) =>
            {
                input.Name = AppName.Text;
            };
            if(input.AppType == AppShortcut.AppTypes.EXE)
            {
                LocalFileInput.Visibility = Visibility.Visible;
                UrlInput.Visibility = Visibility.Collapsed;
                ExeButton.Click += (s, e) =>
                {
                    string result = ShowFileDialog(null);
                    input.ExeUri = result;
                };
            }
            else if (input.AppType == AppShortcut.AppTypes.URL)
            {
                UrlInput.Visibility = Visibility.Visible;
                LocalFileInput.Visibility = Visibility.Collapsed;
                Url.TextChanged += (s, e) =>
                {
                    input.ExeUri = Url.Text;
                };
            }

            IconButton.Click += (s, e) =>
            {
                string result = ShowFileDialog("Image files(*.png; *.jpeg; *.jpg; *.ico)| *.png; *.jpeg; *.jpg; *.ico");
                if (result != String.Empty)
                {
                    input.IconFileName = result;
                    IconButton.Content = Path.GetFileName(result);
                }
            };

            Add.Click += async (s,e) =>
            {
                AppShortcut? inp = await Get();
                if(inp == null)
                {
                    return;
                }
                completed?.Invoke(this, inp);
            };
        }
        private static string ShowFileDialog(string? filter)
        {
            OpenFileDialog fileDialog = new();
            if (filter != null) fileDialog.Filter = filter;

            if(fileDialog.ShowDialog()  == true )
            {
                return fileDialog.FileName;
            }
            else
            {
                return String.Empty;
            }
        }
        public bool InputsAreValid()
        {
            ClearErrors();

            if (String.IsNullOrEmpty(AppName.Text)|| String.IsNullOrEmpty(input.Name)) 
            {
                ShowError(NameError, "App name cannot be empty.");
                return false;
            }

            if(!String.IsNullOrEmpty(input.IconFileName) && !File.Exists(input.IconFileName))
            {
                ShowError(IconError, "Icon file is not valid");
                return false;
            }

            if (String.IsNullOrEmpty(input.ExeUri))
            {
                return false;
            }
            if (input.AppType == AppShortcut.AppTypes.EXE && !File.Exists(input.ExeUri))
            {
                return false;
            }

            return true;
        }
        private void ShowError(TextBlock target, string message)
        {
            target.Visibility = Visibility.Visible;
            target.Text = message;
        }
        private void ClearErrors()
        {
            NameError.Visibility = Visibility.Collapsed;
            FileError.Visibility = Visibility.Collapsed;
            UrlError.Visibility = Visibility.Collapsed;
            IconError.Visibility = Visibility.Collapsed;
        }
        private async Task<AppShortcut?> Get()
        {
            if (!InputsAreValid())
            {
                return null;
            }
            await Complete();
            return input;
        }
        public async Task Complete()
        {
            DoubleAnimation fadeInAnimation = new(0, 1, TimeSpan.FromSeconds(0.1));
            Completed.Visibility = Visibility.Visible;
            Completed.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);

            await Task.Delay(1000);

            this.Close();
        }
    }
}
