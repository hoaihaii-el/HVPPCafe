﻿using System.Windows;
using System.Windows.Input;

namespace HVPPCafeDesktop.Views.SubViews
{
    /// <summary>
    /// Interaction logic for KhuyenMaiThem.xaml
    /// </summary>
    public partial class KhuyenMaiThem : Window
    {
        public KhuyenMaiThem()
        {
            InitializeComponent();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
