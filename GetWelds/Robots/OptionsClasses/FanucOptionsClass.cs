using GetWelds.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetWelds.Robots.OptionsClasses
{
   public class FanucOptionsClass:IOptionsClass
    {
       private string _styleName = @"PG[0-9]+.ls";


        public  string StyleName
        {
            get
            {
                return _styleName;
            }
            set
            {
                _styleName = value;
            }
        }

        private string _studString = "[0-9:]*L P\\[[0-9:;A-Z-a-z[;-]+]* [0-9]+mm/sec [^ ]* RTCP\\s;";

        public  string StudString
        {
            get
            {
                return _studString;
            }
            set
            {
                _studString = value;
            }
        }

        private string _linearString= string.Empty;

        public  string LinearString
        {
            get
            {
                return _linearString;
            }
            set
            {
                _linearString = value;
            }
        }

        private  string _jointString = string.Empty;
        public  string JointString
        {
            get
            {
                return _jointString;
            }
            set
            {
                _jointString = value;
            }
        }

        private string _rivetString= "[0-9:]*L P\\[[0-9:;A-Z-a-z[;-]+]* [0-9]+mm/sec [^ ]* RTCP\\s;";

        public  string RivetString
        {
            get
            {
                return _rivetString;
            }
            set
            {
                _rivetString = value;
            }
        }

        private string _nutweldString = string.Empty;
        public  string NutWeldString
        {
            get
            {
                return _nutweldString;
            }
            set
            {
                _nutweldString = value;
            }
        }


        private string _servoweldString = "([0-9]+):[J|L]{1} +P\\[([0-9:1-z]+)/] +([0-9]+)mm/sec +[0-9a-zA-Z]*[^:]*: *SPOT\\[SD=([0-9]{1,2}),P=([0-9]{1,2}),S=([0-9]{1,2}),ED=([0-9]{1,2})/] *;";
        public string ServoWeldString
        {
            get
            {
                return _servoweldString;

            }
            set
            {
                if (_servoweldString == value)
                    return;
                _servoweldString = value;
            }
        }

        private string _airGunString = string.Empty;
        public string AirGunString { get { return _airGunString; } }
   }
}
