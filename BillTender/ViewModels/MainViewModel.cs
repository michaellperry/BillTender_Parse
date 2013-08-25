using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BillTender.Models;

namespace BillTender.ViewModels
{
    public class MainViewModel
    {
        public enum AccountState
        {
            SigningUp,
            LoggingIn,
            LoggedIn
        }

        private readonly LoginModel _loginModel;

        public MainViewModel(LoginModel loginModel)
        {
            _loginModel = loginModel;
        }

        public AccountState CurrentAccountState
        {
            get
            {
                if (_loginModel.CurrentUser != null)
                    return AccountState.LoggedIn;
                if (_loginModel.IsExistingUser)
                    return AccountState.LoggingIn;
                else
                    return AccountState.SigningUp;
            }
        }
    }
}
