using System.Collections.Generic;
using UpdateControls.Fields;

namespace BillTender.Families.Models
{
    public class FamilySelectionModel
    {
        private Independent<List<Family>> _families = new Independent<List<Family>>();
        private Independent<Family> _selectedFamily = new Independent<Family>();

        public List<Family> Families
        {
            get { return _families; }
            set { _families.Value = value; }
        }

        public Family SelectedFamily
        {
            get { return _selectedFamily; }
            set { _selectedFamily.Value = value; }
        }
    }
}
