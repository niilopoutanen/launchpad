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
using System.Windows.Shapes;
using LaunchPadCore.Common.Controls;

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
            }
            else if (input.AppType == AppShortcut.AppTypes.URL)
            {
                UrlInput.Visibility = Visibility.Visible;
                LocalFileInput.Visibility = Visibility.Collapsed;
            }

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
        public bool InputsAreValid()
        {
            if (String.IsNullOrEmpty(AppName.Text)) 
            {
                return false;
            }
            return true;
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
