using System;
using System.Globalization;
using System.Windows.Data;
using WPFLocalizeExtension.Engine;

namespace Commander.Converters
{
    public class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime) value).ToString(LocalizeDictionary.Instance.Culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}