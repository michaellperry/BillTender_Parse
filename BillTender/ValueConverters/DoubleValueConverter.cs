using System;
using Windows.UI.Xaml.Data;

namespace BillTender.ValueConverters
{
    public class DoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("DoubleValueConverter can only be bound to text properties.");
            if (!(value is double))
                throw new InvalidOperationException("DoubleValueConverter can only be used with double properties.");

            string format = parameter as string;
            if (format != null)
                return ((double)value).ToString(format);
            return ((double)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double result;
            if (Double.TryParse(value as string, out result))
                return result;
            return default(double);
        }
    }
}
