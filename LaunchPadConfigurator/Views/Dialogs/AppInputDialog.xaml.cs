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
        private IAppDialog activeDialog;
        public AppInputDialog()
        {
            this.InitializeComponent();
            InitializeInputControl(0);
        }
        private void InitializeInputControl(int type)
        {
            switch (type)
            {
                default:
                    activeDialog = new StoreAppInput();
                    break;
                case TYPE_EXE:
                    activeDialog = new LocalAppInput();
                    break;
                case TYPE_URL:
                    activeDialog = new WebAppInput();
                    break;
            }
            
            InputFrame.Content = activeDialog;
        }
        public AppShortcut GetInput()
        {
            return activeDialog.Get();
        }

        private void AppTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeInputControl(InputTypeComboBox.SelectedIndex);
        }
    }
}
