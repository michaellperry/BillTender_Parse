using System.Collections.Generic;
using System.Windows.Input;
using BillTender.Families.Models;
using BillTender.ViewModels;
using Parse;
using UpdateControls.Collections;
using UpdateControls.XAML;

namespace BillTender.Families.ViewModels
{
    public class MembersViewModel : ProgressViewModel
    {
        private readonly Family _family;

        private IndependentList<ParseUser> _members = new IndependentList<ParseUser>();

        public MembersViewModel(Family family)
        {
            _family = family;
        }

        public void Load()
        {
            _members.Clear();
            Perform(async delegate
            {
            });
        }

        public IEnumerable<ParseUser> Members
        {
            get { return _members; }
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
