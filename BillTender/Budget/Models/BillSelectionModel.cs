using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _bills.Clear();
            foreach (var bill in bills)
                _bills.Add(bill);
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
