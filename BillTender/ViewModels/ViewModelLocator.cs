using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillTender.Models;
using UpdateControls.XAML;

namespace BillTender.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly LoginModel _loginModel;

        public ViewModelLocator()
        {
            _loginModel = new LoginModel();
        }

        public object Main
        {
            get { return ViewModel(() => new MainViewModel(_loginModel)); }
        }

        public object LogIn
        {
            get { return ViewModel(() => new LogInViewModel(_loginModel)); }
        }

        public object SignUp
        {
            get { return ViewModel(() => new SignUpViewModel(_loginModel)); }
        }

        public object Account
        {
            get { return ViewModel(() => new AccountViewModel(_loginModel)); }
        }
    }
}
