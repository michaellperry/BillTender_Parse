using System.Collections.Generic;
using UpdateControls.Collections;
using UpdateControls.Fields;
using BillTender.Families.Models;
using System;
using System.Linq;

namespace BillTender.Families.Models
{
    public class FamilySelectionModel
    {
        private IndependentList<Family> _families = new IndependentList<Family>();
        private Independent<Family> _selectedFamily = new Independent<Family>();

        public IEnumerable<Family> Families
        {
            get { return _families; }
        }

        public Family SelectedFamily
        {
            get { return _selectedFamily; }
            set { _selectedFamily.Value = value; }
        }

        public void SetFamilies(IEnumerable<Family> families)
        {
            _families.Clear();
            foreach (var family in families)
                _families.Add(family);
            SelectedFamily = families.FirstOrDefault();
        }

        public void AddFamily(Family family)
        {
            _families.Add(family);
        }

        public void RemoveFamily(Family family)
        {
            _families.Remove(family);
        }
    }
}
