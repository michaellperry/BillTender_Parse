using System;
using System.Collections.Generic;
using System.Windows.Input;
using BillTender.Budget.Models;
using BillTender.Helpers;
using BillTender.ViewModels;
using Parse;
using UpdateControls.Collections;
using UpdateControls.Fields;
using UpdateControls.XAML;

namespace BillTender.Budget.ViewModels
{
    public class BudgetViewModel : ProgressViewModel
    {
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
                var query =
                    from bill in new ParseQuery<Bill>()
                    where bill.Family == _family
                    select bill;
                var results = await query.FindAsync();
                foreach (var bill in results)
                    _bills.Add(bill);
            });
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

        public ICommand NewBill
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        var bill = ParseObject.Create<Bill>();
						bill.Family = _family;
                        bill.NextDue = DateTime.Today;
                        DialogManager.ShowBillDialog(bill,
                            completed: delegate
                            {
                                Perform(async delegate
                                {
                                    bill.ACL = _family.ACL;
                                    await bill.SaveAsync();

                                    var members =
                                        _family.Readers.Users.Query.Or(
                                        _family.Writers.Users.Query);
                                    var installations =
                                        from member in members
                                        join installation in
                                            ParseInstallation.Query
                                            on member equals installation["user"]
                                        select installation;

                                    var push = new ParsePush
                                    {
                                        Query = installations,
                                        Alert = "Bill added (from client) for " + bill.Payee
                                    };
                                    await push.SendAsync();

                                    _bills.Add(bill);
                                });
                            });
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
                        var bill = _selectedBill.Value;
                        DialogManager.ShowBillDialog(
                            bill,
                            completed: delegate
                            {
                                Perform(async delegate
                                {
                                    await bill.SaveAsync();
                                });
                            },
                            cancelled: delegate
                            {
                                bill.Revert();
                            });
                    });
            }
        }
    }
}
