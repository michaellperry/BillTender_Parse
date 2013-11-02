using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using BillTender.Families.Models;

namespace BillTender.Families.ViewModels
{
    public class FamilySelectorDesignDataModel
    {
        private FamilySelectorViewModel _familySelector;

        public FamilySelectorDesignDataModel()
        {
            FamilySelectionModel familySelection = new FamilySelectionModel();
            Family perry = new Family { Name = "Perry" };
            Family wilson = new Family { Name = "Wilson" };
            familySelection.AddFamily(perry);
            familySelection.AddFamily(wilson);
            familySelection.SelectedFamily = perry;

            _familySelector = new FamilySelectorViewModel(
                new ParseUser(),
                familySelection);
        }

        public FamilySelectorViewModel FamilySelector
        {
            get { return _familySelector; }
        }
    }
}
