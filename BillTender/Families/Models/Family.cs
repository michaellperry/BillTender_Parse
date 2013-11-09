﻿using System.Collections.Generic;
using BillTender.Budget.Models;
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

        [ParseFieldName("Bills")]
        public IList<Bill> Bills
        {
            get { return GetProperty<IList<Bill>>() ?? new List<Bill>(); }
        }

        public void AddBill(Bill bill)
        {
            AddToList("Bills", bill);
        }

        public void RemoveBill(Bill bill)
        {
            RemoveAllFromList("Bills", new List<Bill> { bill });
        }
    }
}
