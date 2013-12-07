using BillTender.Budget.Models;

namespace BillTender.Budget.Mementos
{
    public class BillMemento
    {
        public string ObjectId { get; set; }
        public string Payee { get; set; }
        public Frequency Frequency { get; set; }
        public double Amount { get; set; }
    }
}
