﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using BillTender.Budget.Models;
using BillTender.Families.Models;
using BillTender.ViewModels;
using Parse;
using UpdateControls.Collections;
using UpdateControls.XAML;

namespace BillTender.Payments.ViewModels
{
    public class PaymentViewModel : ProgressViewModel
    {
        private readonly ParseUser _user;

        private IndependentList<Bill> _bills = new IndependentList<Bill>();

        public PaymentViewModel(ParseUser user)
        {
            _user = user;
        }

        public void Load()
        {
            _bills.Clear();
            Perform(async delegate
            {
                // Using a ParseRelation:

                var bills =
                    from family in new ParseQuery<Family>()
                    where family["Members"] == _user
                    join bill in new ParseQuery<Bill>()
                        on family equals bill.Family
                    select bill;

                // Or, using a join table:
                //
                //var members =
                //    from member in new ParseQuery<Member>()
                //    where member.User == _user
                //    select member;
                //var bills =
                //    from member in members
                //    join bill in new ParseQuery<Bill>()
                //        on member.Family equals bill.Family
                //    select bill;

                var query =
                    from bill in bills
                    where bill.NextDue < new DateTime(2013, 9, 26)
                    orderby bill.NextDue, bill.Payee
                    select bill;

                var results = await query
                    .Include("Family")
                    .FindAsync();
                foreach (var bill in results)
                    _bills.Add(bill);
            });
        }

        public IEnumerable<Bill> Bills
        {
            get { return _bills; }
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
