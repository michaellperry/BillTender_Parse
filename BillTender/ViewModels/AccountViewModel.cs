using System.Windows.Input;
using Parse;
using BillTender.Models;
using UpdateControls.XAML;

namespace BillTender.ViewModels
{
    public class AccountViewModel
    {
        private readonly LoginModel _loginModel;

        public AccountViewModel(LoginModel loginModel)
        {
            _loginModel = loginModel;
        }

        public string UserName
        {
            get
            {
                ParseUser currentUser = _loginModel.CurrentUser;
                if (currentUser == null)
                    return string.Empty;
                return currentUser.Username;
            }
        }

        public ICommand LogOut
        {
            get
            {
                return MakeCommand
                    .When(() => _loginModel.CurrentUser != null)
                    .Do(delegate
                    {
                        ParseUser.LogOut();
                        _loginModel.CurrentUserChanged();
                    });
            }
        }
    }
}
