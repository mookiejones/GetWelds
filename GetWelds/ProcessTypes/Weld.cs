using System;
using System.Xml.Serialization;
using GetWelds.Properties;
using GetWelds.ViewModels;

namespace GetWelds
{
    [XmlInclude(typeof(Position))]
    public class Weld :AbstractWeld
    {
        #region Line

        private const string LineString = "Line";

        private string _line;

        public string Line
        {
            get { return _line; }
            set { _line = value; RaisePropertyChanged(LineString); }
        }

        #endregion Line

        #region IsServoWeld

        private const string IsServoString = "IsServoWeld";

        private bool _isServo;

        public bool IsServoWeld
        {
            get { return _isServo; }
            set { _isServo = value; RaisePropertyChanged(IsServoString); }
        }

        #endregion IsServoWeld

        #region IsSpotWeld

        private const string IsSpotString = "IsSpotWeld";

        private bool _isSpot;

        public bool IsSpotWeld
        {
            get { return _isSpot; }
            set { _isSpot = value; RaisePropertyChanged(IsSpotString); }
        }

        #endregion IsSpotWeld

        #region Gun1

        private const string Gun1String = "Gun1";

        private string _gun1;

        public string Gun1
        {
            get { return _gun1; }
            set { _gun1 = value; RaisePropertyChanged(Gun1String); }
        }

        #endregion Gun1

        #region Gun2

        private const string Gun2String = "Gun2";

        private string _gun2;

        public string Gun2
        {
            get { return _gun2; }
            set { _gun2 = value; RaisePropertyChanged(Gun2String); }
        }

        #endregion Gun2

        #region Gun3

        private const string Gun3String = "Gun3";

        private string _gun3;

        public string Gun3
        {
            get { return _gun3; }
            set { _gun3 = value; RaisePropertyChanged(Gun3String); }
        }

        #endregion Gun3

        #region Gun4

        private const string Gun4String = "Gun4";

        private string _gun4;

        public string Gun4
        {
            get { return _gun4; }
            set { _gun4 = value; RaisePropertyChanged(Gun4String); }
        }

        #endregion Gun4

        #region Force

        private const string ForceString = "Force";

        private string _force;

        public string Force
        {
            get { return _force; }
            set { _force = value; RaisePropertyChanged(ForceString); }
        }

        #endregion Force

        #region Thickness

        private const string ThicknessString = "Thickness";

        private string _thickness;

        public string Thickness
        {
            get { return _thickness; }
            set { _thickness = value; RaisePropertyChanged(ThicknessString); }
        }

        #endregion Thickness

        #region Sequence

        private const string SequenceString = "Sequence";

        private int _sequence = -1;

        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; RaisePropertyChanged(SequenceString); }
        }

        #endregion Sequence

        #region ID

        private const string IDString = "ID";

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(IDString); }
        }

        #endregion ID

        #region Schedule

        private const string ScheduleString = "Schedule";

        private string _schedule;

        public string Schedule
        {
            get { return _schedule; }
            set { _schedule = value; RaisePropertyChanged(ScheduleString); }
        }

        #endregion Schedule

        #region · Properties ·

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

        #endregion · Properties ·

        public Weld() { }
        public Weld(string filename, int style)
            : base(filename, style)
        {
            Filename = filename;
        }

        public Weld(string line, int linenumber, int sequence, string filename, int style)
            : base(filename, style)
        {
            // Is Line A WeldGun
            IsServoWeld = GetWeldViewModel.GetRegexMatch(Settings.Default.IsServoWeldKuka, line).Length > 0;
            IsSpotWeld = !IsServoWeld;
            Line = line;
            //TODO Need to be able to determine if Weld is Spot or servo
            Name = GetWeldViewModel.GetRegexMatch(Settings.Default.KukaWeldNameRegex, line);

            Sequence = sequence;
            Filename = filename;

            Velocity = Convert.ToDouble(GetWeldViewModel.GetRegexMatch(Settings.Default.VelocityRegex, line));
            LineNumber = linenumber;
            Schedule = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldScheduleRegex, line);
            ID = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldIdRegex, line);
            Thickness = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldThicknessRegex, line);
            Force = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldForceRegex, line);

            Gun1 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun1Regex, line);
            Gun2 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun2Regex, line);
            Gun3 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun3Regex, line);
            Gun4 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun4Regex, line);

            Equalizer1 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr1Regex, line).Trim() == "X";
            Equalizer2 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr2Regex, line).Trim() == "X";
            Equalizer3 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr3Regex, line).Trim() == "X";
            Equalizer4 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr4Regex, line).Trim() == "X";

            var antic = GetWeldViewModel.GetRegexMatch(Settings.Default.AnticpRegex, line);
        }

      
        private WeldGunType _weldGunType = WeldGunType.None;

        public override Position Add(string line, int i, int position, string name, int style)
        {
            throw new NotImplementedException();
        }
    }

    public enum WeldGunType { Servo, Spot, None };
}