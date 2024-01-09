using HVPPCafeDesktop.CustomControl;
using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using HVPPCafeDesktop.Views;
using HVPPCafeDesktop.Views.SubViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace HVPPCafeDesktop.ViewModels
{
    class KhuyenMaiVM : BaseViewModel
    {
        private ObservableCollection<KhuyenM> _ListKM = new ObservableCollection<KhuyenM>();
        public ObservableCollection<KhuyenM> ListKM
        {
            get => _ListKM;
            set => SetProperty(ref _ListKM, value);
        }

        private KhuyenM _KMItem = new KhuyenM();
        public KhuyenM KMItem
        {
            get => _KMItem;
            set => SetProperty(ref _KMItem, value);
        }

        public ICommand OpenAddCM { get; set; }
        public ICommand AddCM { get; set; }
        public ICommand EditCM { get; set; }
        public ICommand DeleteCM { get; set; }
        private bool isEdit = false;

        public KhuyenMaiVM()
        {
            GetKM();

            OpenAddCM = new RelayCommand<object>((p) => true, (p) =>
            {
                isEdit = false;
                KMItem = new KhuyenM();
                KMItem.NgayBatDau = DateTime.Now;
                KMItem.NgayKetThuc = DateTime.Now;
                KMItem.TrangThai = "Đang diễn ra";
                var window = new KhuyenMaiThem();
                window.DataContext = this;
                window.ShowDialog();
            });

            EditCM = new RelayCommand<KhuyenM>((p) => true, (p) =>
            {
                isEdit = true;
                KMItem = p;
                var window = new KhuyenMaiThem(true);
                window.DataContext = this;
                window.ShowDialog();
            });

            DeleteCM = new RelayCommand<KhuyenM>((p) => true, (p) =>
            {
                Delete(p.MaKhuyenMai ?? "");
            });

            AddCM = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(KMItem.TenKhuyenMai)) return false;
                if (KMItem.NgayBatDau > KMItem.NgayKetThuc) return false;
                if (KMItem.GiamGia == 0 || KMItem.MucApDung == 0) return false;
                return true;
            }, (p) =>
            {
                if (isEdit) Edit();
                else Add();
            });
        }

        private async void GetKM()
        {
            ListKM.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/KhuyenMai");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var menu = JsonConvert.DeserializeObject<List<KhuyenM>>(result);

                    if (menu == null)
                    {
                        return;
                    }

                    foreach (var item in menu)
                    {
                        ListKM.Add(item);
                    }
                }
            }
        }

        private async void Add()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var json = JsonConvert.SerializeObject(KMItem);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"api/KhuyenMai", content);

                if (response.IsSuccessStatusCode)
                {
                    GetKM();
                    var msg = new CustomMessageBox("Thêm thành công!");
                    msg.ShowDialog();
                }
                else
                {
                    var msg = new CustomMessageBox("Đã có lỗi xảy ra!");
                    msg.ShowDialog();
                }
            }
        }

        private async void Delete(string maKhuyenMai)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.DeleteAsync($"api/KhuyenMai/{maKhuyenMai}");

                if (response.IsSuccessStatusCode)
                {
                    GetKM();
                    var msg = new CustomMessageBox("Xóa thành công!");
                    msg.ShowDialog();
                }
                else
                {
                    var msg = new CustomMessageBox("Đã có lỗi xảy ra!");
                    msg.ShowDialog();
                }
            }
        }

        private async void Edit()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var json = JsonConvert.SerializeObject(KMItem);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"api/KhuyenMai", content);

                if (response.IsSuccessStatusCode)
                {
                    GetKM();
                    var msg = new CustomMessageBox("Sửa thành công!");
                    msg.ShowDialog();
                }
                else
                {
                    var msg = new CustomMessageBox("Đã có lỗi xảy ra!");
                    msg.ShowDialog();
                }
            }
        }
    }
}
