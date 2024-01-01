using System;
using System.Windows;
using System.Windows.Input;

namespace HVPPCafeDesktop.CustomControl
{
    /// <summary>
    /// Interaction logic for ChooseDate.xaml
    /// </summary>
    public partial class ChooseDate : Window
    {
        public ChooseDate()
        {
            InitializeComponent();
        }
        public DateTime? DateBegin => dtBegin.SelectedDate;
        public DateTime? DateEnd => dtEnd.SelectedDate;

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (dtBegin.SelectedDate == null || dtEnd.SelectedDate == null)
            {
                var msg = new CustomMessageBox("Vui lòng chọn ngày!");
                msg.ShowDialog();
                return;
            }
            if (dtBegin.SelectedDate > dtEnd.SelectedDate || dtEnd.SelectedDate > DateTime.Now)
            {
                var msg = new CustomMessageBox("Vui lòng chọn ngày hợp lệ!");
                msg.ShowDialog();
                return;
            }
            this.Close();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
