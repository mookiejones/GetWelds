using System;
using System.Globalization;
using System.Windows.Data;

namespace GetWelds.Converters
{
    public class DoubleConverter:IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
return            String.Format("{0:0.##}",value);  
       
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
