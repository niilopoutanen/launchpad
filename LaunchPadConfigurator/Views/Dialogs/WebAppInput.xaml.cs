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
    public sealed partial class WebAppInput : UserControl, IAppDialog
    {
        public AppShortcut Input { get; set; }

        public WebAppInput()
        {
            this.InitializeComponent();
        }

        public AppShortcut Get()
        {
            return Input;
        }

        private void InputChanged(object sender, TextChangedEventArgs e)
        {
            Input = new()
            {
                Name = NameInput.Text,
                ExeUri = UrlInput.Text
            };
        }
    }
}
