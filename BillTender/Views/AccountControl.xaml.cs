using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillTender.Views
{
    public sealed partial class AccountControl : UserControl
    {
        public AccountControl()
        {
            this.InitializeComponent();
        }

        private void UserName_Click(object sender, RoutedEventArgs e)
        {
            AccountPopup.IsOpen = true;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            AccountPopup.IsOpen = false;
        }
    }
}
