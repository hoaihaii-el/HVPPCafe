using HVPPCafeDesktop.Models;
using HVPPCafeDesktop.Views.SubViews;
using Sub = HVPPCafeDesktop.Views.SubViews;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HVPPCafeDesktop.ViewModels;

namespace HVPPCafeDesktop.Views
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : UserControl
    {
        public Order()
        {
            InitializeComponent();
        }

        public SubOrder subOrder { get; set; }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (subOrder == null)
            {
                subOrder = new SubOrder();
                subOrder.DataContext = this.DataContext;
                subOrder.Show();
            }
            else
            {
                subOrder.Close();
                subOrder = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as Button;
            var point = item?.PointToScreen(Mouse.GetPosition(item));

            var order = item?.DataContext as NewOrder;
            var vm = this.DataContext as OrderVM;

            if (order == null || vm == null) return;

            vm.ResetToppings();
            foreach (var temp in order.toppings)
            {
                if (vm.Toppings.Contains(temp))
                {
                    vm.Toppings[vm.Toppings.IndexOf(temp)].IsPicked = true;
                }
            }

            var popup = new Sub.Topping(order.Index);
            Sub.Topping.Index = order.Index;
            popup.Height = vm.Toppings.Count * 30 + 60;
            popup.Left = point.Value.X;
            popup.Top = point.Value.Y - popup.Height;
            popup.DataContext = vm;
            popup.ShowDialog();
        }
    }
}
