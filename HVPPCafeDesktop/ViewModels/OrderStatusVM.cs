using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Resources;
using HVPPCafeDesktop.Resources.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HVPPCafeDesktop.ViewModels
{
    class OrderStatusVM : BaseViewModel
    {
        private ObservableCollection<CTHD> _Bills = new ObservableCollection<CTHD>();
        public ObservableCollection<CTHD> Bills
        {
            get => _Bills;
            set => SetProperty(ref _Bills, value);
        }
        public ICommand DoneCM { get; set; }

        public OrderStatusVM()
        {
            GetBills();

            DoneCM = new RelayCommand<CTHD>((p) => { return true; }, (p) =>
            {
                var done = Bills.Where(b => b.SoHoaDon == p.SoHoaDon).Count();
                Bills.Remove(p);
                if (done <= 1) 
                { 
                    Done(p.SoHoaDon); 
                }
            });
        }

        public async void GetBills()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.GetAsync($"api/HoaDon/chebien");

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
                        var count = Bills.Where(b => b.SoHoaDon == item.SoHoaDon).Count();
                        if (count <= 0)
                        {
                            Bills.Add(item);
                        }
                    }
                }
            }
        }

        private async void Done(int SoHD)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.DeleteAsync($"api/HoaDon/chebien/{SoHD}");

                if (response.IsSuccessStatusCode)
                {
                    GetBills();
                }
            }
        }
    }
}
