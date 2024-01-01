using System.Windows;

namespace HVPPCafeDesktop.CustomControl
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string mess, bool YES = false)
        {
            InitializeComponent();
            if (YES)
            {
                Run(mess, true);
            }
            else
            {
                Run(mess);
            }
        }
        private void Run(string Message, bool yesno = false)
        {
            lbMessage.Text = Message;
            if (!yesno)
            {
                btnYES.Visibility = Visibility.Hidden;
                btnNO.Visibility = Visibility.Hidden;
            }
            else
            {
                btnOKcenter.Visibility = Visibility.Hidden;
            }
        }
        private bool Accept = false;
        private void btnOKcenter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnYES_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Accept = true;
        }
        private void btnNO_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Accept = false;
        }
        public bool ACCEPT()
        {
            return Accept;
        }
    }
}
