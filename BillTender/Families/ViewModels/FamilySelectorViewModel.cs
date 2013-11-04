using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BillTender.Families.Models;
using BillTender.Families.Views;
using BillTender.Helpers;
using BillTender.ViewModels;
using Parse;
using UpdateControls.XAML;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
                var myFamilies =
                    from family in new ParseQuery<Family>()
                    where family["Members"] == _user
                    select family;

                var families = await myFamilies.FindAsync();
                var tasks = families.Select(family => family.FetchAsync());
                await Task.WhenAll(tasks);
                foreach (var family in families)
                    _familySelection.AddFamily(family);
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

        public ICommand AddFamily
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        Family family = ParseObject.Create<Family>();
                        DialogManager.ShowFamilyDialog(
                            "New Family",
                            family,
                            completed: delegate
                            {
                                Perform(async delegate
                                {
                                    family.AddMember(_user);
                                    await family.SaveAsync();

                                    _familySelection.AddFamily(family);
                                    _familySelection.SelectedFamily = family;
                                });
                            });
                    });
            }
        }

        public ICommand EditFamily
        {
            get
            {
                return MakeCommand
                    .When(() => _familySelection.SelectedFamily != null)
                    .Do(delegate
                    {
                        Family selectedFamily = _familySelection.SelectedFamily;
                        DialogManager.ShowFamilyDialog(
                            "Edit Family",
                            selectedFamily,
                            completed: delegate
                            {
                                Perform(async delegate
                                {
                                    await selectedFamily.SaveAsync();
                                });
                            },
                            cancelled: delegate
                            {
                                selectedFamily.Revert();
                            });
                    });
            }
        }

        public ICommand RemoveFamily
        {
            get
            {
                return MakeCommand
                    .When(() => _familySelection.SelectedFamily != null)
                    .Do(delegate
                    {
                        Family selectedFamily = _familySelection.SelectedFamily;
                        selectedFamily.RemoveMember(_user);

                        _familySelection.RemoveFamily(selectedFamily);
                        _familySelection.SelectedFamily = _familySelection.Families.FirstOrDefault();

                        Perform(async delegate
                        {
                            await selectedFamily.SaveAsync();
                        });
                    });
            }
        }
    }
}
