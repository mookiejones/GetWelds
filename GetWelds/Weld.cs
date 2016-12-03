using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GetWelds
{   [XmlInclude(typeof(Position))]
    public class Weld : Position
    {
        #region · Fields ·
        private string _line = string.Empty;
        private bool _isServo = false;
        private bool _isSpot = false;
        private string _gun1 = string.Empty;


        private string _gun2 = string.Empty;

        private string _gun3 = string.Empty;
        private string _gun4 = string.Empty;

        private string _force = string.Empty;

        private string _thickness = string.Empty;

        private int _sequence = -1;


        private string _id = string.Empty;

        private string _schedule = string.Empty;
       
        #endregion

        #region · Properties ·
        public bool IsSpotWeld { get { return _isSpot; } set { _isSpot = value; RaisePropertyChanged("IsSpotWeld"); } }
        public bool IsServoWeld { get { return _isServo; } set { _isServo = value; RaisePropertyChanged("IsServo"); } }
        public string Line { get { return _line; } set { _line = value; RaisePropertyChanged("Line"); } }
        public string Gun1 { get { return _gun1; } set { _gun1 = value; RaisePropertyChanged("Gun1"); } }
        public string Gun2 { get { return _gun2; } set { _gun2 = value; RaisePropertyChanged("Gun2"); } }
        public string Gun3 { get { return _gun3; } set { _gun3 = value; RaisePropertyChanged("Gun3"); } }
        public string Gun4 { get { return _gun4; } set { _gun4 = value; RaisePropertyChanged("Gun4"); } }
        public string Force { get { return _force; } set { _force = value; RaisePropertyChanged("Force"); } }
        public string Thickness { get { return _thickness; } set { _thickness = value; RaisePropertyChanged("Thickness"); } }
        public int Sequence { get { return _sequence; } set { _sequence = value; RaisePropertyChanged("Sequence"); } }
        public string ID { get { return _id; } set { _id = value; RaisePropertyChanged("ID"); } }
        public string Schedule { get { return _schedule; } set { _schedule = value; RaisePropertyChanged("Schedule"); } }
        public bool OpenBackup1 { get; set; }
        public bool OpenBackup2 { get; set; }
        public bool OpenBackup3 { get; set; }
        public bool OpenBackup4 { get; set; }

        public bool UseGun1 { get; set; }
        public bool Equalizer1 { get; set; }
        public bool UseGun2 { get; set; }
        public bool Equalizer2 { get; set; }
        public bool UseGun3 { get; set; }
        public bool Equalizer3 { get; set; }
        public bool UseGun4 { get; set; }
        public bool Equalizer4 { get; set; }

        public bool Anticipate { get; set; }
        public WeldGunType GunType { get { return _weldGunType; } set { _weldGunType = value; RaisePropertyChanged("GunType"); } }

        #endregion


        public Weld(string filename,int style):base(filename,style) { }

        public Weld(string line, int linenumber, int sequence,string filename,int style):base(filename,style)
        {
            // Is Line A WeldGun
            IsServoWeld = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.IsServoWeldKuka, line).Length > 0;
            IsSpotWeld = !IsServoWeld;
            Line = line;
            //TODO Need to be able to determine if Weld is Spot or servo
            Name = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.KukaWeldNameRegex, line);

            Sequence = sequence;


            Velocity = Convert.ToDouble(GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.VelocityRegex, line));
            LineNumber = linenumber;
            Schedule = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.WeldScheduleRegex, line);
            ID = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.WeldIdRegex, line);
            Thickness = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.WeldThicknessRegex, line);
            Force = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.WeldForceRegex, line);

            Gun1 = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.Gun1Regex, line);
            Gun2 = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.Gun2Regex, line);
            Gun3 = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.Gun3Regex, line);
            Gun4 = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.Gun4Regex, line);

            Equalizer1 = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.Eqlzr1Regex, line).Trim() == "X";
            Equalizer2 = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.Eqlzr2Regex, line).Trim() == "X";
            Equalizer3 = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.Eqlzr3Regex, line).Trim() == "X";
            Equalizer4 = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.Eqlzr4Regex, line).Trim() == "X";

            var antic = GetWeldViewModel.GetRegexMatch(Properties.Settings.Default.AnticpRegex, line);
        }




        public Weld() { }




        private WeldGunType _weldGunType = WeldGunType.None;


    }
   public enum WeldGunType { Servo, Spot, None };
}
