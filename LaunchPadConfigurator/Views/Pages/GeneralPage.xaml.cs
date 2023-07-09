using LaunchPadCore.Models;
using Microsoft.UI.Xaml.Controls;
using System;

namespace LaunchPadConfigurator.Views.Pages
{
    public sealed partial class GeneralPage : Page
    {
        UserPreferences preferences;
        public GeneralPage()
        {
            preferences = UserPreferences.Load();

            this.InitializeComponent();
            this.InitializeElements();
        }
        private void InitializeElements()
        {
            ColumnCountSlider.Value = preferences.PreferredWidth;
            ColumnCountHeader.Text = "LaunchPad max width: " + preferences.PreferredWidth;

            ColumnCountSlider.ValueChanged += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.PreferredWidth = (int)e.NewValue;
                preferences.Save();

                ColumnCountHeader.Text = "LaunchPad max width: " + ColumnCountSlider.Value;
            };

            NameVisibleToggle.IsOn = preferences.NameVisible;
            NameVisibleToggle.Toggled += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.NameVisible = ((ToggleSwitch)s).IsOn;
                preferences.Save();
            };

            ThemeComboBox.ItemsSource = Enum.GetValues(typeof(UserPreferences.LaunchPadTheme));
            ThemeComboBox.SelectedItem = preferences.SelectedTheme;
            ThemeComboBox.SelectionChanged += (s, e) =>
            {
                preferences = UserPreferences.Load();
                if (Enum.TryParse(ThemeComboBox.SelectedValue.ToString(), out UserPreferences.LaunchPadTheme selectedTheme))
                {
                    preferences.SelectedTheme = selectedTheme;
                    preferences.Save();
                }
            };

            transparentThemeToggle.IsOn = preferences.TransparentTheme;
            transparentThemeToggle.Toggled += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.TransparentTheme = ((ToggleSwitch)s).IsOn;
                preferences.Save();
            };

            accentThemeToggle.IsOn = preferences.UseSystemAccent;
            accentThemeToggle.Toggled += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.UseSystemAccent = ((ToggleSwitch)s).IsOn;
                preferences.Save();
            };

            fullSizeIconToggle.IsOn = preferences.FullSizeIcon;
            fullSizeIconToggle.Toggled += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.FullSizeIcon = ((ToggleSwitch)s).IsOn;
                preferences.Save();
            };

            AnimationComboBox.ItemsSource = Enum.GetValues(typeof(UserPreferences.AnimationTypes));
            AnimationComboBox.SelectedItem = preferences.SelectedAnimation;
            AnimationComboBox.SelectionChanged += (s, e) =>
            {
                preferences = UserPreferences.Load();
                if (Enum.TryParse(AnimationComboBox.SelectedValue.ToString(), out UserPreferences.AnimationTypes selectedAnimation))
                {
                    preferences.SelectedAnimation = selectedAnimation;
                    preferences.Save();
                }
            };

            themedWidgetsToggle.IsOn = preferences.ThemedWidgets;
            themedWidgetsToggle.Toggled += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.ThemedWidgets = ((ToggleSwitch)s).IsOn;
                preferences.Save();
            };

            rememberWidgetVariationToggle.IsOn = preferences.RememberWidgetVariation;
            rememberWidgetVariationToggle.Toggled += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.RememberWidgetVariation = ((ToggleSwitch)s).IsOn;
                preferences.Save();
            };
        }
    }
}
