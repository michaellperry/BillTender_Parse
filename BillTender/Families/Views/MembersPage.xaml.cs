using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillTender.Families.Views
{
    public sealed partial class MembersPage : Page
    {
        public MembersPage()
        {
            this.InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }
    }
}
