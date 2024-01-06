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
using System.Linq;
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

        private bool _IsHaveSizeL;
        public bool IsHaveSizeL
        {
            get => _IsHaveSizeL;
            set => SetProperty(ref _IsHaveSizeL, value);
        }

        private bool _IsHaveSizeXL;
        public bool IsHaveSizeXL
        {
            get => _IsHaveSizeXL;
            set => SetProperty(ref _IsHaveSizeXL, value);
        }

        private string _TyLeL;
        public string TyLeL
        {
            get => _TyLeL;
            set
            {
                _TyLeL = value;
                OnPropertyChanged();
                try
                {
                    MenuItem.TyLeL = double.Parse(TyLeL);
                }
                catch
                {

                }
            }
        }

        private string _TyLeXL;
        public string TyLeXL
        {
            get => _TyLeXL;
            set
            {
                _TyLeXL = value;
                OnPropertyChanged();
                try
                {
                    MenuItem.TyLeXL = double.Parse(TyLeXL);
                }
                catch
                {

                }
            }
        }

        public ICommand NewMenuCM { get; set; }
        public ICommand EditDetailCM { get; set; }
        public ICommand SaveMenuCM { get; set; }
        public ICommand SaveDetailCM { get; set; }
        public ICommand DeleteCM { get; set; }

        private bool _IsEditMenu = false;
        public static string? ImagePath;
        public List<Mon> NewMon = new List<Mon>();
        public List<Mon> EditMon = new List<Mon>();

        public MenuVM()
        {
            LoadMenuCol();

            NewMenuCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                _IsEditMenu = false;
                MenuItem = new Mon();
                MenuItem.Nhom = "Trà trái cây";
                MenuItem.GiaBanM = 0;
                MenuItem.GiaBanL = 0;
                MenuItem.GiaBanXL = 0;
                MenuItem.TyLeL = 0;
                MenuItem.TyLeXL = 0;
                IsHaveSizeL = false;
                IsHaveSizeXL = false;

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
                IsHaveSizeL = MenuSelected.GiaBanL > 0;
                IsHaveSizeXL = MenuSelected.GiaBanXL > 0;
                TyLeL = MenuSelected.TyLeL.ToString();
                TyLeXL = MenuSelected.TyLeXL.ToString();

                var editWindow = new MenuThem(true);
                editWindow.DataContext = this;
                editWindow.ShowDialog();
            });

            SaveMenuCM = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MenuItem.TenMon) || MenuItem.GiaBanM <= 0)
                {
                    return false;
                }
                if (IsHaveSizeL)
                {
                    if (MenuItem.GiaBanL <= 0 || !double.TryParse(TyLeL, out _)) return false;
                }
                if (IsHaveSizeXL)
                {
                    if (MenuItem.GiaBanXL <= 0 || !double.TryParse(TyLeXL, out _)) return false;
                }

                return true;
            },(p) =>
            {
                if (!_IsEditMenu)
                {
                    NewMenu();
                }
                else
                {

                }
            });

            DeleteCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (p is Mon)
                {
                    var mon = p as Mon;
                    DeleteMenu(mon?.MaMon ?? "");
                }

                LoadMenuCol();
            });
        }

        public async void LoadMenuCol()
        {
            MenuCol.Clear();
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/Mon/all");

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
        }

        public async void NewMenu()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

                var mon = new MonDTO
                {
                    TenMon = MenuItem.TenMon,
                    GiaBanM = MenuItem.GiaBanM,
                    GiaBanL = IsHaveSizeL ? MenuItem.GiaBanL : 0,
                    GiaBanXL = IsHaveSizeXL ? MenuItem.GiaBanXL : 0,
                    Nhom = MenuItem.Nhom,
                    TyLeL = IsHaveSizeL ? MenuItem.TyLeL : 0,
                    TyLeXL = IsHaveSizeXL ? MenuItem.TyLeXL : 0,
                };

                if (string.IsNullOrEmpty(ImagePath))
                {
                    var msgBox2 = new CustomMessageBox("Hãy chọn hình ảnh!");
                    msgBox2.ShowDialog();
                    return;
                }

                var msgBox = new CustomMessageBox(true);
                msgBox.Show();

                byte[] bytes = File.ReadAllBytes(ImagePath);
                var base64 = Convert.ToBase64String(bytes);

                mon.AnhMonAn = base64;

                var json = JsonConvert.SerializeObject(mon);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/Mon", content);

                if (response.IsSuccessStatusCode)
                {
                    msgBox.TaskDone("Thêm thành công!");
                }
            }

            LoadMenuCol();
        }

        public async void DeleteMenu(string _MaMon)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

                var response = await client.DeleteAsync($"api/Mon/{_MaMon}");

                if (response.IsSuccessStatusCode)
                {
                    var msgBox = new CustomMessageBox("Xóa thành công!");
                    msgBox.ShowDialog();
                }
                else
                {
                    var msgBox = new CustomMessageBox("Đã có lỗi xảy ra!");
                    msgBox.ShowDialog();
                }
            }            
        }
    }
}
