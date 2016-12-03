using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GetWelds.Robots;
using GetWelds.ViewModels;
namespace GetWelds.Converters
{
    public class ZoneFileTextConverter:IMultiValueConverter
    {


      
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (values[0]==null||values[1]==null)
                return Binding.DoNothing;
          
            var zip = values[1] as RobotZipFile;
            var zone = values[0] as Zone;


            // return if not Zip File
            if (zip == null||zone==null)
                return Binding.DoNothing;

            var text = zip.GetFileText(zone.EntryFilename);


            return text;
        }

  

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
