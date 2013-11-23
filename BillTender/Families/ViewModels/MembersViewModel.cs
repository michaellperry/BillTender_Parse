using System.Collections.Generic;
using System.Windows.Input;
using BillTender.Families.Models;
using BillTender.Helpers;
using BillTender.ViewModels;
using Parse;
using UpdateControls.Collections;
using UpdateControls.XAML;

namespace BillTender.Families.ViewModels
{
    public class MembersViewModel : ProgressViewModel
    {
        private readonly Family _family;
        private readonly MemberSelectionModel _memberSelection;
        
        public MembersViewModel(Family family, MemberSelectionModel memberSelection)
        {
            _family = family;
            _memberSelection = memberSelection;
        }

        public void Load()
        {
            _memberSelection.Clear();
            Perform(async delegate
            {
                var members = await _family.Members.Query.FindAsync();
                _memberSelection.AddRange(members);
            });
        }

        public IEnumerable<ParseUser> Members
        {
            get { return _memberSelection.Members; }
        }

        public ICommand AddMember
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        var invitation = new InvitationModel();
                        DialogManager.ShowInvitationDialog(
                            invitation,
                            delegate
                            {
                                // TODO
                            });
                    });
            }
        }

        public ICommand RemoveMember
        {
            get
            {
                return MakeCommand
                    .When(() => _memberSelection.SelectedMember != null)
                    .Do(delegate
                    {
                        // TODO

                        _memberSelection.Remove(_memberSelection.SelectedMember);
                        _memberSelection.SelectedMember = null;
                    });
            }
        }

        public ICommand Refresh
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        Load();
                    });
            }
        }
    }
}
