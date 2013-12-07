using System;
using BillTender.Budget.Models;
using BillTender.Messaging;

namespace BillTender.Budget.Messages
{
    public class UpdateBill : Message
    {
        public string BillId { get; set; }
        public string Payee { get; set; }
        public double Amount { get; set; }
        public Frequency Frequency { get; set; }
        public DateTime NextDue { get; set; }
    }
}
