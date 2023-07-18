using LaunchPadConfiguratorWPF.Views.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPadConfiguratorWPF
{
    public partial class SideBar : UserControl
    {
        public EventHandler<string> HeaderChanged;
        public EventHandler<Page> PageChanged;

        public SideBar()
        {
            InitializeComponent();
            InitializeElements();
        }
        public void ChangeTab(int index)
        {
            HomeTab.Tag = "";
            GeneralTab.Tag = "";
            AppsTab.Tag = "";
            WidgetsTab.Tag = "";
            Page? page = null;
            switch (index)
            {
                case 0:
                    HomeTab.Tag = "active";
                    HeaderChanged(this, "Home");
                    page = new HomePage();
                    break;

                case 1:
                    GeneralTab.Tag = "active";
                    HeaderChanged.Invoke(this, "General");
                    page = new GeneralPage();
                    break;

                case 2:
                    AppsTab.Tag = "active";
                    HeaderChanged.Invoke(this, "Apps");
                    page = new AppsPage();
                    break;
                case 3:
                    WidgetsTab.Tag = "active";
                    HeaderChanged.Invoke(this, "Widgets");
                    page = new WidgetsPage();
                    break;
            }

            page ??= new HomePage();
            PageChanged.Invoke(this, page);
        }
        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new();
            animation.Duration = TimeSpan.FromSeconds(0.2);
            animation.EasingFunction = new QuadraticEase();
            if (SidebarContainer.Width == 200)
            {
                animation.To = 40;
                SetSmallLayout();
            }
            else
            {
                animation.To = 200;
                SetLargeLayout();
            }

            Storyboard.SetTarget(animation, SidebarContainer);
            Storyboard.SetTargetProperty(animation, new PropertyPath(WidthProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
        private void InitializeElements()
        {
            HomeTab.MouseLeftButtonUp += (s, e) =>
            {
                ChangeTab(0);
            };
            GeneralTab.MouseLeftButtonUp += (s, e) =>
            {
                ChangeTab(1);
            };
            AppsTab.MouseLeftButtonUp += (s, e) =>
            {
                ChangeTab(2);
            };
            WidgetsTab.MouseLeftButtonUp += (s, e) =>
            {
                ChangeTab(3);
            };
        }
        private void SetSmallLayout()
        {
            ResourceDictionary resourceDict = this.Resources;

            Style collapsedStyle = resourceDict["CollapsedSideBarItem"] as Style;

            HomeTab.Style = collapsedStyle;
            GeneralTab.Style = collapsedStyle;
            AppsTab.Style = collapsedStyle;
            WidgetsTab.Style = collapsedStyle;

            CollapseButton.Margin = new Thickness(5,0,0,0);
        }
        private void SetLargeLayout()
        {
            ResourceDictionary resourceDict = this.Resources;

            Style initialStyle = resourceDict["SideBarItem"] as Style;

            HomeTab.Style = initialStyle;
            GeneralTab.Style = initialStyle;
            AppsTab.Style = initialStyle;
            WidgetsTab.Style = initialStyle;

            CollapseButton.Margin = new Thickness(20, 0, 0, 0);
        }
    }
}
