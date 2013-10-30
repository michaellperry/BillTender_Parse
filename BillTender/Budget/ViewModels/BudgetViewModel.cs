using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BillTender.Budget.Models;
using BillTender.ViewModels;
using Parse;
using UpdateControls;
using UpdateControls.XAML;
using System;
using UpdateControls.Fields;
using System.Threading.Tasks;
using UpdateControls.Collections;

namespace BillTender.Budget.ViewModels
{
    public class BudgetViewModel : ProgressViewModel
    {
        public delegate void BillEditedHandler(object sender, BillEditedEventArgs args);
        public event BillEditedHandler BillEdited;

        private readonly BillTender.Families.Models.Family _family;

        private IndependentList<Bill> _bills = new IndependentList<Bill>();
        private Independent<Bill> _selectedBill = new Independent<Bill>();

        public BudgetViewModel(BillTender.Families.Models.Family family)
        {
            _family = family;
        }

        public void Load()
        {
            Perform(async delegate
            {
                //var query = new ParseQuery<Bill>()
                //    .Where(bill => bill.User == _user);
                var query =
                    from bill in new ParseQuery<Bill>()
                    where bill.User == _user
                    select bill;
                var results = await query.FindAsync();
                foreach (var bill in results)
                    _bills.Add(bill);
            });
        }

        public IEnumerable<Bill> Bills
        {
            get
            {
                return _bills;
            }
        }

        public Bill SelectedBill
        {
            get { return _selectedBill.Value; }
            set { _selectedBill.Value = value; }
        }

        public ICommand NewBill
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        if (BillEdited != null)
                        {
                            var bill = ParseObject.Create<Bill>();
                            bill.User = _user;
                            bill.NextDue = DateTime.Today;
                            BillEditedEventArgs args = new BillEditedEventArgs
                            {
                                Bill = bill,
                                Completed = delegate
                                {
                                    Perform(async delegate
                                    {
                                        await bill.SaveAsync();
                                        _bills.Add(bill);
                                    });
                                }
                            };
                            BillEdited(this, args);
                        }
                    });
            }
        }

        public ICommand EditBill
        {
            get
            {
                return MakeCommand
                    .When(() => _selectedBill.Value != null)
                    .Do(delegate
                    {
                        if (BillEdited != null)
                        {
                            var args = new BillEditedEventArgs
                            {
                                Bill = _selectedBill.Value,
                                Completed = delegate
                                {
                                    Perform(async delegate
                                    {
                                        await _selectedBill.Value
                                            .SaveAsync();
                                    });
                                },
                                Cancelled = delegate
                                {
                                    _selectedBill.Value
                                        .Revert();
                                }
                            };
                            BillEdited(this, args);
                        }
                    });
            }
        }
    }
}
