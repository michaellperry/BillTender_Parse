using System.Windows.Input;
using BillTender.Settings.Models;
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
                        _accountModel.CurrentUserChanged();
                    });
            }
        }
    }
}
