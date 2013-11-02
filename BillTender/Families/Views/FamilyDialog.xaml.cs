using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillTender.Families.Views
{
    public sealed partial class FamilyDialog : UserControl
    {
        public event RoutedEventHandler Ok;
        public event RoutedEventHandler Cancel;
       
        public FamilyDialog()
        {
            this.InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Ok != null)
                Ok(sender, e);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel != null)
                Cancel(sender, e);
        }
    }
}
