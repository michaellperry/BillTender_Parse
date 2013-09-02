using Parse;
using UpdateControls;
using UpdateControls.Fields;

namespace BillTender.Settings.Models
{
    public class AccountModel
    {
        private Independent _currentUser = new Independent();
        private Independent<bool> _isExistingUser = new Independent<bool>(false);
        private Independent<bool> _busy = new Independent<bool>(false);
        private Independent<string> _lastError = new Independent<string>(string.Empty);

        public ParseUser CurrentUser
        {
            get
            {
                _currentUser.OnGet();
                return ParseUser.CurrentUser;
            }
        }

        public void CurrentUserChanged()
        {
            _currentUser.OnSet();
        }

        public bool IsExistingUser
        {
            get { return _isExistingUser; }
            set { _isExistingUser.Value = value; }
        }

        public bool Busy
        {
            get { return _busy; }
            set { _busy.Value = value; }
        }

        public string LastError
        {
            get { return _lastError; }
            set { _lastError.Value = value; }
        }
    }
}
