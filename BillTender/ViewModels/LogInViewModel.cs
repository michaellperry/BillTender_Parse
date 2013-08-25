using System.Windows.Input;
using BillTender.Models;
using UpdateControls.XAML;

namespace BillTender.ViewModels
{
    public class LogInViewModel
    {
        private readonly LoginModel _loginModel;

        public LogInViewModel(LoginModel loginModel)
        {
            _loginModel = loginModel;
        }

        public bool Busy
        {
            get { return _loginModel.Busy; }
            set { _loginModel.Busy = value; }
        }

        public string LastError
        {
            get { return _loginModel.LastError; }
            set { _loginModel.LastError = value; }
        }

        public ICommand CreateAccount
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        _loginModel.IsExistingUser = false;
                    });
            }
        }

        public void LoggedIn()
        {
            _loginModel.CurrentUserChanged();
        }
    }
}
