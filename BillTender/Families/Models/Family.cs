using Parse;

namespace BillTender.Families.Models
{
    [ParseClassName("Family")]
    public class Family : ParseObject
    {
        public void AddMember(ParseUser user)
        {
            Members.Add(user);
        }

        public void RemoveMember(ParseUser user)
        {
            Members.Remove(user);
        }

        public ParseRelation<ParseUser> Members
        {
            get { return GetRelation<ParseUser>("Members"); }
        }

        [ParseFieldName("Name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
    }
}
