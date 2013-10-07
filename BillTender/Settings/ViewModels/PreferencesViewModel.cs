using System;
using System.Linq;
using BillTender.Settings.Models;
using BillTender.ViewModels;
using UpdateControls;

namespace BillTender.Settings.ViewModels
{
    public class PreferencesViewModel : ProgressViewModel
    {
        private Independent _toastNotifications = new Independent();
        private Independent _sharing = new Independent();

        public bool ToastNotifications
        {
            get
            {
                _toastNotifications.OnGet();
                return false;
            }
            set
            {
                _toastNotifications.OnSet();
            }
        }

        public int Sharing
        {
            get
            {
                _sharing.OnGet();
                return (int)SharingLevel.None;
            }
            set
            {
                _sharing.OnSet();
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
