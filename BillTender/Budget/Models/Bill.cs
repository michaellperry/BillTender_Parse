using System;
using System.Linq;
using Parse;

namespace BillTender.Budget.Models
{
    [ParseClassName("Bill")]
    public class Bill : ParseObject
    {
        [ParseFieldName("Payee")]
        public string Payee
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("Amount")]
        public decimal Amount
        {
            get { return GetProperty<decimal>(); }
            set { SetProperty<decimal>(value); }
        }

        [ParseFieldName("Frequency")]
        public Frequency Frequency
        {
            get
            {
                int value = GetProperty<int>();
                if (IsValidFrequency(value))
                    return (Frequency)value;
                return Frequency.Monthly;
            }
            set { SetProperty<int>((int)value); }
        }

        [ParseFieldName("NextDue")]
        public DateTime NextDue
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty<DateTime>(value); }
        }

        public static bool IsValidFrequency(int value)
        {
            return Enum
                .GetValues(typeof(Frequency))
                .OfType<Frequency>()
                .Select(v => (int)v)
                .Contains(value);
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            Bill that = obj as Bill;
            if (that == null)
                return false;
            return Object.Equals(this.ObjectId, that.ObjectId);
        }

        public override int GetHashCode()
        {
            return ObjectId.GetHashCode();
        }
    }
}
