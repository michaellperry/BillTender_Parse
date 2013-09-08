using System;
using System.Linq;
using BillTender.Settings.Models;
using Parse;
using UpdateControls;
using UpdateControls.Fields;

namespace BillTender.Settings.ViewModels
{
    public class PreferencesViewModel
    {
        private readonly ParseUser _user;

        private Independent _toastNotifications = new Independent();
        private Independent _sharing = new Independent();
        private Independent<string> _lastError = new Independent<string>();
        private Independent<bool> _busy = new Independent<bool>();

        public PreferencesViewModel(ParseUser user)
        {
            _user = user;
        }

        public string LastError
        {
            get { return _lastError; }
        }

        public bool Busy
        {
            get { return _busy; }
        }

        public bool ToastNotifications
        {
            get
            {
                _toastNotifications.OnGet();
                bool value;
                if (_user.TryGetValue<bool>(
                    "ToastNotifications",
                    out value))
                    return value;
                return false;
            }
            set
            {
                _toastNotifications.OnSet();
                _user["ToastNotifications"] = value;
                SaveChanges();
            }
        }

        public int Sharing
        {
            get
            {
                _sharing.OnGet();
                int value;
                if (_user.TryGetValue<int>(
                    "Sharing",
                    out value))
                    return value;
                return (int)SharingLevel.None;
            }
            set
            {
                _sharing.OnSet();
                if (IsValidSharingLevel(value))
                {
                    _user["Sharing"] = value;
                    SaveChanges();
                }
            }
        }

        private static bool IsValidSharingLevel(int value)
        {
            return Enum
                .GetValues(typeof(SharingLevel))
                .OfType<SharingLevel>()
                .Select(v => (int)v)
                .Contains(value);
        }

        private async void SaveChanges()
        {
            try
            {
                _busy.Value = true;
                _lastError.Value = string.Empty;
                await _user.SaveAsync();
            }
            catch (Exception x)
            {
                _lastError.Value = x.Message;
            }
            finally
            {
                _busy.Value = false;
            }
        }
    }
}
