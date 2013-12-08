using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpdateControls;
using UpdateControls.Collections;
using UpdateControls.Fields;

namespace BillTender.Budget.Models
{
    public class BillSelectionModel
    {
        private IndependentList<Bill> _bills = new IndependentList<Bill>();
        private Independent<Bill> _selectedBill = new Independent<Bill>();

        public void SetBills(IEnumerable<Bill> bills)
        {
            var bin = new RecycleBin<Bill>(_bills);

            _bills.Clear();
            foreach (var bill in bills)
            {
                var recycled = bin.Extract(bill);
                recycled.Payee = bill.Payee;
                recycled.Amount = bill.Amount;
                recycled.Frequency = bill.Frequency;
                recycled.NextDue = bill.NextDue;

                _bills.Add(recycled);
            }
        }

        public IEnumerable<Bill> Bills
        {
            get { return _bills; }
        }

        public Bill SelectedBill
        {
            get { return _selectedBill.Value; }
            set { _selectedBill.Value = value; }
        }

        public void AddBill(Bill bill)
        {
            _bills.Add(bill);
        }
    }
}
