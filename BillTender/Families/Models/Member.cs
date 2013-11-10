using Parse;

namespace BillTender.Families.Models
{
    [ParseClassName("Member")]
    public class Member : ParseObject
    {
        [ParseFieldName("User")]
        public ParseUser User
        {
            get { return GetProperty<ParseUser>(); }
            set { SetProperty<ParseUser>(value); }
        }

        [ParseFieldName("Family")]
        public Family Family
        {
            get { return GetProperty<Family>(); }
            set { SetProperty<Family>(value); }
        }

        [ParseFieldName("Role")]
        public string Role
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(); }
        }
    }
}
