using System.Windows.Input;
using BillTender.Models;
using UpdateControls.XAML;

namespace BillTender.ViewModels
{
    public class SignUpViewModel
    {
        private readonly LoginModel _loginModel;

        public SignUpViewModel(LoginModel loginModel)
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

        public ICommand ExistingAccount
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        _loginModel.IsExistingUser = true;
                    });
            }
        }

        public void SignedUp()
        {
            _loginModel.CurrentUserChanged();
        }
    }
}
