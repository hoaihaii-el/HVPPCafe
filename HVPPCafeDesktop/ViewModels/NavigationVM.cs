using HVPPCafeDesktop.Resources.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HVPPCafeDesktop.ViewModels
{
    class NavigationVM : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public ICommand MenuCM { get; set; }
        public ICommand OrderCM { get; set; }
        public ICommand OrderStatusCM { get; set; }
        public ICommand KhoCM { get; set; }
        public ICommand KhuyenMaiCM { get; set; }
        public ICommand LichSuCM { get; set; }
        public ICommand LichSuCaCM { get; set; }
        public ICommand NhanVienCM { get; set; }
        public ICommand ThongKeCM { get; set; }

        private void Menu(object obj) => CurrentView = new MenuVM();
        private void Order(object obj) => CurrentView = new OrderVM();
        private void OrderStatus(object obj) => CurrentView = new OrderStatusVM();
        private void Kho(object obj) => CurrentView = new KhoVM();
        private void KhuyenMai(object obj) => CurrentView = new KhuyenMaiVM();
        private void LichSu(object obj) => CurrentView = new LichSuVM();
        private void LichSuCa(object obj) => CurrentView = new LichSuCaVM();
        private void NhanVien(object obj) => CurrentView = new NhanVienVM();
        private void ThongKe(object obj) => CurrentView = new ThongKeVM();

        public NavigationVM()
        {
            MenuCM = new RelayCommand(Menu);
            OrderCM = new RelayCommand(Order);
            OrderStatusCM = new RelayCommand(OrderStatus);
            KhoCM = new RelayCommand(Kho);
            KhuyenMaiCM = new RelayCommand(KhuyenMai);
            LichSuCM = new RelayCommand(LichSu);
            LichSuCaCM = new RelayCommand(LichSuCa);
            NhanVienCM = new RelayCommand(NhanVien);
            ThongKeCM = new RelayCommand(ThongKe);

            CurrentView = new MenuVM();
        }
    }
}
