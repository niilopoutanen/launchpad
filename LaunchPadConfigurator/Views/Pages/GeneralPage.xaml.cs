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
            ColumnCountSlider.Value = preferences.PreferredWidth;
            ColumnCountHeader.Text = "LaunchPad max width: " + preferences.PreferredWidth;

            ColumnCountSlider.ValueChanged += (s, e) =>
            {
                preferences = SaveSystem.LoadPreferences();
                preferences.PreferredWidth = (int)e.NewValue;
                SaveSystem.SavePreferences(preferences);

                ColumnCountHeader.Text = "LaunchPad max width: " + ColumnCountSlider.Value;
            };

            NameVisibleToggle.IsOn = preferences.NameVisible;
            NameVisibleToggle.Toggled += (s, e) =>
            {
                preferences = SaveSystem.LoadPreferences();
                preferences.NameVisible = ((ToggleSwitch)s).IsOn;
                SaveSystem.SavePreferences(preferences);
            };

            ThemeComboBox.ItemsSource = Enum.GetValues(typeof(UserPreferences.LaunchPadTheme));
            ThemeComboBox.SelectedItem = preferences.SelectedTheme;
            ThemeComboBox.SelectionChanged += (s, e) =>
            {
                preferences = SaveSystem.LoadPreferences();
                if (Enum.TryParse(ThemeComboBox.SelectedValue.ToString(), out UserPreferences.LaunchPadTheme selectedTheme))
                {
                    preferences.SelectedTheme = selectedTheme;
                    SaveSystem.SavePreferences(preferences);
                }
            };

            transparentThemeToggle.IsOn = preferences.TransparentTheme;
            transparentThemeToggle.Toggled += (s, e) =>
            {
                preferences = SaveSystem.LoadPreferences();
                preferences.TransparentTheme = ((ToggleSwitch)s).IsOn;
                SaveSystem.SavePreferences(preferences);
            };
        }
    }
}
