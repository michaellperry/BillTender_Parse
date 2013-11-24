using System;

namespace BillTender.Families.Models
{
    public class InvitationModel
    {
        private string _emailAddress;
        private bool _canManageBillsAndUsers;

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        public bool CanManageBillsAndUsers
        {
            get { return _canManageBillsAndUsers; }
            set { _canManageBillsAndUsers = value; }
        }
    }
}
