using System;
using System.Globalization;
using System.Windows.Data;

namespace GetWelds.Converters
{
    public class DoubleConverter:IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
return $"{value:0.##}";  
       
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
