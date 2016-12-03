using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using GetWelds.ViewModels;
using GetWelds.Properties;
using Ionic.Zip;
using GetWelds.Converters;
using GetWelds.Helpers;

namespace GetWelds.Robots
{
    [XmlInclude(typeof(Kuka))]
    [XmlInclude(typeof(Fanuc))]
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

            Console.WriteLine("Processing Files for {0}", this.Name);
            ProcessRobotFiles();

       
        }


        public void GetSystems(RobotZipFile zip)
        {
            Zip = zip;
            GetMainPrograms(MainRegexString);
            GetConfigData();
            GetProcessData();
            Console.WriteLine("Processing Files for {0}", this.Name);
            ProcessRobotFiles();

        }

        public abstract void ParseZipFiles(IEnumerable<FileInfo> zips);

        #region · Properties ·


        protected abstract string MainRegexString { get; }

        #region · PositionCollection ·

        /// <summary>
        /// The <see cref="PositionCollection" /> property's name.
        /// </summary>
        public const string PositionCollectionPropertyName = "PositionCollection";


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
                var positions = from position in Positions where position is AbstractWeld select (AbstractWeld)position;
                return positions.ToList();
            }
        }

        #endregion


        #region Positions

        /// <summary>
        /// The <see cref="Positions" /> property's name.
        /// </summary>
        public const string PositionsPropertyName = "Positions";

        private ObservableCollection<AbstractWeld> _positions = new ObservableCollection<AbstractWeld>();

        /// <summary>
        /// Sets and gets the Positions property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ObservableCollection<AbstractWeld> Positions
        {
            get
            {
                return _positions;
            }

            set
            {
                if (_positions == value)
                {
                    return;
                }

                _positions = value;
                RaisePropertyChanged(PositionsPropertyName);
                RaisePropertyChanged("PositionCollection");
            }
        }

        #endregion Positions


        #region Files
        /// <summary>
        /// The <see cref="Files" /> property's name.
        /// </summary>
        public const string FilesPropertyName = "Files";

        private List<RobotFile> _filesInfos = null;

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

                RaisePropertyChanging(FilesPropertyName);
                _filesInfos = value;
                RaisePropertyChanged(FilesPropertyName);
            }
        }
        #endregion



        public ObservableCollection<Tool> Tools { get; set; }

        #region Zip

        /// <summary>
        /// The <see cref="Zip" /> property's name.
        /// </summary>
        public const string ZipPropertyName = "Zip";

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
                Set(ZipPropertyName, ref _zip, value);
            }
        }

        #endregion Zip

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

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
        public const string Process2PropertyName = "Process2";

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

                RaisePropertyChanging(Process2PropertyName);
                _process2 = value;
                RaisePropertyChanged(Process2PropertyName);
            }
        }

        #endregion Process2

        #region Process1

        /// <summary>
        /// The <see cref="Process1" /> property's name.
        /// </summary>
        public const string Process1PropertyName = "Process1";

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

                RaisePropertyChanging(Process1PropertyName);
                _process1 = value;
                RaisePropertyChanged(Process1PropertyName);
            }
        }

        #endregion Process1




        

        #region OptionsValues
        /// <summary>
        /// The <see cref="OptionalValues" /> property's name.
        /// </summary>
        public const string OptionsValuesPropertyName = "OptionsValues";

        private ObservableCollection<SearchParam> Options = new ObservableCollection<SearchParam>();

        /// <summary>
        /// Sets and gets the OptionsValues property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// <remarks> Used with a configuration file so that additional values can be retrieved from config.Dat</remarks>
        /// </summary>
        public ObservableCollection<SearchParam> OptionalValues
        {
            get
            {
                return Options;
            }

            set
            {
                if (Options == value)
                {
                    return;
                }

                RaisePropertyChanging(OptionsValuesPropertyName);
                Options = value;
                RaisePropertyChanged(OptionsValuesPropertyName);
            }
        }
        #endregion
        

        #endregion · Properties ·

        #region · Fields ·

        [Annotations.NotNull]
        private readonly List<int> _ignoredStyles = new List<int> { 41, 40, 42, 43, 44, 45, 46, 47, 48, 49, 50, 57, 58, 59, 60, 61, 62, 63, 64, 65 };

        internal IEnumerable<RobotFile> _processableFiles;


        #endregion · Fields ·

        #region · SelectedStyle ·

        /// <summary>
        /// The <see cref="SelectedStyle" /> property's name.
        /// </summary>
        public const string SelectedStylePropertyName = "SelectedStyle";

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
                Set(SelectedStylePropertyName, ref _selectedStyle, value, true);
            }
        }

        #endregion · SelectedStyle ·        

        #region Styles
        /// <summary>
        /// The <see cref="Styles" /> property's name.
        /// </summary>
        public const string StylesPropertyName = "Styles";

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

                RaisePropertyChanging(StylesPropertyName);
                _styles = value;
                RaisePropertyChanged(StylesPropertyName);
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


        internal string _tempPath = string.Empty;

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
            _processableFiles = _files;
        }

        /// <summary>
        /// The <see cref="Zones" /> property's name.
        /// </summary>
        public const string ZonesPropertyName = "Zones";

        private ObservableCollection<Zone> _zones = new ObservableCollection<Zone>();

        /// <summary>
        /// Sets and gets the Zones property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Zone> Zones
        {
            get
            {
                return _zones;
            }

            set
            {
                if (_zones == value)
                {
                    return;
                }

                RaisePropertyChanging(ZonesPropertyName);
                _zones = value;
                RaisePropertyChanged(ZonesPropertyName);
            }
        }
        public abstract Regex PTPWeldRegex { get; }
        public abstract Regex LINWeldRegex { get; }
        public abstract Regex LINStudWeldRegex { get; }
        public abstract Regex PTPStudWeldRegex { get; }


        public abstract Regex RivetWeldRegex { get; }

        public abstract string FileExtension { get; }


        /// <summary>
        /// The <see cref="SelectedZone" /> property's name.
        /// </summary>
        public const string SelectedZonePropertyName = "SelectedZone";

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

                RaisePropertyChanging(SelectedZonePropertyName);
                _selectedZone = value;
                RaisePropertyChanged(SelectedZonePropertyName);
            }
        }


        #region RobotOptions
        /// <summary>
        /// The <see cref="RobotOptions" /> property's name.
        /// </summary>
        public const string RobotOptionsPropertyName = "RobotOptions";

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

                RaisePropertyChanging(RobotOptionsPropertyName);
                _robotOptions = value;
                RaisePropertyChanged(RobotOptionsPropertyName);
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
        public const string DeclarationPropertyName = "Declaration";

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

                RaisePropertyChanging(DeclarationPropertyName);
                _declaration = value;
                RaisePropertyChanged(DeclarationPropertyName);
            }
        }
        #endregion

        

        #region ZoneFilePosition
        /// <summary>
        /// The <see cref="ZoneFilePosition" /> property's name.
        /// </summary>
        public const string ZoneFilePositionPropertyName = "ZoneFilePosition";

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

                RaisePropertyChanging(ZoneFilePositionPropertyName);
                _zoneFilePosition = value;
                RaisePropertyChanged(ZoneFilePositionPropertyName);
            }
        }
        #endregion
        

        #region EntryFilename
        /// <summary>
        /// The <see cref="EntryFilename" /> property's name.
        /// </summary>
        public const string EntryFilenamePropertyName = "EntryFilename";

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

                RaisePropertyChanging(EntryFilenamePropertyName);
                _entryFilename = value;
                RaisePropertyChanged(EntryFilenamePropertyName);
            }
        }
        #endregion
        

        #region LineBefore
        /// <summary>
        /// The <see cref="LineBefore" /> property's name.
        /// </summary>
        public const string LineBeforePropertyName = "LineBefore";

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

                RaisePropertyChanging(LineBeforePropertyName);
                _lineBefore = value;
                RaisePropertyChanged(LineBeforePropertyName);
            }
        }
        #endregion
        
       
    }
   

}
