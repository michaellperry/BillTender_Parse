using BillTender.Settings.Models;
using BillTender.Settings.ViewModels;
using UpdateControls.XAML;

namespace BillTender.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly AccountModel _accountModel;

        public ViewModelLocator()
        {
            _accountModel = new AccountModel();
        }

        public object CurrentUser
        {
            get { return ViewModel(() => new CurrentUserViewModel(_accountModel)); }
        }

        public object Account
        {
            get { return ViewModel(() => new AccountViewModel(_accountModel)); }
        }

        public object LogIn
        {
            get { return ViewModel(() => new LogInViewModel(_accountModel)); }
        }

        public object SignUp
        {
            get { return ViewModel(() => new SignUpViewModel(_accountModel)); }
        }
    }
}
