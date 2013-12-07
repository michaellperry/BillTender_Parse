using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BillTender.Budget.Models;
using BillTender.Helpers;
using BillTender.ViewModels;
using Parse;
using UpdateControls.Collections;
using UpdateControls.Fields;
using UpdateControls.XAML;
using Windows.Storage;
using System.Xml.Serialization;
using BillTender.Budget.Mementos;

namespace BillTender.Budget.ViewModels
{
    public class BudgetViewModel : ProgressViewModel
    {
        private readonly BillTender.Families.Models.Family _family;
        private readonly BillSelectionModel _billSelection;
        
        public BudgetViewModel(
            BillTender.Families.Models.Family family,
            BillSelectionModel billSelection)
        {
            _family = family;
            _billSelection = billSelection;
        }

        public void Load()
        {
            Perform(async delegate
            {
                var billsCached = await LoadBillsAsync(_family.ObjectId);
                _billSelection.SetBills(billsCached);

                var results = await QueryBillsAsync(_family);
                _billSelection.SetBills(results);

                await SaveBillsAsync(
                    _family.ObjectId,
                    results);
            });
        }

        public IEnumerable<Bill> Bills
        {
            get { return _billSelection.Bills; }
        }

        public Bill SelectedBill
        {
            get { return _billSelection.SelectedBill; }
            set { _billSelection.SelectedBill = value; }
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

                                    _billSelection.AddBill(bill);
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
                    .When(() => _billSelection.SelectedBill != null)
                    .Do(delegate
                    {
                        var bill = _billSelection.SelectedBill;
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

        private static XmlSerializer BillSerializer =
            new XmlSerializer(typeof(BillListMemento));

        private static async Task<List<Bill>> LoadBillsAsync(
            string familyId)
        {
            try
            {
                string fileName = familyId + ".xml";
                var folder = await ApplicationData.Current.LocalFolder
                    .GetFolderAsync("Bills");
                var file = await folder.GetFileAsync(fileName);
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    var billList = (BillListMemento)BillSerializer
                        .Deserialize(stream);
                    return billList.Bills
                        .Select(memento => Bill.FromMemento(memento))
                        .ToList();
                }
            }
            catch (Exception x)
            {
                return new List<Bill>();
            }
        }

        private static async Task SaveBillsAsync(
            string familyId,
            List<Bill> bills)
        {
            string fileName = familyId + ".xml";
            var folder = await ApplicationData.Current.LocalFolder
                .CreateFolderAsync(
                    "Bills",
                    CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(
                fileName,
                CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                var memento = new BillListMemento
                {
                    Bills = bills
                        .Select(bill => bill.ToMemento())
                        .ToList()
                };
                BillSerializer.Serialize(stream, memento);
            }
        }

        private static async Task<List<Bill>> QueryBillsAsync(
            BillTender.Families.Models.Family family)
        {
            var query =
                from bill in new ParseQuery<Bill>()
                where bill.Family == family
                select bill;
            var results = await query.FindAsync();
            return results.ToList();
        }
    }
}
