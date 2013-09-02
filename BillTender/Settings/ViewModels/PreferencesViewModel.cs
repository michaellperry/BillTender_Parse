using System;
using System.Linq;
using BillTender.Settings.Models;
using Parse;
using UpdateControls;

namespace BillTender.Settings.ViewModels
{
    public class PreferencesViewModel
    {
        private readonly ParseUser _user;

        private Independent _toastNotifications = new Independent();
        private Independent _sharing = new Independent();

        public PreferencesViewModel(ParseUser user)
        {
            _user = user;
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
                _user.SaveAsync();
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
                    _user.SaveAsync();
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
    }
}
