using HVPPCafeDesktop.CustomControl;
using HVPPCafeDesktop.DTO;
using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using HVPPCafeDesktop.Views.SubViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace HVPPCafeDesktop.ViewModels
{
    class MenuVM : BaseViewModel
    {
        private ObservableCollection<Mon> _MenuCol = new ObservableCollection<Mon>();
        public ObservableCollection<Mon> MenuCol
        {
            get => _MenuCol;
            set => SetProperty(ref _MenuCol, value);
        }

        private Mon _MenuSelected;
        public Mon MenuSelected
        {
            get => _MenuSelected;
            set => SetProperty(ref _MenuSelected, value);
        }

        private Mon _MenuItem = new Mon();
        public Mon MenuItem
        {
            get => _MenuItem;
            set => SetProperty(ref _MenuItem, value);
        }

        public ICommand NewMenuCM { get; set; }
        public ICommand EditDetailCM { get; set; }
        public ICommand SaveMenuCM { get; set; }
        public ICommand SaveDetailCM { get; set; }

        private bool _IsEditMenu = false;
        public static string? ImagePath;

        public MenuVM()
        {
            LoadMenuCol();

            NewMenuCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                _IsEditMenu = false;
                MenuItem = new Mon();
                MenuItem.Nhom = "Trà trái cây";

                var addWindow = new MenuThem();
                addWindow.DataContext = this;
                addWindow.ShowDialog();
            });

            EditDetailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (MenuSelected == null)
                {
                    var msgBox = new CustomMessageBox("Vui lòng chọn món cần sửa ở bảng!");
                    msgBox.ShowDialog();
                    return;
                }

                _IsEditMenu = true;
                MenuItem = MenuSelected;

                var editWindow = new MenuThem(true);
                editWindow.DataContext = this;
                editWindow.ShowDialog();
            });

            SaveMenuCM = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MenuItem.TenMon) || MenuItem.GiaBan < 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                NewMenu();
            });
        }

        public async void LoadMenuCol()
        {
            MenuCol.Clear();

            var client = new HttpClient();
            client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

            var response = await client.GetAsync("api/Mon");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var menu = JsonConvert.DeserializeObject<List<Mon>>(result);

                if (menu == null)
                {
                    return;
                }

                foreach (var item in menu)
                {
                    MenuCol.Add(item);
                }
            }
        }

        public async void NewMenu()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

            var mon = new MonDTO
            {
                TenMon = MenuItem.TenMon,
                GiaBan = MenuItem.GiaBan,
                Nhom = MenuItem.Nhom,
            };

            if (string.IsNullOrEmpty(ImagePath))
            {
                var msgBox = new CustomMessageBox("Hãy chọn hình ảnh!");
                msgBox.ShowDialog();
                return;
            }

            byte[] bytes = File.ReadAllBytes(ImagePath);
            var base64 = Convert.ToBase64String(bytes);

            mon.AnhMonAn = base64;

            var json = JsonConvert.SerializeObject(mon);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Mon", content);

            if (response.IsSuccessStatusCode)
            {
                var msgBox = new CustomMessageBox("Thêm thành công!");
                msgBox.ShowDialog();
            }
        }
    }
}
