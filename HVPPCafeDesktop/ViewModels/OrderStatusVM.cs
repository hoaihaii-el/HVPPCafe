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
using HVPPCafeDesktop.CustomControl;

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

        public static List<CTHD> BillDone = new List<CTHD>();

        public ICommand DoneCM { get; set; }

        public OrderStatusVM()
        {
            GetBills();

            DoneCM = new RelayCommand<CTHD>((p) => { return true; }, (p) =>
            {
                var done = Bills.Where(b => b.SoHoaDon == p.SoHoaDon).Count();
                BillDone.Add(p);
                Bills.Remove(p);
                if (done <= 1) 
                { 
                    Done(p.SoHoaDon, p.SoBan); 
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
                        var bill = BillDone.Where(b => b.ID == item.ID).FirstOrDefault();

                        if (bill == null)
                        {
                            if (Bills.Any(b => b.ID == item.ID)) continue;
                            Bills.Add(item);
                        }
                    }
                }
            }
        }

        private async void Done(int SoHD, int SoBan)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HVPPStringRes.BaseAPIAddress);
                var response = await client.DeleteAsync($"api/HoaDon/chebien/{SoHD}");

                if (response.IsSuccessStatusCode)
                {
                    var msg = new CustomMessageBox($"Đơn vừa hoàn thành #{SoHD}\nSố thẻ: {SoBan}");
                    msg.ShowDialog();
                    GetBills();
                }
            }
        }
    }
}
