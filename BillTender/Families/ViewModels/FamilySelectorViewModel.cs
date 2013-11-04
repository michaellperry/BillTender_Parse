using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BillTender.Families.Models;
using BillTender.ViewModels;
using Parse;
using UpdateControls.XAML;

namespace BillTender.Families.ViewModels
{
    public class FamilySelectorViewModel : ProgressViewModel
    {
        public delegate void FamilyEditedHandler(object sender, FamilyEditedEventArgs args);
        public event FamilyEditedHandler FamilyEdited;

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
                        if (FamilyEdited != null)
                        {
                            Family family = ParseObject.Create<Family>();
                            FamilyEditedEventArgs args = new FamilyEditedEventArgs
                            {
                                NewFamily = true,
                                Family = family,
                                Completed = delegate
                                {
                                    Perform(async delegate
                                    {
                                        family.AddMember(_user);
                                        await family.SaveAsync();

                                        _familySelection.AddFamily(family);
                                        _familySelection.SelectedFamily = family;
                                    });
                                }
                            };
                            FamilyEdited(this, args);
                        }
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
                        if (FamilyEdited != null)
                        {
                            Family selectedFamily = _familySelection.SelectedFamily;
                            FamilyEditedEventArgs args = new FamilyEditedEventArgs
                            {
                                NewFamily = false,
                                Family = selectedFamily,
                                Completed = delegate
                                {
                                    Perform(async delegate
                                    {
                                        await selectedFamily.SaveAsync();
                                    });
                                },
                                Cancelled = delegate
                                {
                                    selectedFamily.Revert();
                                }
                            };
                            FamilyEdited(this, args);
                        }
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
