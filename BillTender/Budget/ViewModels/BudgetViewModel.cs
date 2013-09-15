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

namespace BillTender.Budget.ViewModels
{
    public class BudgetViewModel : ProgressViewModel
    {
        public delegate void BillEditedHandler(object sender, BillEditedEventArgs args);
        public event BillEditedHandler BillEdited;

        private readonly ParseUser _user;

        private Independent _bills = new Independent();
        private Independent<Bill> _selectedBill = new Independent<Bill>();

        public BudgetViewModel(ParseUser user)
        {
            _user = user;
        }

        public IEnumerable<Bill> Bills
        {
            get
            {
                return Enumerable.Empty<Bill>();
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
                            var bill = new Bill();
                            bill.NextDue = DateTime.Today;
                            BillEditedEventArgs args = new BillEditedEventArgs
                            {
                                Bill = bill,
                                Completed = delegate
                                {
                                    _bills.OnSet();
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
                                },
                                Cancelled = delegate
                                {
                                }
                            };
                            BillEdited(this, args);
                        }
                    });
            }
        }
    }
}
