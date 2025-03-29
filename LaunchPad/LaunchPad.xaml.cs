﻿using Core;
using Core.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchPad
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            LoadTheme();
        }


        private void LoadTheme()
        {
            Theme theme = DbManager.LoadTheme();

        }
    }
}