using Parse;

namespace BillTender.Families.Models
{
    [ParseClassName("Family")]
    public class Family : ParseObject
    {
        public void Initialize()
        {
            ACL = new ParseACL();
            Readers = new ParseRole(ObjectId + "_Readers", ACL);
            Writers = new ParseRole(ObjectId + "_Writers", ACL);

            ACL.SetRoleReadAccess(Readers, true);
            ACL.SetRoleReadAccess(Writers, true);
            ACL.SetRoleWriteAccess(Writers, true);
        }

        public ParseRelation<ParseUser> Members
        {
            get { return GetRelation<ParseUser>("Members"); }
        }

        [ParseFieldName("Readers")]
        public ParseRole Readers
        {
            get { return GetProperty<ParseRole>(); }
            set { SetProperty<ParseRole>(value); }
        }

        [ParseFieldName("Writers")]
        public ParseRole Writers
        {
            get { return GetProperty<ParseRole>(); }
            set { SetProperty<ParseRole>(value); }
        }

        [ParseFieldName("Name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
    }
}
