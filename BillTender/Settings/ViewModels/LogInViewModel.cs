using System.Windows.Input;
using BillTender.Settings.Models;
using UpdateControls.XAML;

namespace BillTender.Settings.ViewModels
{
    public class LogInViewModel
    {
        private readonly AccountModel _accountModel;

        public LogInViewModel(AccountModel accountModel)
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

        public ICommand CreateAccount
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        _accountModel.IsExistingUser = false;
                    });
            }
        }

        public void LoggedIn()
        {
            _accountModel.CurrentUserChanged();
        }
    }
}
