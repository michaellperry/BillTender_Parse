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
                if (_accountModel.CurrentUser != null)
                    return _accountModel.CurrentUser
                        .Username;
                return string.Empty;
            }
        }

        public ICommand LogOut
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        ParseUser.LogOut();
                        _accountModel.CurrentUserChanged();
                    });
            }
        }
    }
}
