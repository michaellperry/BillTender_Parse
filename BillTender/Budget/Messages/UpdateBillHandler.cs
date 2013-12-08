using System.Threading.Tasks;
using BillTender.Budget.Models;
using BillTender.Messaging;
using Parse;

namespace BillTender.Budget.Messages
{
    public class UpdateBillHandler : MessageHandler<UpdateBill>
    {
        protected override async Task Handle(UpdateBill message)
        {
            var bill = await new ParseQuery<Bill>()
                .Where(b => b.UniqueId == message.BillId)
                .FirstOrDefaultAsync();
            if (bill != null)
            {
                bill.Payee = message.Payee;
                bill.Amount = message.Amount;
                bill.Frequency = message.Frequency;
                bill.NextDue = message.NextDue;
                await bill.SaveAsync();
            }
        }
    }
}
