using System;
using BillTender.Budget.Models;
using Windows.UI.Xaml.Data;

namespace BillTender.ValueConverters
{
    public class FrequencyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(int))
                throw new InvalidOperationException("FrequencyValueConverter can only be bound to integer properties.");
            if (!(value is Frequency))
                throw new InvalidOperationException("FrequencyValueConverter can only be used with Frequency properties.");

            return (int)(Frequency)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (Frequency)(int)value;
        }
    }
}
