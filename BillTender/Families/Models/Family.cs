using Parse;
using System;
using BillTender.Families.Mementos;

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

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            Family that = obj as Family;
            if (that == null)
                return false;
            return Object.Equals(this.ObjectId, that.ObjectId);
        }

        public override int GetHashCode()
        {
            return ObjectId.GetHashCode();
        }

        public static Family FromMemento(FamilyMemento memento)
        {
            var family = ParseObject.Create<Family>();
            family.ObjectId = memento.ObjectId;
            family.Name = memento.Name;
            return family;
        }

        public FamilyMemento ToMemento()
        {
            return new FamilyMemento
            {
                ObjectId = ObjectId,
                Name = Name
            };
        }
    }
}
