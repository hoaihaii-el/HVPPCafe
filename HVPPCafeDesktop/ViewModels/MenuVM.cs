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

        private ObservableCollection<CongThuc> _NguyenLieuCol = new ObservableCollection<CongThuc>();
        public ObservableCollection<CongThuc> NguyenLieuCol
        {
            get => _NguyenLieuCol;
            set => SetProperty(ref _NguyenLieuCol, value);
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

                GetNguyenLieu();

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
                ImagePath = "";

                GetNguyenLieu(MenuItem.MaMon);

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
                    EditMenu();
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

            SaveDetailCM = new RelayCommand<object>((p) => 
            {
                return true;
            }, (p) =>
            {
                if (string.IsNullOrEmpty(MenuItem.MaMon))
                {
                    var msg = new CustomMessageBox("Vui lòng thêm món trước!");
                    msg.ShowDialog();
                    return;
                }
                if (IsAnyNguyenLieuSelected() == false)
                {
                    var msg = new CustomMessageBox("Vui lòng chọn nguyên liệu cần thiết!");
                    msg.ShowDialog();
                    return;
                }
                if (IsValidNguyenLieu() == false)
                {
                    var msg = new CustomMessageBox("Định lượng phải lớn hơn 0!");
                    msg.ShowDialog();
                    return;
                }

                AddNguyenLieu(MenuItem.MaMon);

                LoadMenuCol();
            });
        }

        private async void AddNguyenLieu(string maMon)
        {
            foreach (var nl in NguyenLieuCol)
            {
                if (nl.DinhLuong <= 0) continue;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

                    var ctm = new
                    {
                        TenNguyenLieu = nl.TenSanPham,
                        MaMon = maMon,
                        DinhLuong = nl.DinhLuong
                    };

                    var json = JsonConvert.SerializeObject(ctm);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync("api/ChiTietMon", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var msg = new CustomMessageBox("Thêm nguyên liệu thành công!");
                        msg.ShowDialog();
                    }
                }
            }
        }

        public bool IsAnyNguyenLieuSelected()
        {
            foreach (var item in NguyenLieuCol)
            {
                if (item.DuocChon == true)
                    return true;
            }

            return false;
        }

        public bool IsValidNguyenLieu()
        {
            foreach (var item in NguyenLieuCol)
            {
                if (item.DuocChon && item.DinhLuong <= 0) return false;
            }
            return true;
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

        public async void GetNguyenLieu(string MaMon = "empty")
        {
            NguyenLieuCol.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/ChiTietMon/cong-thuc/{MaMon}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var menu = JsonConvert.DeserializeObject<List<CongThuc>>(result);

                    if (menu == null)
                    {
                        return;
                    }

                    foreach (var item in menu)
                    {
                        NguyenLieuCol.Add(item);
                    }
                }
            }
        }

        public async void AddTopping(string name, decimal gia)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

                var topping = new ToppingDTO
                {
                    TenTopping = name,
                    Gia = gia
                };

                var json = JsonConvert.SerializeObject(topping);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Mon/add-topping", content);

                if (response.IsSuccessStatusCode)
                {

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
                    var result = await response.Content.ReadAsStringAsync();
                    MenuItem.MaMon = JsonConvert.DeserializeObject<string>(result);
                    msgBox.TaskDone("Thêm thành công!");
                }
            }

            LoadMenuCol();
        }

        public async void EditMenu()
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

                var msgBox = new CustomMessageBox(true);
                msgBox.Show();

                if (!string.IsNullOrEmpty(ImagePath))
                {
                    byte[] bytes = File.ReadAllBytes(ImagePath);
                    var base64 = Convert.ToBase64String(bytes);
                    mon.AnhMonAn = base64;
                }
                else
                {
                    mon.AnhMonAn = "";
                }

                var json = JsonConvert.SerializeObject(mon);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"api/mon/{MenuItem.MaMon}", content);

                if (response.IsSuccessStatusCode)
                {
                    msgBox.TaskDone("Sửa thành công!");
                }
                else
                {
                    msgBox.TaskDone("Sửa thất bại!");
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
