using System;
using System.IO;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GetWelds.Properties;
using GetWelds.Robots;
using Ionic.Zip;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;

namespace GetWelds.ViewModels
{
    public class StyleProgramViewModel : ViewModelBase
    {
        private AbstractRobot _robot;
         
        #region · Properties ·

        /// <summary>
        /// The <see cref="StyleProgramName" /> property's name.
        /// </summary>
        public const string StyleProgramNamePropertyName = "StyleProgramName";

        private string _styleProgramName = string.Empty;

        /// <summary>
        /// Sets and gets the StyleProgramName property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string StyleProgramName
        {
            get
            {
                return _styleProgramName;
            }

            set
            {
                if (_styleProgramName == value)
                {
                    return;
                }

                RaisePropertyChanging(StyleProgramNamePropertyName);
                var oldValue = _styleProgramName;
                _styleProgramName = value;
                RaisePropertyChanged(StyleProgramNamePropertyName, oldValue, value, true);
            }
        }

        #endregion · Properties ·

        #region · Text ·

        /// <summary>
        /// The <see cref="Text" /> property's name.
        /// </summary>
        public const string TextPropertyName = "Text";

        private string _text = string.Empty;

        /// <summary>
        /// Sets and gets the Text property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                Set(TextPropertyName, ref _text, value, true);
            }
        }

        #endregion · Text ·

        #region · ZipFile ·

        /// <summary>
        /// The <see cref="ZipFile" /> property's name.
        /// </summary>
        public const string ZipFilePropertyName = "ZipFile";

        [XmlIgnore]
        private ZipFile _zipFile = null;

        /// <summary>
        /// Sets and gets the ZipFile property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        [XmlIgnore]
        public ZipFile ZipFile
        {
            get
            {
                return _zipFile;
            }
            set
            {
                Set(ZipFilePropertyName, ref _zipFile, value, true);
            }
        }

        #region · RobotName ·

        /// <summary>
        /// The <see cref="RobotName" /> property's name.
        /// </summary>
        public const string RobotNamePropertyName = "RobotName";

        private string _robotName = string.Empty;

        /// <summary>
        /// Sets and gets the RobotName property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string RobotName
        {
            get
            {
                return _robotName;
            }
            set
            {
                Set(RobotNamePropertyName, ref _robotName, value, true);
            }
        }

        #endregion · RobotName ·

        #region · Style ·

        /// <summary>
        /// The <see cref="Style" /> property's name.
        /// </summary>
        public const string StylePropertyName = "Style";

        private int _style = -1;

        /// <summary>
        /// Sets and gets the Style property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public int Style
        {
            get
            {
                return _style;
            }
            set
            {
                Set(StylePropertyName, ref _style, value, true);
            }
        }

        #endregion · Style ·

        #endregion · ZipFile ·


        #region FilterWeldsCommand

        private RelayCommand _filterWeldsCommand;
        /// <summary>
        /// Gets the FilterWeldsCommand.
        /// </summary>
        public RelayCommand FilterWeldsCommand
        {
            get
            {

                return _filterWeldsCommand
                    ?? (_filterWeldsCommand = new RelayCommand(ExecuteFilterWeldsCommand));
            }
        }

        bool filtering = false;
        private void ExecuteFilterWeldsCommand()
        {
            PositionsCollection = CollectionViewSource.GetDefaultView(Positions);

            ListCollectionView list = (ListCollectionView) PositionsCollection;

            filtering = ! filtering;
            if (filtering)
            {
                list.Filter = new Predicate<object>(x => ((AbstractWeld) x).Sequence != -1);
            }
            else
                list.Filter = new Predicate<object>(x => x == x);


        }
        #endregion

        #region · Collections ·

        

        #region Programs
        /// <summary>
        /// The <see cref="Programs" /> property's name.
        /// </summary>
        public const string ProgramsPropertyName = "Programs";

        private List<string> _programs = new List<string>();

        /// <summary>
        /// Sets and gets the Programs property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<string> Programs
        {
            get
            {
                return _programs;
            }

            set
            {
                if (_programs == value)
                {
                    return;
                }

                RaisePropertyChanging(ProgramsPropertyName);
                _programs = value;
                RaisePropertyChanged(ProgramsPropertyName);
            }
        }
        #endregion


        public ICollectionView PositionsCollection { get; set; }

        #region · Positions ·

        /// <summary>
        /// The <see cref="Positions" /> property's name.
        /// </summary>
        public const string PositionsPropertyName = "Positions";

        private List<Position> _positions = new List<Position>();

        /// <summary>
        /// Sets and gets the Positions property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
      
        public List<Position> Positions
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

                RaisePropertyChanging(() => Positions);
                var oldValue = _positions;
                _positions = value;
                RaisePropertyChanged(() => Positions, oldValue, value, true);
            }
        }

        #endregion · Positions ·

        #endregion · Collections ·

        public static bool IsNumeric(string text)
        {
            try
            {
                var result = Convert.ToInt32(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private int GetStyle(string text)
        {
            var regex = new Regex(Settings.Default.FANUCSTYLEFROMPROGRAMREGEX);
            var match = regex.Match(text);
            var result = match.Groups[1].ToString();
            if (String.IsNullOrEmpty(result)) return -1;
            if (IsNumeric(result)&&(!String.IsNullOrEmpty(result)))
                return Convert.ToInt32(result);

            return -1;
        }

        public bool AddProgram(FileInfo program, AbstractRobot robot)
        {
          

            var name = Path.GetFileNameWithoutExtension(program.Name);
         //   Programs.Add(name);
            _robot = robot;
            //TODO Add Stud Programs
            //TODO Add Rivet Programs

            try
            {

                var servowelds = GetPositionMatches(_robot.LINWeldRegex, program);
                var studWelds = GetPositionMatches(_robot.LINStudWeldRegex, program);
                var rivets = GetPositionMatches(_robot.RivetWeldRegex, program);
                return servowelds||studWelds||rivets;

              

            }

            catch (Exception ex)
            {
                Console.WriteLine(StringResources.NotUsed + ex);
                return false;
            }
        }

        /// <summary>
        /// Reusable method to get positional data from files;
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="text"></param>
        private bool GetPositionMatches(Regex regex,FileInfo program)
        {
            if (!program.Exists)
                return false;


            // Had to change to adapt to command spanning multiple lines
            var text = File.ReadAllText(program.FullName);

            // Get Style Number
            var style = GetStyle(text);

            var name = Path.GetFileNameWithoutExtension(program.Name);
           
            var match = regex.Match(text);

            if (!match.Success)
                return false;
            else
               Programs.Add(name);


            var i = 0;
            while (match.Success)
            {
                i++;
                var line = match.ToString();
                var lineNumber = IsNumeric(match.Groups[1].ToString()) ? Convert.ToInt32(match.Groups[1].ToString()): -1;
                var sequence = Positions.Count + 1;
                var programName = program.Name;
                var weld = _robot.GetWeld(match, lineNumber, sequence, programName, style);
                Positions.Add(weld);

                match = match.NextMatch();

            }

            return true;

        }
     


        public StyleProgramViewModel(AbstractRobot robot)
        {
            _robot = robot;

            PositionsCollection = CollectionViewSource.GetDefaultView(Positions);
            PositionsCollection.Filter = OnFilterWelds;
        }

        bool OnFilterWelds(object item)
        {
            return true;
        }



        public StyleProgramViewModel(string stylename, string filepath)
        {
        }

        public StyleProgramViewModel()
        {
        }

        

        #region DirectoryName
        /// <summary>
        /// The <see cref="DirectoryName" /> property's name.
        /// </summary>
        public const string DirectoryNamePropertyName = "DirectoryName";

        private string _directoryName = string.Empty;

        /// <summary>
        /// Sets and gets the DirectoryName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DirectoryName
        {
            get
            {
                return _directoryName;
            }

            set
            {
                if (_directoryName == value)
                {
                    return;
                }

                RaisePropertyChanging(DirectoryNamePropertyName);
                _directoryName = value;
                RaisePropertyChanged(DirectoryNamePropertyName);
            }
        }
        #endregion
        

        #region FullName
        /// <summary>
        /// The <see cref="FullName" /> property's name.
        /// </summary>
        public const string FullNamePropertyName = "FullName";

        private string _fullName = string.Empty;

        /// <summary>
        /// Sets and gets the FullName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string FullName
        {
            get
            {
                return _fullName;
            }

            set
            {
                if (_fullName == value)
                {
                    return;
                }

                RaisePropertyChanging(FullNamePropertyName);
                _fullName = value;
                RaisePropertyChanged(FullNamePropertyName);
            }
        }
        #endregion
        

        #region Name
        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _name = string.Empty;

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name == value)
                {
                    return;
                }

                RaisePropertyChanging(NamePropertyName);
                _name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }
        #endregion
        
    }
}