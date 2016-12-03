using GetWelds.Robots;
using System;
using System.Xml.Serialization;

namespace GetWelds
{


    /// <remarks>Abstract Weld Class for different types of welds</remarks>
    
    [XmlInclude(typeof(Weld))]
    [XmlInclude(typeof(Fanuc.RobotWeld))]
    [XmlInclude(typeof(Kuka.RobotWeld))]
    public abstract class AbstractWeld:Position
    {
        
        #region · Properties ·
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

        

        #region StartDistance
        /// <summary>
        /// The <see cref="StartDistance" /> property's name.
        /// </summary>
        public const string StartDistancePropertyName = "StartDistance";

        private int _startDistance = -1;

        /// <summary>
        /// Sets and gets the StartDistance property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int StartDistance
        {
            get
            {
                return _startDistance;
            }

            set
            {
                if (_startDistance == value)
                {
                    return;
                }

                RaisePropertyChanging(StartDistancePropertyName);
                _startDistance = value;
                RaisePropertyChanged(StartDistancePropertyName);
            }
        }
        #endregion

        

        #region EndDistance
        /// <summary>
        /// The <see cref="EndDistance" /> property's name.
        /// </summary>
        public const string EndDistancePropertyName = "EndDistance";

        private int _endDistance = 0;

        /// <summary>
        /// Sets and gets the EndDistance property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int EndDistance
        {
            get
            {
                return _endDistance;
            }

            set
            {
                if (_endDistance == value)
                {
                    return;
                }

                RaisePropertyChanging(EndDistancePropertyName);
                _endDistance = value;
                RaisePropertyChanged(EndDistancePropertyName);
            }
        }
        #endregion
        

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


        private WeldGunType _weldGunType = WeldGunType.None;
        public WeldGunType GunType { get { return _weldGunType; } set { _weldGunType = value; RaisePropertyChanged("GunType"); } }

        
        #endregion

        protected AbstractWeld()
        {
        }

        public abstract Position Add(string line, int i, int position, string name, int style);

        protected AbstractWeld(string filename, int style)
            : base(filename, style)
        {
            Filename = filename;
            Style = style;
        }

     
        
    }

    public class BaseWeld : AbstractWeld
    {
        public override Position Add(string line, int i, int position, string name, int style)
        {
            throw new NotImplementedException();
        }
    }
}
