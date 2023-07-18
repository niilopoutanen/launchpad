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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using LaunchPadCore.Models;

namespace LaunchPadConfiguratorWPF.Views.Pages
{
    public partial class GeneralPage : Page
    {
        private UserPreferences preferences;
        public GeneralPage()
        {
            preferences = UserPreferences.Load();
            InitializeComponent();
            InitializeElements();
        }
        private void InitializeElements()
        {
            ColumnCountSlider.Value = preferences.PreferredWidth;
            ColumnCountHeader.Text = "LaunchPad max width: " + preferences.PreferredWidth + "px";

            ColumnCountSlider.ValueChanged += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.PreferredWidth = (int)e.NewValue;
                preferences.Save();

                ColumnCountHeader.Text = "LaunchPad max width: " + (int)e.NewValue + "px";
            };

            NameVisibleToggle.IsChecked = preferences.NameVisible;
            NameVisibleToggle.Checked += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.NameVisible = e;
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

            TransparentThemeToggle.IsChecked = preferences.TransparentTheme;
            TransparentThemeToggle.Checked += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.TransparentTheme = e;
                preferences.Save();
            };

            AccentThemeToggle.IsChecked = preferences.UseSystemAccent;
            AccentThemeToggle.Checked += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.UseSystemAccent = e;
                preferences.Save();
            };

            FullSizeIconToggle.IsChecked = preferences.FullSizeIcon;
            FullSizeIconToggle.Checked += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.FullSizeIcon = e;
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

            ThemedWidgetsToggle.IsChecked = preferences.ThemedWidgets;
            ThemedWidgetsToggle.Checked += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.ThemedWidgets = e;
                preferences.Save();
            };

            RememberWidgetVariationToggle.IsChecked = preferences.RememberWidgetVariation;
            RememberWidgetVariationToggle.Checked += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.RememberWidgetVariation = e;
                preferences.Save();
            };

            AnimationSpeedSlider.Value = preferences.AnimationSpeed;
            AnimationSpeedheader.Text = "Animation speed: " + AnimationSpeedSlider.Value.ToString("0.0");
            AnimationSpeedSlider.ValueChanged += (s, e) =>
            {
                preferences = UserPreferences.Load();
                preferences.AnimationSpeed = e.NewValue;
                preferences.Save();

                AnimationSpeedheader.Text = "Animation speed: " + AnimationSpeedSlider.Value.ToString("0.0");
            };
        }
    }
}
