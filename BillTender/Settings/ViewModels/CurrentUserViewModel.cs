using System.Windows.Input;
using BillTender.Settings.Models;
using Parse;
using UpdateControls.XAML;

namespace BillTender.Settings.ViewModels
{
    public class CurrentUserViewModel
    {
        private readonly AccountModel _accountModel;
        
        public CurrentUserViewModel(AccountModel accountModel)
        {
            _accountModel = accountModel;
        }

        public string UserName
        {
            get
            {
                ParseUser currentUser = _accountModel.CurrentUser;
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
                    .When(() => _accountModel.CurrentUser != null)
                    .Do(delegate
                    {
                        ParseUser.LogOut();
                        _accountModel.CurrentUserChanged();
                    });
            }
        }
    }
}
