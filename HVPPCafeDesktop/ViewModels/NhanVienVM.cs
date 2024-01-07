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
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;

namespace HVPPCafeDesktop.ViewModels
{
    class NhanVienVM : BaseViewModel
    {
        private ObservableCollection<NhanVien> _ListStaff = new ObservableCollection<NhanVien>();
        public ObservableCollection<NhanVien> ListStaff
        {
            get => _ListStaff;
            set => SetProperty(ref _ListStaff, value);
        }

        public List<NhanVien> Raw = new List<NhanVien>();

        private NhanVien _NhanVienItem = new NhanVien();
        public NhanVien NhanVienItem
        {
            get => _NhanVienItem;
            set => SetProperty(ref _NhanVienItem, value);
        }

        private NhanVien _StaffSelected = new NhanVien();
        public NhanVien StaffSelected
        {
            get => _StaffSelected;
            set => SetProperty(ref _StaffSelected, value);
        }

        private string _Search;
        public string Search
        {
            get => Search;
            set
            {
                _Search = value;
                Filter(Search);
                OnPropertyChanged();
            }
        }

        public ICommand AddCM { get; set; }
        public ICommand EditCM { get; set; }
        public ICommand OpenAddCM { get; set; }
        public ICommand CheckCM { get; set; }

        public NhanVienVM()
        {
            GetStaffs();

            OpenAddCM = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new NhanVienThem();
                window.DataContext = this;
                window.ShowDialog();
            });

            AddCM = new RelayCommand<object>((p) => true, (p) =>
            {
                AddNewStaff();
            });

            EditCM = new RelayCommand<object>((p) => true, (p) =>
            {
                EditStaff();
            });

            CheckCM = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new ChamCong();
                window.DataContext = this;
                window.ShowDialog();
            });
        }

        private async void GetStaffs()
        {
            ListStaff.Clear();
            Raw.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/NhanVien/all");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var menu = JsonConvert.DeserializeObject<List<NhanVien>>(result);

                    if (menu == null)
                    {
                        return;
                    }

                    foreach (var item in menu)
                    {
                        ListStaff.Add(item);
                        Raw.Add(item);
                    }
                }
            }
        }

        public void Filter(string search)
        {
            var nv = search == "" ? Raw : Raw.Where(n => n.HoTen.ToLower().Contains(search.ToLower()));

            ListStaff.Clear();

            foreach (var item in nv) 
            {
                ListStaff.Add(item);
            }
        }

        private async void EditStaff()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var json = JsonConvert.SerializeObject(StaffSelected);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("api/NhanVien", content);

                if (response.IsSuccessStatusCode)
                {
                    var msg = new CustomMessageBox("Sửa thành công!");
                    msg.ShowDialog();
                }
            }
        }

        public async void AddNewStaff()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);

                NhanVienItem.MaNV = "";

                var json = JsonConvert.SerializeObject(NhanVienItem);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/NhanVien", content);

                if (response.IsSuccessStatusCode)
                {
                    var msg = new CustomMessageBox("Thêm thành công!");
                    msg.ShowDialog();
                }
            }
        }
    }
}
