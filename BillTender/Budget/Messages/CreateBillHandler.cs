using System.Threading.Tasks;
using BillTender.Budget.Models;
using BillTender.Families.Models;
using BillTender.Messaging;
using Parse;

namespace BillTender.Budget.Messages
{
    public class CreateBillHandler : MessageHandler<CreateBill>
    {
        protected override async Task Handle(CreateBill message)
        {
            var bill = ParseObject.Create<Bill>();
            Family family = ParseObject.CreateWithoutData<Family>(
                message.FamilyId);
            await family.FetchAsync();
            bill.Family = family;
            bill.UniqueId = message.BillId;
            bill.Payee = message.Payee;
            bill.Amount = message.Amount;
            bill.Frequency = message.Frequency;
            bill.NextDue = message.NextDue;
            bill.ACL = family.ACL;
            await bill.SaveAsync();
        }
    }
}
