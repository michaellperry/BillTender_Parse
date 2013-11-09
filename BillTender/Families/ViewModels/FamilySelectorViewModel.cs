using System.Collections.Generic;
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
            this.Perform(async delegate
            {
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
                        });
                    });
            }
        }
    }
}
