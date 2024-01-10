using HVPPCafeDesktop.CustomControl;
using HVPPCafeDesktop.Resources;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Model = HVPPCafeDesktop.Models;

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

        public static string MaNV { get; set; }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await TryLogin();
        }

        private async Task TryLogin()
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
                    MaNV = tk.MaNV ?? "Admin";
                    var logout = false;

                    msg.Close();

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
                        UserName.Text = "";
                        Passw.Password = "";
                        this.Show();
                    }
                    else { this.Close(); }
                }
                else
                {
                    msg.TaskDone("Sai tên đăng nhập hoặc mật khẩu!");
                }
            }
        }

        private async void Passw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await TryLogin();
            }
        }
    }
}
