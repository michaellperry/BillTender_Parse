using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillTender.Families.Models;
using BillTender.ViewModels;
using Parse;

namespace BillTender.Families.ViewModels
{
    public class FamilySelectorViewModel : ProgressViewModel
    {
        private readonly ParseUser _user;
        private readonly FamilySelectionModel _familySelection;

        public FamilySelectorViewModel(ParseUser user, FamilySelectionModel familySelection)
        {
            _user = user;
            _familySelection = familySelection;
        }

        public void Load()
        {
            this.Perform(async delegate
            {
                var families = await _user.GetRelation<Family>("Families").Query.FindAsync();
                var tasks = families.Select(family => family.FetchAsync());
                await Task.WhenAll(tasks);
                _familySelection.Families = families.ToList();
                _familySelection.SelectedFamily = families.FirstOrDefault();
            });
        }

        public IEnumerable<Family> Families
        {
            get
            {
                return
                    from family in _familySelection.Families
                    orderby family.Name
                    select family;
            }
        }

        public Family SelectedFamily
        {
            get { return _familySelection.SelectedFamily; }
            set { _familySelection.SelectedFamily = value; }
        }
    }
}
