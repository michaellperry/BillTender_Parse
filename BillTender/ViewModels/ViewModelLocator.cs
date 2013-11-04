using BillTender.Settings.Models;
using BillTender.Settings.ViewModels;
using UpdateControls.XAML;
using System;
using Parse;
using BillTender.Payments.ViewModels;
using BillTender.Budget.ViewModels;
using BillTender.Families.ViewModels;
using BillTender.Families.Models;

namespace BillTender.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly AccountModel _accountModel;
        private readonly FamilySelectionModel _familySelection;

        public ViewModelLocator()
        {
            _accountModel = new AccountModel();
            _familySelection = new FamilySelectionModel();
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

        public object FamilySelector
        {
            get
            {
                return ViewModel(delegate
                {
                    if (_accountModel.CurrentUser == null)
                        return null;

                    FamilySelectorViewModel viewModel = new FamilySelectorViewModel(
                        _accountModel.CurrentUser,
                        _familySelection);
                    viewModel.Load();
                    return viewModel;
                });
            }
        }

        public object Budget
        {
            get
            {
                return ViewModel(delegate
                {
                    if (_familySelection.SelectedFamily == null)
                        return null;

                    BudgetViewModel viewModel = new BudgetViewModel(_accountModel.CurrentUser);
                    viewModel.Load();
                    return viewModel;
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

                    var viewModel = new PaymentViewModel(currentUser);
                    viewModel.Load();
                    return viewModel;
                });
            }
        }
    }
}
