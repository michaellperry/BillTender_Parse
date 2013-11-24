using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BillTender.Families.Models;
using BillTender.Helpers;
using BillTender.ViewModels;
using Parse;
using UpdateControls.XAML;

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
            _familySelection.ClearFamilies();
            Perform(async delegate
            {
                var roles =
                    from role in new ParseQuery<ParseRole>()
                    where role["users"] == _user
                    select role;
                var writable =
                    from writer in roles
                    join family in new ParseQuery<Family>()
                        on writer equals family.Writers
                    select family;
                var readable =
                    from reader in roles
                    join family in new ParseQuery<Family>()
                        on reader equals family.Readers
                    select family;
                var families = await writable.Or(readable)
                    .Include("Readers")
                    .Include("Writers")
                    .FindAsync();

                _familySelection.AddFamilies(families);
            });
        }

        public IEnumerable<Family> Families
        {
            get { return _familySelection.Families; }
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
                                    // TODO
                                    await family.SaveAsync();
                                    family.Initialize();
                                    family.Writers.Users.Add(_user);
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
                        Perform(async delegate
                        {
                            Family selectedFamily = _familySelection.SelectedFamily;

                            // TODO
                            selectedFamily.Readers.Users.Remove(_user);
                            selectedFamily.Writers.Users.Remove(_user);
                            await selectedFamily.SaveAsync();

                            _familySelection.RemoveFamily(selectedFamily);
                            _familySelection.SelectedFamily =
                                _familySelection.Families.FirstOrDefault();
                        });
                    });
            }
        }
    }
}
