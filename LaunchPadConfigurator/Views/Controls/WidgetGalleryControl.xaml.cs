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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPadConfiguratorWPF.Views.Controls
{
    public partial class WidgetGalleryControl : UserControl
    {
        private readonly Widget widget;
        public WidgetGalleryControl(Widget widget)
        {
            this.widget = widget;
            InitializeComponent();
            InitializeElement();
        }
        private void InitializeElement()
        {
            if (this.widget == null)
            {
                return;
            }

            WidgetName.Text = widget.WidgetName;
            WidgetDescription.Text = widget.Description;
            WidgetIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Widgets/" + widget.IconFile));

            WidgetActive.Checked += (s, e) =>
            {
                widget.Active = e;

                WidgetActive.Content = e ? "Active" : "Inactive";

                widget.Save();
            };
            WidgetActive.IsChecked = widget.Active;
            WidgetActive.Content = widget.Active ? "Active" : "Inactive";

        }

    }
}
