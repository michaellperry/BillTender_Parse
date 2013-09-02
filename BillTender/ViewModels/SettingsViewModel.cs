using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parse;

namespace BillTender.ViewModels
{
    public class SettingsViewModel
    {
        private readonly ParseUser _user;

        public SettingsViewModel(ParseUser user)
        {
            _user = user;
        }

        public ParseObject User
        {
            get { return _user; }
        }
    }
}
