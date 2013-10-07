using System;
using System.Linq;

namespace BillTender.Budget.Models
{
    public class Bill
    {
        public string Payee { get; set; }
        public double Amount { get; set; }
        public Frequency Frequency { get; set; }
        public DateTime NextDue { get; set; }

        public static bool IsValidFrequency(int value)
        {
            return Enum
                .GetValues(typeof(Frequency))
                .OfType<Frequency>()
                .Select(v => (int)v)
                .Contains(value);
        }
    }
}
