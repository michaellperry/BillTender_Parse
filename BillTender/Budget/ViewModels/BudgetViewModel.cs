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

        public void Load()
        {
            Perform(async delegate
            {
                IList<Bill> bills;
                if (_user.TryGetValue<IList<Bill>>("Bills", out bills))
                {
                    var tasks = bills
                        .Select(bill => bill.FetchAsync());
                    await Task.WhenAll(tasks);
                }
            });
        }

        public IEnumerable<Bill> Bills
        {
            get
            {
                _bills.OnGet();
                IList<Bill> bills;
                if (_user.TryGetValue<IList<Bill>>("Bills", out bills))
                    return bills;
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
                            var bill = ParseObject.Create<Bill>();
                            BillEditedEventArgs args = new BillEditedEventArgs
                            {
                                Bill = bill,
                                Completed = delegate
                                {
                                    Perform(async delegate
                                    {
                                        await bill.SaveAsync();
                                        _user.AddToList("Bills", bill);
                                        await _user.SaveAsync();
                                        _bills.OnSet();
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
                                        await _selectedBill.Value.SaveAsync();
                                    });
                                },
                                Cancelled = delegate
                                {
                                    Perform(async delegate
                                    {
                                        await _selectedBill.Value.FetchAsync();
                                    });
                                }
                            };
                            BillEdited(this, args);
                        }
                    });
            }
        }
    }
}
