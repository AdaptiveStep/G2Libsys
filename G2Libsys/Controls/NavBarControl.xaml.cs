﻿using System;
using System.Collections.Generic;
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

namespace G2Libsys.Controls
{
    /// <summary>
    /// Interaction logic for NavBarControl.xaml
    /// </summary>
    public partial class NavBarControl : UserControl
    {
        public NavBarControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.MainWindow;
            window.Close();
        }
    }
}
