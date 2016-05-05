using System;
using System.Globalization;
using System.Windows.Data;

namespace Commander.Converters
{
    public class CultureToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as CultureInfo)?.Name.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}