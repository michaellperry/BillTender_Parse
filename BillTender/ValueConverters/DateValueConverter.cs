using System;
using Windows.UI.Xaml.Data;

namespace BillTender.ValueConverters
{
    public class DateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("DateValueConverter can only be bound to string properties.");
            if (!(value is DateTime))
                throw new InvalidOperationException("DateValueConverter can only be used with DateTime properties.");

            string format = parameter as string;
            if (format != null)
                return ((DateTime)value).ToString(format);
            return ((DateTime)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            DateTime result;
            if (DateTime.TryParse(value as string, out result))
                return result;
            return default(DateTime);
        }
    }
}
