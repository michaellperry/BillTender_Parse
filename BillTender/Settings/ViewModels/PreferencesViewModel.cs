using System;
using System.Linq;
using BillTender.Settings.Models;
using UpdateControls;

namespace BillTender.Settings.ViewModels
{
    public class PreferencesViewModel
    {
        public PreferencesViewModel(object user)
        {
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
