using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BillTender.Families.Models;

namespace BillTender.Families.ViewModels
{
    public class FamilyHeaderViewModel
    {
        private readonly Family _family;

        public FamilyHeaderViewModel(Family family)
        {
            _family = family;
        }

        public Family Family
        {
            get { return _family; }
        }

        public string Name
        {
            get { return _family.Name; }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            FamilyHeaderViewModel that = obj as FamilyHeaderViewModel;
            if (that == null)
                return false;
            return Object.Equals(this._family.ObjectId, that._family.ObjectId);
        }

        public override int GetHashCode()
        {
            return _family.ObjectId.GetHashCode();
        }
    }
}
