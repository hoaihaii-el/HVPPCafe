using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HVPPCafe.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HVPPCafe.ViewModels
{
    public partial class MenuViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Menu> menus;

        public MenuViewModel()
        {
            Menus = new ObservableCollection<Menu>();
            GetMenus();
            Menus.Add(new Menu()
            {
                TenMon = "MON1",
                GiaBanM = 30000,
                GiaBanL = 35000,
                GiaBanXL = 40000
            });
            Menus.Add(new Menu()
            {
                TenMon = "MON2",
                GiaBanM = 30000,
                GiaBanL = 35000,
                GiaBanXL = 40000
            });
        }

        [RelayCommand]
        public void AddM(Menu m)
        {

        }

        public async void GetMenus()
        {
#if DEBUG
            var handler = new HttpsClientHandlerService();
            var client = new HttpClient(handler.GetPlatformMessageHandler());
#else
            client = new HttpClient();
#endif

            client.BaseAddress = new Uri("https://10.0.2.2:7170");
            var response = await client.GetAsync($"api/Mon/all");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var menu = JsonConvert.DeserializeObject<List<Menu>>(result);

                if (menu == null)
                {
                    return;
                }

                foreach (var item in menu)
                {
                    Menus.Add(item);
                }
            }
        }
    }
}
