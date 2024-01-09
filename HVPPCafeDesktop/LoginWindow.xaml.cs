using HVPPCafeDesktop.CustomControl;
using Model = HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HVPPCafeDesktop
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName.Text) || string.IsNullOrEmpty(Passw.Password))
            {
                var msg = new CustomMessageBox("Không được để trống!");
                msg.ShowDialog();
                return;
            }

            using (var client = new HttpClient())
            {
                this.Hide();
                var msg = new CustomMessageBox(true);
                msg.Show();
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/TaiKhoan/{UserName.Text}/{Passw.Password}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var tk = JsonConvert.DeserializeObject<Model.TaiKhoan>(result);
                    var logout = false;

                    msg.TaskDone("Đăng nhập thành công!");

                    if (tk.Quyen.ToLower() == "admin")
                    {
                        var mainW = new MainWindow(true);
                        mainW.ShowDialog();
                        logout = mainW.isLogout;
                    }
                    else
                    {
                        var mainW = new MainWindow();
                        mainW.ShowDialog();
                        logout = mainW.isLogout;
                    }
                    if (logout)
                    {
                        this.Show();
                    }
                    else { this.Close(); }
                }
                else
                {
                    var msg1 = new CustomMessageBox("Sai tên đăng nhập hoặc mật khẩu!");
                    msg1.ShowDialog();
                }
            }
        }
    }
}
