using System.Windows.Input;
using BillTender.Settings.Models;
using UpdateControls.XAML;

namespace BillTender.Settings.ViewModels
{
    public class SignUpViewModel
    {
        private readonly AccountModel _accountModel;

        public SignUpViewModel(AccountModel accountModel)
        {
            _accountModel = accountModel;            
        }

        public bool Busy
        {
            get { return _accountModel.Busy; }
            set { _accountModel.Busy = value; }
        }

        public string LastError
        {
            get { return _accountModel.LastError; }
            set { _accountModel.LastError = value; }
        }

        public ICommand ExistingAccount
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        _accountModel.IsExistingUser = true;
                    });
            }
        }

        public void SignedUp()
        {
            _accountModel.CurrentUserChanged();
        }
    }
}
