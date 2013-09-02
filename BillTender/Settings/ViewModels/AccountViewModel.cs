﻿using System.Windows.Input;
using BillTender.Settings.Models;
using Parse;
using UpdateControls.XAML;

namespace BillTender.Settings.ViewModels
{
    public class AccountViewModel
    {
        public enum AccountState
        {
            SigningUp,
            LoggingIn,
            LoggedIn
        }

        private readonly AccountModel _accountModel;

        public AccountViewModel(AccountModel accountModel)
        {
            _accountModel = accountModel;
        }

        public bool Busy
        {
            get { return _accountModel.Busy; }
        }

        public string LastError
        {
            get { return _accountModel.LastError; }
        }

        public AccountState CurrentAccountState
        {
            get
            {
                if (_accountModel.CurrentUser != null)
                    return AccountState.LoggedIn;
                if (_accountModel.IsExistingUser)
                    return AccountState.LoggingIn;
                else
                    return AccountState.SigningUp;
            }
        }

        public bool CurrentUserVisible
        {
            get { return CurrentAccountState == AccountState.LoggedIn; }
        }

        public bool LogInVisible
        {
            get { return CurrentAccountState == AccountState.LoggingIn; }
        }

        public bool SignUpVisible
        {
            get { return CurrentAccountState == AccountState.SigningUp; }
        }
    }
}
