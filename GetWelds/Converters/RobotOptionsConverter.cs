using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GetWelds.Converters
{
    public class RobotOptionsConverter : IMultiValueConverter
    {
     

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var robType = values[0];
            var options = values[1] as GetWelds.ViewModels.OptionsViewModel;

            if (values[0] is Robots.RobotBase)
                return null;

            if (values[0] is Robots.Fanuc)
                return options.RobotOptions.FanucOptions;

            if (values[0] is Robots.Kuka)
                return options.RobotOptions.KukaOptions;

            return Binding.DoNothing;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
