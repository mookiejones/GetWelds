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

        private const string LINE_STRING = "Line";

        private string _line;

        public string Line
        {
            get { return _line; }
            set { _line = value; RaisePropertyChanged(LINE_STRING); }
        }

        #endregion Line

        #region IsServoWeld

        private const string IS_SERVO_STRING = "IsServoWeld";

        private bool _isServo;

        public bool IsServoWeld
        {
            get { return _isServo; }
            set { _isServo = value; RaisePropertyChanged(IS_SERVO_STRING); }
        }

        #endregion IsServoWeld

        #region IsSpotWeld

        private const string IS_SPOT_STRING = "IsSpotWeld";

        private bool _isSpot;

        public bool IsSpotWeld
        {
            get { return _isSpot; }
            set { _isSpot = value; RaisePropertyChanged(IS_SPOT_STRING); }
        }

        #endregion IsSpotWeld

        #region Gun1

        private const string GUN1_STRING = "Gun1";

        private string _gun1;

        public string Gun1
        {
            get { return _gun1; }
            set { _gun1 = value; RaisePropertyChanged(GUN1_STRING); }
        }

        #endregion Gun1

        #region Gun2

        private const string GUN2_STRING = "Gun2";

        private string _gun2;

        public string Gun2
        {
            get { return _gun2; }
            set { _gun2 = value; RaisePropertyChanged(GUN2_STRING); }
        }

        #endregion Gun2

        #region Gun3

        private const string GUN3_STRING = "Gun3";

        private string _gun3;

        public string Gun3
        {
            get { return _gun3; }
            set { _gun3 = value; RaisePropertyChanged(GUN3_STRING); }
        }

        #endregion Gun3

        #region Gun4

        private const string GUN4_STRING = "Gun4";

        private string _gun4;

        public string Gun4
        {
            get { return _gun4; }
            set { _gun4 = value; RaisePropertyChanged(GUN4_STRING); }
        }

        #endregion Gun4

        #region Force

        private const string FORCE_STRING = "Force";

        private string _force;

        public string Force
        {
            get { return _force; }
            set { _force = value; RaisePropertyChanged(FORCE_STRING); }
        }

        #endregion Force

        #region Thickness

        private const string THICKNESS_STRING = "Thickness";

        private string _thickness;

        public string Thickness
        {
            get { return _thickness; }
            set { _thickness = value; RaisePropertyChanged(THICKNESS_STRING); }
        }

        #endregion Thickness

        

        #region StartDistance
        /// <summary>
        /// The <see cref="StartDistance" /> property's name.
        /// </summary>
        public const string START_DISTANCE_PROPERTY_NAME = "StartDistance";

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

                RaisePropertyChanging(START_DISTANCE_PROPERTY_NAME);
                _startDistance = value;
                RaisePropertyChanged(START_DISTANCE_PROPERTY_NAME);
            }
        }
        #endregion

        

        #region EndDistance
        /// <summary>
        /// The <see cref="EndDistance" /> property's name.
        /// </summary>
        public const string END_DISTANCE_PROPERTY_NAME = "EndDistance";

        private int _endDistance;

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

                RaisePropertyChanging(END_DISTANCE_PROPERTY_NAME);
                _endDistance = value;
                RaisePropertyChanged(END_DISTANCE_PROPERTY_NAME);
            }
        }
        #endregion
        

        #region Sequence

        private const string SEQUENCE_STRING = "Sequence";

        private int _sequence = -1;

        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; RaisePropertyChanged(SEQUENCE_STRING); }
        }

        #endregion Sequence

        #region ID

        private const string ID_STRING = "ID";

        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(ID_STRING); }
        }

        #endregion ID

        #region Schedule

        private const string SCHEDULE_STRING = "Schedule";

        private string _schedule;

        public string Schedule
        {
            get { return _schedule; }
            set { _schedule = value; RaisePropertyChanged(SCHEDULE_STRING); }
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
