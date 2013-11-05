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
            });
        }

        public IEnumerable<Bill> Bills
        {
            get { return _bills; }
        }
    }
}
