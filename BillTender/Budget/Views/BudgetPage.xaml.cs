using BillTender.Families.Views;
using BillTender.Payments.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillTender.Budget.Views
{
    public sealed partial class BudgetPage : Page
    {
        public BudgetPage()
        {
            this.InitializeComponent();
        }

        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PaymentPage));
        }

        private void Members_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MembersPage));
        }
    }
}
