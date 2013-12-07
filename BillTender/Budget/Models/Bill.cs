using System;
using System.Linq;
using Parse;
using BillTender.Payments.Models;
using System.Collections.Generic;
using BillTender.Families.Models;
using BillTender.Budget.Mementos;

namespace BillTender.Budget.Models
{
    [ParseClassName("Bill")]
    public class Bill : ParseObject
    {
        [ParseFieldName("Family")]
        public Family Family
        {
            get { return GetProperty<Family>(); }
            set { SetProperty<Family>(value); }
        }

        [ParseFieldName("Payee")]
        public string Payee
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("Amount")]
        public double Amount
        {
            get { return GetProperty<double>(); }
            set { SetProperty<double>(value); }
        }

        [ParseFieldName("Frequency")]
        public Frequency Frequency
        {
            get
            {
                int value = GetProperty<int>();
                if (!IsValidFrequency(value))
                    return Frequency.Monthly;
                return (Frequency)value;
            }
            set { SetProperty<int>((int)value); }
        }

        [ParseFieldName("NextDue")]
        public DateTime NextDue
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty<DateTime>(value); }
        }

        [ParseFieldName("Payments")]
        public IList<Payment> Payments
        {
            get { return GetProperty<IList<Payment>>(); }
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

        public static Bill FromMemento(BillMemento memento)
        {
            var bill = ParseObject.Create<Bill>();
            bill.ObjectId = memento.ObjectId;
            bill.Payee = memento.Payee;
            bill.Frequency = memento.Frequency;
            bill.Amount = memento.Amount;
            return bill;
        }

        public BillMemento ToMemento()
        {
            return new BillMemento
            {
                ObjectId = ObjectId,
                Payee = Payee,
                Frequency = Frequency,
                Amount = Amount
            };
        }
    }
}
