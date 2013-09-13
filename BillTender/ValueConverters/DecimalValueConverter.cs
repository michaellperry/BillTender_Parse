using System;
using Windows.UI.Xaml.Data;

namespace BillTender.ValueConverters
{
    public class DecimalValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("DecimalValueConverter can only be bound to text properties.");
            if (!(value is decimal))
                throw new InvalidOperationException("DecimalValueConverter can only be used with decimal properties.");

            string format = parameter as string;
            if (format != null)
                return ((decimal)value).ToString(format);
            return ((decimal)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            decimal result;
            if (Decimal.TryParse(value as string, out result))
                return result;
            return default(decimal);
        }
    }
}
