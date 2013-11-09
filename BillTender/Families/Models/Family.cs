using Parse;

namespace BillTender.Families.Models
{
    [ParseClassName("Family")]
    public class Family : ParseObject
    {
        [ParseFieldName("Name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
    }
}
