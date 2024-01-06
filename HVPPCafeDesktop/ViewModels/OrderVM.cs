using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using HVPPCafeDesktop.Views.SubViews;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Input;

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

            OrderSizeM = new RelayCommand<Mon>((p) => true, (p) => AddToOrder(p, "M"));
            OrderSizeL = new RelayCommand<Mon>((p) => true, (p) => AddToOrder(p, "L"));
            OrderSizeXL = new RelayCommand<Mon>((p) => true, (p) => AddToOrder(p, "XL"));

            RemoveCM = new RelayCommand<NewOrder>((p) => true, (p) => RemoveFromOrder(p));
            DeleteAllCM = new RelayCommand<object>((p) => true, (p) =>
            {
                NewOrder.Clear();
            });
            OrderCM = new RelayCommand<object>((p) => true, (p) =>
            {
                var popup = new Topping();
                popup.ShowDialog();
            });
        }

        private void RemoveFromOrder(NewOrder p)
        {
            for (int i = 0; i < NewOrder.Count; i++)
            {
                if (NewOrder[i].MaMon == p.MaMon && NewOrder[i].Size == p.Size)
                {
                    NewOrder.RemoveAt(i);
                    p.SoLuong--;
                    if (p.SoLuong > 0)
                    {
                        NewOrder.Insert(i, p);
                    }
                    break;
                }
            }
            SubTotal = String.Format("{0:0,0 VND}", NewOrder.Select(o => o.GiaBan * o.SoLuong).Sum());
        }

        private void AddToOrder(Mon mon, string size)
        {
            var isContain = false;

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
                    MaMon = mon.MaMon,
                    TenMon = mon.TenMon,
                    Size = size,
                    SoLuong = 1,
                    GiaBan = size == "M" ? mon.GiaBanM 
                            :size == "L" ? mon.GiaBanL : mon.GiaBanXL
                });
            }
            SubTotal = String.Format("{0:0,0 VND}", NewOrder.Select(o => o.GiaBan * o.SoLuong).Sum());
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
