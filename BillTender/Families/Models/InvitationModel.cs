using System;

namespace BillTender.Families.Models
{
    public class InvitationModel
    {
        private string _emailAddress;

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }
    }
}
