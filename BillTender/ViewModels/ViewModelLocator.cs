using BillTender.Settings.Models;
using BillTender.Settings.ViewModels;
using UpdateControls.XAML;
using System;
using Parse;

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

        public bool CanSetPreferences
        {
            get { return _accountModel.CurrentUser != null; }
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

        public object Budget
        {
            get
            {
                return ViewModel(delegate
                {
                    if (_accountModel.CurrentUser == null)
                        return null;

                    return new BillTender.Budget.ViewModels.BudgetViewModel(_accountModel.CurrentUser);
                });
            }
        }

        public object Payment
        {
            get
            {
                return ViewModel(delegate
                {
                    ParseUser currentUser = _accountModel.CurrentUser;
                    if (currentUser == null)
                        return null;

                    var viewModel = new BillTender.Payments.ViewModels.PaymentViewModel(
                        currentUser);
                    viewModel.Load();
                    return viewModel;
                });
            }
        }
    }
}
