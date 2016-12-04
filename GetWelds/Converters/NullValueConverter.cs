using System;
using System.Windows.Data;

namespace GetWelds.Converters
{
    public class NullValueConverter:IValueConverter
    {
       

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return (int)value == -1 ? string.Empty : value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
