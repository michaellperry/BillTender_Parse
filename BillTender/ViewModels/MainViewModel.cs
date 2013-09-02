using BillTender.Settings.Models;

namespace BillTender.ViewModels
{
    public class MainViewModel
    {
        private readonly AccountModel _accountModel;

        public MainViewModel(AccountModel accountModel)
        {
            _accountModel = accountModel;            
        }

        public bool CanSetPreferences
        {
            get { return _accountModel.CurrentUser != null; }
        }
    }
}
