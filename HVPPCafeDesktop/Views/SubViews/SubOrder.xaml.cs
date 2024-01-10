using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HVPPCafeDesktop.Views.SubViews
{
    /// <summary>
    /// Interaction logic for SubOrder.xaml
    /// </summary>
    public partial class SubOrder : Window
    {
        public SubOrder()
        {
            InitializeComponent();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SetRegistry();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetRegistry();
        }

        private void GetRegistry()
        {
            string registryPath = "HKEY_CURRENT_USER\\Software\\HVPPCafe";
            var top = Registry.GetValue(registryPath, "Top", null);
            var left = Registry.GetValue(registryPath, "Left", null);

            if (top != null)
            {
                this.Top = double.Parse(top.ToString());
            }
            if (left != null)
            {
                this.Left = double.Parse(left.ToString());
            }
        }

        private void SetRegistry()
        {
            string registryPath = "HKEY_CURRENT_USER\\Software\\HVPPCafe";

            if (Registry.GetValue(registryPath, null, null) == null)
            {
                Registry.CurrentUser.CreateSubKey(@"Software\HVPPCafe");
            }

            Registry.SetValue(registryPath, "Top", this.Top);
            Registry.SetValue(registryPath, "Left", this.Left);
        }
    }
}
