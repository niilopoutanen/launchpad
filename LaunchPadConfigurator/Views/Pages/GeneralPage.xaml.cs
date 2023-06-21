using LaunchPadCore;
using Microsoft.UI.Xaml.Controls;
using System;

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


            fullSizeIconToggle.IsOn = preferences.FullSizeIcon;
            fullSizeIconToggle.Toggled += (s, e) =>
            {
                preferences = SaveSystem.LoadPreferences();
                preferences.FullSizeIcon = ((ToggleSwitch)s).IsOn;
                SaveSystem.SavePreferences(preferences);
            };
        }
    }
}
