using HVPPCafeDesktop.ViewModels;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HVPPCafeDesktop.Views.SubViews
{
    /// <summary>
    /// Interaction logic for MenuThem.xaml
    /// </summary>
    public partial class MenuThem : Window
    {
        public MenuThem(bool edit = false)
        {
            InitializeComponent();

            if (edit)
            {
                Title.Content = "  SỬA MÓN ĂN";
                Button1.Content = "Sửa món ăn";
                Button2.Content = "Sửa ảnh món ăn";
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            openFileDialog.Title = "Thêm ảnh món ăn";

            if (openFileDialog.ShowDialog() == true)
            {
                MenuVM.ImagePath = openFileDialog.FileName;

                var bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.UriSource = new Uri(MenuVM.ImagePath);
                bmi.EndInit();

                ImageMenu.ImageSource = bmi;
            }
        }
    }
}
