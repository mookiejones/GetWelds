using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using GetWelds.ViewModels;
using GetWelds.Properties;
using Ionic.Zip;
using GetWelds.Converters;
using GetWelds.Helpers;

namespace GetWelds.Robots
{
    [System.Xml.Serialization.XmlInclude(typeof(Kuka))]
    [System.Xml.Serialization.XmlInclude(typeof(Fanuc))]
    [Serializable]
    public abstract class AbstractRobot : ViewModelBase
    {

        #region · Constructor ·

      

        /// <summary>
        /// Robot with Zip File
        /// </summary>
        /// <param name="zip"></param>
        protected AbstractRobot(RobotZipFile zip)
        {
            Zip = zip;
            ParseZipFile();
        }

        public abstract void GetMainPrograms(string regexstring);



        /// <summary>
        /// Robot
        /// <remarks>Parse Robot Directory</remarks>
        /// </summary>
        /// <param name="dir"></param>
        protected AbstractRobot(DirectoryInfo dir)
        {

        }

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        protected AbstractRobot()
        {
        }

        /// <summary>
        /// Robot with list of files
        /// </summary>
        /// <param name="files"></param>
        protected AbstractRobot(IEnumerable<FileInfo> files)
        {
            var options = new ReadOptions();
        }
        #endregion

        protected string GetStyleNumber(string stylestring)
        {
            var regex = GetRegex(Settings.Default.STYLEEXTRACTREGEX);

            var match = regex.Match(stylestring);

            return match.Groups[1].ToString();

        }

        public abstract Type Weld { get; }

        public abstract void ProcessRobotFiles();

        public void GetSystems(List<RobotFile> files)
        {

            Files = files;
            GetRobotName();
            GetMainPrograms(MainRegexString);
            GetConfigData();

            GetProcessData();

            Console.WriteLine("Processing Files for {0}", Name);
            ProcessRobotFiles();

       
        }


        public void GetSystems(RobotZipFile zip)
        {
            Zip = zip;
            GetMainPrograms(MainRegexString);
            GetConfigData();
            GetProcessData();
            Console.WriteLine("Processing Files for {0}", Name);
            ProcessRobotFiles();

        }

        public abstract void ParseZipFiles(IEnumerable<FileInfo> zips);

        #region · Properties ·


        protected abstract string MainRegexString { get; }

        #region · PositionCollection ·

        /// <summary>
        /// The <see cref="PositionCollection" /> property's name.
        /// </summary>
        public const string POSITION_COLLECTION_PROPERTY_NAME = "PositionCollection";


        private List<AbstractWeld> _positionCollection = new List<AbstractWeld>();

        /// <summary>
        /// Sets and gets the PositionCollection property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public List<AbstractWeld> PositionCollection
        {
            get
            {
                var positions = Positions.OfType<AbstractWeld>();
                return positions.ToList();
            }
        }

        #endregion

        public ObservableCollection<AbstractWeld> Positions { get; set; }= new ObservableCollection<AbstractWeld>();
   


        #region Files
        /// <summary>
        /// The <see cref="Files" /> property's name.
        /// </summary>
        public const string FILES_PROPERTY_NAME = "Files";

        private List<RobotFile> _filesInfos;

        /// <summary>
        /// Sets and gets the Files property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
      
        public List<RobotFile> Files
        {
            get
            {
                return _filesInfos;
            }

            set
            {
                if (_filesInfos == value)
                {
                    return;
                }

                RaisePropertyChanging(FILES_PROPERTY_NAME);
                _filesInfos = value;
                RaisePropertyChanged(FILES_PROPERTY_NAME);
            }
        }
        #endregion



        public ObservableCollection<Tool> Tools { get; set; } = new ObservableCollection<Tool>();

        #region Zip

        /// <summary>
        /// The <see cref="Zip" /> property's name.
        /// </summary>
        public const string ZIP_PROPERTY_NAME = "Zip";

        private RobotZipFile _zip = default(RobotZipFile);

        /// <summary>
        /// Sets and gets the Zip property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public RobotZipFile Zip
        {
            get
            {
                return _zip;
            }
            set
            {
                Set(ZIP_PROPERTY_NAME, ref _zip, value);
            }
        }

        #endregion Zip

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NAME_PROPERTY_NAME = "Name";

        private string _name = string.Empty;

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Set(() => Name, ref _name, value, true);
            }
        }

        #region Process2

        /// <summary>
        /// The <see cref="Process2" /> property's name.
        /// </summary>
        public const string PROCESS2_PROPERTY_NAME = "Process2";

        private string _process2 = string.Empty;

        /// <summary>
        /// Sets and gets the Process2 property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Process2
        {
            get
            {
                return _process2;
            }

            set
            {
                if (_process2 == value)
                {
                    return;
                }

                RaisePropertyChanging(PROCESS2_PROPERTY_NAME);
                _process2 = value;
                RaisePropertyChanged(PROCESS2_PROPERTY_NAME);
            }
        }

        #endregion Process2

        #region Process1

        /// <summary>
        /// The <see cref="Process1" /> property's name.
        /// </summary>
        public const string PROCESS1_PROPERTY_NAME = "Process1";

        private string _process1 = string.Empty;

        /// <summary>
        /// Sets and gets the Process1 property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Process1
        {
            get
            {
                return _process1;
            }

            set
            {
                if (_process1 == value)
                {
                    return;
                }

                RaisePropertyChanging(PROCESS1_PROPERTY_NAME);
                _process1 = value;
                RaisePropertyChanged(PROCESS1_PROPERTY_NAME);
            }
        }

        #endregion Process1




        public ObservableCollection<SearchParam> OptionalValues { get; set; } = new ObservableCollection<SearchParam>();

       
        

        #endregion · Properties ·

        #region · Fields ·

        [Annotations.NotNull]
        private readonly List<int> _ignoredStyles = new List<int> { 41, 40, 42, 43, 44, 45, 46, 47, 48, 49, 50, 57, 58, 59, 60, 61, 62, 63, 64, 65 };

        internal IEnumerable<RobotFile> ProcessableFiles;


        #endregion · Fields ·

        #region · SelectedStyle ·

        /// <summary>
        /// The <see cref="SelectedStyle" /> property's name.
        /// </summary>
        public const string SELECTED_STYLE_PROPERTY_NAME = "SelectedStyle";

        private StyleProgramViewModel _selectedStyle = new StyleProgramViewModel();

        /// <summary>
        /// Sets and gets the SelectedStyle property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public StyleProgramViewModel SelectedStyle
        {
            get
            {
                return _selectedStyle;
            }
            set
            {
                Set(SELECTED_STYLE_PROPERTY_NAME, ref _selectedStyle, value, true);
            }
        }

        #endregion · SelectedStyle ·        

        #region Styles
        /// <summary>
        /// The <see cref="Styles" /> property's name.
        /// </summary>
        public const string STYLES_PROPERTY_NAME = "Styles";

        private List<StyleProgramViewModel> _styles = new List<StyleProgramViewModel>();

        /// <summary>
        /// Sets and gets the Styles property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<StyleProgramViewModel> Styles
        {
            get
            {
                return _styles;
            }

            set
            {
                if (_styles == value)
                {
                    return;
                }

                RaisePropertyChanging(STYLES_PROPERTY_NAME);
                _styles = value;
                RaisePropertyChanged(STYLES_PROPERTY_NAME);
            }
        }
        #endregion
        



        internal void Initialize()
        {
        }

        protected abstract void GetRobotName();


        //TODO Get Tool Data
        public void ParseZipFile()
        {
            var options = new ReadOptions();
            GetRobotName();
            GetMainPrograms(MainRegexString);
            GetConfigData();
            GetProcessData();
        }

        protected abstract void GetProcessData();

        /// <summary>
        /// Get Tool Information for robot
        /// </summary>
        protected abstract void GetConfigData();


        public abstract Position ParsePosition(string line, int linenumber, string file, int style);


        internal bool IsIgnored(int style)
        {
            return _ignoredStyles.Any(i => i == style);
        }

      
        protected abstract void AddStylePrograms(IEnumerable<string> filenames, int style, string programname);


        internal string TempPath = string.Empty;

        protected abstract void GetStyleProgram(string programname, int stylenumber);


        private void SerializePrograms()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<StyleProgramViewModel>));
                using (var stream = new StreamWriter("styles.xml"))
                {
                    serializer.Serialize(stream, Styles);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Parses parameters into a weld
        /// </summary>
        /// <param name="match"></param>
        /// <param name="linenumber"></param>
        /// <param name="sequence"></param>
        /// <param name="filename"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public abstract AbstractWeld GetWeld(Match match, int linenumber, int sequence, string filename, int style);
        /// <summary>
        /// Parses parameters into a weld
        /// </summary>
        /// <param name="line"></param>
        /// <param name="linenumber"></param>
        /// <param name="sequence"></param>
        /// <param name="filename"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public abstract AbstractWeld GetStudWeld(string line, int linenumber, int sequence, string filename, int style);

        protected Regex GetRegex(string regexstring)
        {
            return new Regex(regexstring, RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        public void ParseRobot(RobotZipFile zip)
        {
            
        }

        public void ParseRobot(IEnumerable<RobotFile> files)
        {
            // Get files available to process
            var _files = from file in files
                         where file.Extension.ToLowerInvariant() == FileExtension
                         select file;
            ProcessableFiles = _files;
        }
        public ObservableCollection<Zone> Zones { get; set; }= new ObservableCollection<Zone>();
        
        public abstract Regex PtpWeldRegex { get; }
        public abstract Regex LinWeldRegex { get; }
        public abstract Regex LinStudWeldRegex { get; }
        public abstract Regex PtpStudWeldRegex { get; }


        public abstract Regex RivetWeldRegex { get; }

        public abstract string FileExtension { get; }


        /// <summary>
        /// The <see cref="SelectedZone" /> property's name.
        /// </summary>
        public const string SELECTED_ZONE_PROPERTY_NAME = "SelectedZone";

        private Zone _selectedZone;

        /// <summary>
        /// Sets and gets the SelectedZone property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Zone SelectedZone
        {
            get
            {
                return _selectedZone;
            }

            set
            {
                if (_selectedZone == value)
                {
                    return;
                }

                RaisePropertyChanging(SELECTED_ZONE_PROPERTY_NAME);
                _selectedZone = value;
                RaisePropertyChanged(SELECTED_ZONE_PROPERTY_NAME);
            }
        }


        #region RobotOptions
        /// <summary>
        /// The <see cref="RobotOptions" /> property's name.
        /// </summary>
        public const string ROBOT_OPTIONS_PROPERTY_NAME = "RobotOptions";

        private OptionsClass _robotOptions = new OptionsClass();

        /// <summary>
        /// Sets and gets the RobotOptions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public OptionsClass RobotOptions
        {
            get
            {
                return _robotOptions;
            }

            set
            {
                if (_robotOptions == value)
                {
                    return;
                }

                RaisePropertyChanging(ROBOT_OPTIONS_PROPERTY_NAME);
                _robotOptions = value;
                RaisePropertyChanged(ROBOT_OPTIONS_PROPERTY_NAME);
            }
        }
        #endregion
        


    }


    public class Zone:ViewModelBase
    {


        #region Declaration
        /// <summary>
        /// The <see cref="Declaration" /> property's name.
        /// </summary>
        public const string DECLARATION_PROPERTY_NAME = "Declaration";

        private string _declaration = string.Empty;

        /// <summary>
        /// Sets and gets the Declaration property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Declaration
        {
            get
            {
                return _declaration;
            }

            set
            {
                if (_declaration == value)
                {
                    return;
                }

                RaisePropertyChanging(DECLARATION_PROPERTY_NAME);
                _declaration = value;
                RaisePropertyChanged(DECLARATION_PROPERTY_NAME);
            }
        }
        #endregion

        

        #region ZoneFilePosition
        /// <summary>
        /// The <see cref="ZoneFilePosition" /> property's name.
        /// </summary>
        public const string ZONE_FILE_POSITION_PROPERTY_NAME = "ZoneFilePosition";

        private int _zoneFilePosition = -1;

        /// <summary>
        /// Sets and gets the ZoneFilePosition property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ZoneFilePosition
        {
            get
            {
                return _zoneFilePosition;
            }

            set
            {
                if (_zoneFilePosition == value)
                {
                    return;
                }

                RaisePropertyChanging(ZONE_FILE_POSITION_PROPERTY_NAME);
                _zoneFilePosition = value;
                RaisePropertyChanged(ZONE_FILE_POSITION_PROPERTY_NAME);
            }
        }
        #endregion
        

        #region EntryFilename
        /// <summary>
        /// The <see cref="EntryFilename" /> property's name.
        /// </summary>
        public const string ENTRY_FILENAME_PROPERTY_NAME = "EntryFilename";

        private string _entryFilename = string.Empty;

        /// <summary>
        /// Sets and gets the EntryFilename property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string EntryFilename
        {
            get
            {
                return _entryFilename;
            }

            set
            {
                if (_entryFilename == value)
                {
                    return;
                }

                RaisePropertyChanging(ENTRY_FILENAME_PROPERTY_NAME);
                _entryFilename = value;
                RaisePropertyChanged(ENTRY_FILENAME_PROPERTY_NAME);
            }
        }
        #endregion
        

        #region LineBefore
        /// <summary>
        /// The <see cref="LineBefore" /> property's name.
        /// </summary>
        public const string LINE_BEFORE_PROPERTY_NAME = "LineBefore";

        private string _lineBefore = string.Empty;

        /// <summary>
        /// Sets and gets the LineBefore property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LineBefore
        {
            get
            {
                return _lineBefore;
            }

            set
            {
                if (_lineBefore == value)
                {
                    return;
                }

                RaisePropertyChanging(LINE_BEFORE_PROPERTY_NAME);
                _lineBefore = value;
                RaisePropertyChanged(LINE_BEFORE_PROPERTY_NAME);
            }
        }
        #endregion
        
       
    }
   

}
