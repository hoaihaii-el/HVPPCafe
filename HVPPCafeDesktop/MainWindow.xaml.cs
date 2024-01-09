using HVPPCafeDesktop.ViewModels;
using HVPPCafeDesktop.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace HVPPCafeDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(bool isAdmin = false)
        {
            InitializeComponent();
            this.DataContext = new NavigationVM();

            StartTime = DateTime.Now;

            if (isAdmin )
            {
                StaffNav.Visibility = Visibility.Hidden;
                AdminNav.Visibility = Visibility.Visible;
            }
            else
            {
                StaffNav.Visibility = Visibility.Visible;
                AdminNav.Visibility = Visibility.Hidden;
            }
        }

        public bool isLogout = false;
        public static double MainWX { get; set; }
        public static double MainWY { get; set; }
        public static string MaNV { get; set; }
        public static DateTime StartTime { get; set; } = DateTime.Now;

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            MainWX = this.Left;
            MainWY = this.Top;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var window = new TaiKhoan();
            window.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isLogout = true;
            this.Close();
        }
    }
}
