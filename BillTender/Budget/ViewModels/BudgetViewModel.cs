using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BillTender.Budget.Models;
using BillTender.ViewModels;
using Parse;
using UpdateControls;
using UpdateControls.XAML;

namespace BillTender.Budget.ViewModels
{
    public class BudgetViewModel : ProgressViewModel
    {
        private readonly ParseUser _user;

        private Independent _bills = new Independent();

        public BudgetViewModel(ParseUser user)
        {
            _user = user;
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

        public ICommand NewBill
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        Perform(async delegate
                        {
                            var bill = ParseObject.Create<Bill>();
                            await bill.SaveAsync();
                            _user.AddToList("Bills", bill);
                            await _user.SaveAsync();
                            _bills.OnSet();
                        });
                    });
            }
        }
    }
}
