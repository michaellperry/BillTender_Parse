﻿using System.Collections.Generic;
using System.Windows.Input;
using BillTender.Families.Models;
using BillTender.Helpers;
using BillTender.ViewModels;
using Parse;
using UpdateControls.Collections;
using UpdateControls.XAML;
using System;

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

        public ParseUser SelectedMember
        {
            get { return _memberSelection.SelectedMember; }
            set { _memberSelection.SelectedMember = value; }
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
                                Perform(async delegate
                                {
                                    // TODO

                                    var selectedUser = await new ParseQuery<ParseUser>()
                                        .Where(u => u.Email == invitation.EmailAddress)
                                        .FirstOrDefaultAsync();

                                    if (selectedUser == null)
                                        this.LastError = "User not found";
                                    else
                                    {
                                        if (_memberSelection.Add(selectedUser))
                                        {
                                            _family.Members.Add(selectedUser);
                                            if (_family.ACL == null)
                                                _family.ACL = new ParseACL();
                                            _family.ACL.SetReadAccess(selectedUser, true);
                                            _family.ACL.SetWriteAccess(selectedUser, true);
                                            await _family.SaveAsync();
                                        }
                                    }
                                });
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
                        Perform(async delegate
                        {
                            ParseUser selectedUser = _memberSelection.SelectedMember;
                            // TODO

                            _family.Members.Remove(selectedUser);
                            if (_family.ACL == null)
                                _family.ACL = new ParseACL();
                            _family.ACL.SetReadAccess(selectedUser, false);
                            _family.ACL.SetWriteAccess(selectedUser, false);
                            await _family.SaveAsync();

                            _memberSelection.Remove(selectedUser);
                            _memberSelection.SelectedMember = null;
                        });
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
