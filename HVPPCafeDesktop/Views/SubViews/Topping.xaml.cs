using HVPPCafeDesktop.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace HVPPCafeDesktop.Views.SubViews
{
    /// <summary>
    /// Interaction logic for Topping.xaml
    /// </summary>
    public partial class Topping : Window
    {
        public Topping()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
