using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using UpdateControls.Collections;
using UpdateControls.Fields;

namespace BillTender.Families.Models
{
    public class MemberSelectionModel
    {
        private IndependentList<ParseUser> _members = new IndependentList<ParseUser>();
        private Independent<ParseUser> _selectedMember = new Independent<ParseUser>();

        public IEnumerable<ParseUser> Members
        {
            get { return _members; }
        }

        public void Clear()
        {
            _members.Clear();
        }

        public void AddRange(IEnumerable<ParseUser> members)
        {
            foreach (var member in members)
                _members.Add(member);
        }

        public void Remove(ParseUser member)
        {
            _members.Remove(member);
        }

        public ParseUser SelectedMember
        {
            get { return _selectedMember; }
            set { _selectedMember.Value = value; }
        }
    }
}
