using LaunchPadClassLibrary;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaunchPadConfigurator
{
    public sealed partial class GeneralPage : Page
    {
        UserPreferences preferences;
        public GeneralPage()
        {
            preferences = SaveSystem.LoadPreferences();

            this.InitializeComponent();
            this.InitializeElements();
        }
        private void InitializeElements()
        {
            ColumnCountSlider.Value = preferences.ColumnCount;
            ColumnCountHeader.Text = "LaunchPad column count: " + preferences.ColumnCount;

            ColumnCountSlider.ValueChanged += (s, e) =>
            {
                preferences = SaveSystem.LoadPreferences();
                preferences.ColumnCount = (int)e.NewValue;
                SaveSystem.SavePreferences(preferences);

                ColumnCountHeader.Text = "LaunchPad column count: " + ColumnCountSlider.Value;
            };
        }
    }
}
