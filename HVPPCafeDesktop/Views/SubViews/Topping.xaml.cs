using Models = HVPPCafeDesktop.Models;
using HVPPCafeDesktop.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HVPPCafeDesktop.Views.SubViews
{
    /// <summary>
    /// Interaction logic for Topping.xaml
    /// </summary>
    public partial class Topping : Window
    {
        private int index;
        public static int Index = 0;
        public Topping(int index)
        {
            InitializeComponent();
            this.index = index;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (index != Index) return;

            var cbox = sender as CheckBox;
            var item = cbox?.DataContext as Models.Topping;
            var VM = this.DataContext as OrderVM;

            if (!VM.NewOrder[index].toppings.Contains(item))
            {
                VM.NewOrder[index].toppings.Add(item);
            }
            var order = VM.NewOrder[index];
            VM.NewOrder.RemoveAt(index);
            VM.NewOrder.Insert(index, order);
            VM.CalculateTotal();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (index != Index) return;

            var cbox = sender as CheckBox;
            var item = cbox?.DataContext as Models.Topping;
            var VM = this.DataContext as OrderVM;

            if (item == null) return;

            VM.NewOrder[index].toppings.Remove(item);
            var order = VM.NewOrder[index];
            VM.NewOrder.RemoveAt(index);
            VM.NewOrder.Insert(index, order);
            VM.CalculateTotal();
        }
    }
}
