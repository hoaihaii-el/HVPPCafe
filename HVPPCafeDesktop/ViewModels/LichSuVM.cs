using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;

namespace HVPPCafeDesktop.ViewModels
{
    class LichSuVM : BaseViewModel
    {
        private ObservableCollection<HoaDon> _ListBill = new ObservableCollection<HoaDon>();
        public ObservableCollection<HoaDon> ListBill
        {
            get => _ListBill;
            set => SetProperty(ref _ListBill, value);
        }

        private List<HoaDon> Raw = new List<HoaDon>();

        private HoaDon _BillSelected;
        public HoaDon BillSelected
        {
            get => _BillSelected;
            set => SetProperty(ref _BillSelected, value);
        }

        private ObservableCollection<CTHD> _Details = new ObservableCollection<CTHD>();
        public ObservableCollection<CTHD> Details
        {
            get => _Details;
            set => SetProperty(ref _Details, value);
        }

        private string _DetailOrderID;
        public string DetailOrderID
        {
            get => _DetailOrderID;
            set => SetProperty(ref _DetailOrderID, value);
        }

        private string _Search;
        public string Search
        {
            get => _Search;
            set
            {
                _Search = value;
                OnPropertyChanged();
                Filter(Search, DateBegin, DateEnd, FilterMaNV);
            }
        }

        private string _FilterMaNV;
        public string FilterMaNV
        {
            get => _FilterMaNV;
            set
            {
                _FilterMaNV = value;
                OnPropertyChanged();
                Filter(Search, DateBegin, DateEnd, FilterMaNV);
            }
        }

        private string _DateBegin;
        public string DateBegin
        {
            get => _DateBegin;
            set
            {
                _DateBegin = value;
                OnPropertyChanged();
                Filter(Search, DateBegin, DateEnd, FilterMaNV);
            }
        }

        private string _DateEnd;
        public string DateEnd
        {
            get => _DateEnd;
            set
            {
                _DateEnd = value;
                OnPropertyChanged();
                Filter(Search, DateBegin, DateEnd, FilterMaNV);
            }
        }

        public ICommand DetailCM { get; set; }
        public ICommand ExportCM { get; set; }

        public LichSuVM()
        {
            GetBills();
            DateBegin = DateTime.Now.ToShortDateString();
            DateEnd = DateTime.Now.ToShortDateString();

            DetailCM = new RelayCommand<HoaDon>((p) => { return true; }, (p) =>
            {
                DetailOrderID = p.SoHoaDon.ToString();
                GetDetail(p.SoHoaDon);
            });

            ExportCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Export();
            });
        }

        private void Export()
        {

        }

        private void Filter(string search, string dateBein, string dateEnd, string MaNV)
        {
            var bill = search == "" ? Raw : Raw.Where(h => h.SoHoaDon.ToString().Contains(search));

            if (!string.IsNullOrEmpty(dateBein) && !string.IsNullOrEmpty(dateEnd))
            {
                bill = bill.Where(h => h.NgayHoaDon.Date >= DateTime.Parse(dateBein)
                                    && h.NgayHoaDon.Date <= DateTime.Parse(dateEnd));
            }
            if (MaNV != "Tất cả")
            {
                bill = bill.Where(b => b.MaNV == MaNV);
            }

            ListBill.Clear();
            foreach (var h in bill)
            {
                ListBill.Add(h);
            }
        }

        public async void GetBills()
        {
            ListBill.Clear();
            Raw.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/HoaDon/all");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var menu = JsonConvert.DeserializeObject<List<HoaDon>>(result);

                    if (menu == null)
                    {
                        return;
                    }

                    foreach (var item in menu)
                    {
                        ListBill.Add(item);
                        Raw.Add(item);
                    }
                }
            }
        }

        public async void GetDetail(int SoHD)
        {
            Details.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/HoaDon/cthd-all/{SoHD}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var menu = JsonConvert.DeserializeObject<List<CTHD>>(result);

                    if (menu == null)
                    {
                        return;
                    }

                    foreach (var item in menu)
                    {
                        Details.Add(item);
                    }
                }
            }
        }
    }
}
