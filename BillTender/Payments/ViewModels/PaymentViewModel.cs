using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillTender.Budget.Models;
using BillTender.ViewModels;
using Parse;
using UpdateControls.Collections;

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
            Perform(async delegate
            {
                //var query = new ParseQuery<ParseUser>()
                //    .Where(user => user.ObjectId == _user.ObjectId)
                //    .Join(new ParseQuery<Bill>(),
                //        user => user,
                //        bill => bill.User,
                //        (user, bill) => bill)
                //    .Where(bill => bill.NextDue < new DateTime(2013, 9, 26))
                //    .OrderByDescending(bill => bill.NextDue);
                var query =
                    from user in new ParseQuery<ParseUser>()
                    where user.ObjectId == _user.ObjectId
                    join bill in new ParseQuery<Bill>()
                        on user equals bill.User
                    select bill;
                var filteredQuery =
                    from bill in query
                    where bill.NextDue < new DateTime(2013, 9, 26)
                    orderby bill.NextDue descending
                    select bill;
                var results = await filteredQuery.FindAsync();
                foreach (var bill in results)
                    _bills.Add(bill);
            });
        }

        public IEnumerable<Bill> Bills
        {
            get { return _bills; }
        }
    }
}
