using BillTender.Settings.Models;
using BillTender.Settings.ViewModels;
using UpdateControls.XAML;
using System;

namespace BillTender.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly AccountModel _accountModel;

        public ViewModelLocator()
        {
            _accountModel = new AccountModel();
        }

        public object Main
        {
            get { return ViewModel(() => new MainViewModel(_accountModel)); }
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

        public object Preferences
        {
            get
            {
                return ViewModel(delegate
                {
                    if (_accountModel.CurrentUser == null)
                        return null;

                    return new PreferencesViewModel(_accountModel.CurrentUser);
                });
            }
        }
    }
}
