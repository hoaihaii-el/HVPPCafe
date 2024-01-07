using HVPPCafeDesktop.CustomControl;
using HVPPCafeDesktop.DTO;
using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using HVPPCafeDesktop.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static iTextSharp.text.pdf.AcroFields;
using SubView = HVPPCafeDesktop.Views.SubViews;

namespace HVPPCafeDesktop.ViewModels
{
    class OrderVM : BaseViewModel
    {
        private string _Search;
        public string Search
        {
            get => _Search;
            set
            {
                _Search = value;
                OnPropertyChanged();
                Filter(GroupSelected, Search);
            }
        }

        private string _GroupName;
        public string GroupName
        {
            get => _GroupName;
            set => SetProperty(ref _GroupName, value);
        }

        private string _GroupSelected;
        public string GroupSelected
        {
            get => _GroupSelected;
            set
            {
                _GroupSelected = value;
                OnPropertyChanged();
                Filter(GroupSelected, Search);
            }
        }

        private string _SubTotal;
        public string SubTotal
        {
            get => _SubTotal;
            set => SetProperty(ref _SubTotal, value);
        }

        public string ToDay => DateTime.Today.DayOfWeek.ToString() + ", " + DateTime.Now.ToString("dd/MM/yyyy");

        private ObservableCollection<Mon> _MenuCol = new ObservableCollection<Mon>();
        public ObservableCollection<Mon> MenuCol
        {
            get => _MenuCol;
            set => SetProperty(ref _MenuCol, value);
        }

        private ObservableCollection<NewOrder> _NewOrder = new ObservableCollection<NewOrder>();
        public ObservableCollection<NewOrder> NewOrder
        {
            get => _NewOrder;
            set => SetProperty(ref _NewOrder, value);
        }

        private ObservableCollection<Topping> _Toppings = new ObservableCollection<Topping>();
        public ObservableCollection<Topping> Toppings
        {
            get => _Toppings;
            set => SetProperty(ref _Toppings, value);
        }

        public List<Mon> MenuRaw { get; set; } = new List<Mon>();

        public ICommand OrderCM { get; set; }
        public ICommand OrderSizeM { get; set; }
        public ICommand OrderSizeL { get; set; }
        public ICommand OrderSizeXL { get; set; }
        public ICommand RemoveCM { get; set; }
        public ICommand DeleteAllCM { get; set; }

        public OrderVM()
        {
            GetMenu();
            GetToppings();
            CalculateTotal();

            OrderSizeM = new RelayCommand<Mon>((p) => true, (p) => AddToOrder(p, "M"));
            OrderSizeL = new RelayCommand<Mon>((p) => true, (p) => AddToOrder(p, "L"));
            OrderSizeXL = new RelayCommand<Mon>((p) => true, (p) => AddToOrder(p, "XL"));

            RemoveCM = new RelayCommand<NewOrder>((p) => true, (p) => RemoveFromOrder(p));
            DeleteAllCM = new RelayCommand<object>((p) => true, (p) =>
            {
                NewOrder.Clear();
                CalculateTotal();
            });
            OrderCM = new RelayCommand<object>((p) => true, (p) =>
            {
                CreateOrderAsync();
            });
        }

        private async void CreateOrderAsync()
        {
            int OrderID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var hoadon = new HoaDonDTO
                {
                    LoaiHoaDon = "Dùng tại chỗ",
                    TriGia = NewOrder.Select(o => o.GiaBan).Sum(),
                    NgayHoaDon = DateTime.Now,
                    MaNV = "",
                    MaKhuyenMai = "",
                    DaCheBien = false,
                    DaThanhToan = true,
                    SoBan = 0
                };

                var json = JsonConvert.SerializeObject(hoadon);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/HoaDon", content);

                if (!response.IsSuccessStatusCode)
                {
                    var msg = new CustomMessageBox("Đã có lỗi xảy ra!");
                    msg.ShowDialog();
                    return;
                }

                var result = await response.Content.ReadAsStringAsync();
                OrderID = JsonConvert.DeserializeObject<int>(result);
                try
                {
                    AddCTHD(OrderID);
                }
                catch
                {
                    var msg = new CustomMessageBox("Đã có lỗi xảy ra!");
                    msg.ShowDialog();
                    return;
                }

                var msg1 = new CustomMessageBox("Tạo đơn thành công!");
                msg1.ShowDialog();
            }
        }

        public async void AddCTHD(int OrderID)
        {
            foreach (var order in NewOrder)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                    var cthd = new
                    {
                        MaMon = order.MaMon,
                        Size = order.Size,
                        SoLuong = order.SoLuong,
                        SoHoaDon = OrderID
                    };

                    var json = JsonConvert.SerializeObject(cthd);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/HoaDon/add-cthd", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async void AddCTTopping(int CTHDID, NewOrder item)
        {
            if (item.toppings.Count <= 0) return;

            foreach (var topping in item.toppings)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                    var cttopping = new
                    {
                        ID = CTHDID,
                        TenTopping = topping.TenTopping
                    };

                    var json = JsonConvert.SerializeObject(cttopping);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/HoaDon/add-cttopping", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        private void RemoveFromOrder(NewOrder p)
        {
            NewOrder.RemoveAt(p.Index);
            p.SoLuong--;
            if (p.SoLuong > 0)
            {
                NewOrder.Insert(p.Index, p);
            }
            for (int i = 0; i < NewOrder.Count; i++)
            {
                NewOrder[i].Index = i;
            }
            CalculateTotal();
        }

        private void AddToOrder(Mon mon, string size)
        {
            var isContain = false;
            if (size == "L" && mon.GiaBanL <= 0) return;
            if (size == "XL" && mon.GiaBanXL <= 0) return;

            if (mon.Nhom == "Trà sữa")
            {
                NewOrder.Add(new NewOrder
                {
                    Index = NewOrder.Count,
                    MaMon = mon.MaMon,
                    TenMon = mon.TenMon,
                    Size = size,
                    SoLuong = 1,
                    GiaBan = size == "M" ? mon.GiaBanM
                            : size == "L" ? mon.GiaBanL : mon.GiaBanXL,
                    CoTopping = mon.Nhom == "Trà sữa"
                });
                CalculateTotal();
                return;
            }

            for (int index = 0; index < NewOrder.Count; index++)
            {
                if (NewOrder[index].MaMon == mon.MaMon && NewOrder[index].Size == size)
                {
                    isContain = true;
                    var item = NewOrder[index];
                    NewOrder.RemoveAt(index);
                    item.SoLuong++;
                    NewOrder.Insert(index, item);
                    break;
                }
            }

            if (!isContain)
            {
                NewOrder.Add(new NewOrder
                {
                    Index = NewOrder.Count,
                    MaMon = mon.MaMon,
                    TenMon = mon.TenMon,
                    Size = size,
                    SoLuong = 1,
                    GiaBan = size == "M" ? mon.GiaBanM
                            : size == "L" ? mon.GiaBanL : mon.GiaBanXL,
                    CoTopping = mon.Nhom == "Trà sữa"
                });
            }
            CalculateTotal();
        }

        public async void GetMenu()
        {
            MenuRaw.Clear();
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
                        MenuRaw.Add(item);
                        MenuCol.Add(item);
                    }
                }
            }
        }

        public async void GetToppings()
        {
            Toppings.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/Mon/toppings");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var toppings = JsonConvert.DeserializeObject<ObservableCollection<Topping>>(result);

                    if (toppings == null) return;
                    Toppings = toppings;
                }
            }
        }

        public void ResetToppings()
        {
            foreach (var item in Toppings)
            {
                item.IsPicked = false;
            }
        }

        public void CalculateTotal()
        {
            decimal total = 0;
            foreach (var item in NewOrder)
            {
                var mon = MenuRaw.Where(m => m.MaMon == item.MaMon).FirstOrDefault();
                var dongia = item.Size == "M" ? mon.GiaBanM
                           : item.Size == "L" ? mon.GiaBanL : mon.GiaBanXL;
                total += dongia * item.SoLuong;
                item.GiaBan = item.SoLuong * dongia;
                foreach (var topping in item.toppings)
                {
                    total += topping.Gia;
                }
            }

            SubTotal = String.Format("{0:0,0 VND}", total);
        }

        public void Filter(string filter = "", string search = "")
        {
            MenuCol.Clear();

            if (!string.IsNullOrEmpty(filter))
            {
                var menu = filter == "Tất cả" ? MenuRaw
                : MenuRaw
                .Where(m => m.Nhom.ToLower() == filter.ToLower())
                .ToList();

                foreach (var item in menu)
                {
                    MenuCol.Add(item);
                }
            }

            if (string.IsNullOrEmpty(search)) return;
            var list = MenuCol
                .Where(m => m.TenMon.ToLower().Contains(search.ToLower()))
                .ToList();
            MenuCol.Clear();
            foreach (var item in list)
            {
                MenuCol.Add(item);
            }
        }
    }
}
