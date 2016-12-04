using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GetWelds.Properties;
using Microsoft.Win32;

namespace GetWelds.ViewModels
{
    public class Global
    {
    }

    public class GetWeldViewModel : ViewModelBase
    {
        public void ParseFiles(string[] files)
        {
            foreach (var file in files)
            {
                ParseRobotFile(file);
            }
        }

        public void ParseZipFile(FileInfo file)
        {
        }

        public void ParseDirectory(DirectoryInfo directory)
        {
        }

        private string GetRandomMotion
        {
            get
            {
                var rnd = new Random();
                return rnd.Next(-580, 580).ToString(CultureInfo.InvariantCulture);
            }
        }

        private string GetRandom
        {
            get
            {
                var rnd = new Random();
                return rnd.Next(-180, 180).ToString(CultureInfo.InvariantCulture);
            }
        }

        private string GetThickness
        {
            get
            {
                var rnd = new Random();
                return rnd.Next().ToString(CultureInfo.InvariantCulture);
            }
        }

        private string GetWeldForce
        {
            get
            {
                var rnd = new Random();
                return rnd.Next(400, 600).ToString(CultureInfo.InvariantCulture);
            }
        }

        private string GetWeldSchedule
        {
            get
            {
                var rnd = new Random();
                return rnd.Next(1, 30).ToString(CultureInfo.InvariantCulture);
            }
        }



        #region Constructor

        public GetWeldViewModel()
        {
            Positions = new List<Position>();
            if (IsInDesignMode)
            {
                for (var i = 0; i < 20; i++)
                {
                    var p = new Weld { Sequence = i, Schedule = GetWeldSchedule, Thickness = GetThickness, Force = GetWeldForce, Id = GetWeldForce, Name =
                        $"Position{i}", A = GetRandom, B = GetRandom, C = GetRandom, X = GetRandomMotion, Y = GetRandomMotion, Z = GetRandomMotion, E1 = "20" };

                    Positions.Add(p);
                }// Code runs in Blend --> create design time data.
            }
        }

        public GetWeldViewModel(string[] files)
        {
            Positions = new List<Position>();
            ParseFiles(files);
        }

        #endregion Constructor

        #region Properties

        #endregion Properties

        #region Members

        #endregion Members

        #region Properties

        private FileInfo _fileName;

        /// <summary>
        /// Sets and gets the FileName property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public FileInfo FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value; RaisePropertyChanged("FileName");
            }
        }

        private bool _showOnlyWelds;

        public bool ShowOnlyWelds { get { return _showOnlyWelds; } set { _showOnlyWelds = value; RaisePropertyChanged("ShowOnlyWelds"); } }

        #region Positions

        /// <summary>
        /// The <see cref="Positions" /> property's name.
        /// </summary>
        public const string POSITIONS_PROPERTY_NAME = "Positions";

        private List<Position> _positions;

        /// <summary>
        /// Sets and gets the Positions property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public List<Position> Positions
        {
            get
            {
                return _positions ?? (_positions = new List<Position>());
            }

            set
            {
                if (_positions == value)
                {
                    return;
                }

                RaisePropertyChanging(POSITIONS_PROPERTY_NAME);
                _positions = value;
                RaisePropertyChanged(POSITIONS_PROPERTY_NAME);
            }
        }

        #endregion Positions

        private ICommand _exportToXml;

        public ICommand ExportToXmlCommand { get { return _exportToXml ?? (_exportToXml = new RelayCommand(ExportXml, CanExportCsv)); } }

        #endregion Properties

        private void ExportXml()
        {
            var sfd = new SaveFileDialog { Filter = "XML File (*.xml)|*.xml" };
            var result = sfd.ShowDialog();
            if (result != true) return;
            ExportToXmlFile(sfd.FileName);
        }

        public void ExportToXmlFile(string filename)
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<Weld>));
            var writer = new StreamWriter(filename);
            serializer.Serialize(writer, Positions);

            writer.Close();
        }

        public ICommand ExportToXml { get; set; }


        protected bool CanExportCsv()
        {
            return true;
        }

        #region FindWeldsCommand

        private ICommand _findWeldsCommand;

        public ICommand FindWeldsCommand { get { return _findWeldsCommand ?? (_findWeldsCommand = new RelayCommand(FindWelds, CanFindWelds)); } }

        private void FindWelds()
        {
            Positions.Clear();
            var ofd = new OpenFileDialog
            {
                Filter = "KUKA Source Files *.src(*.src)|*.src",
                Title = "Select Robot Program"
            };

            var result = ofd.ShowDialog();

            if (result == false) return;
            var fn = Path.GetFileName(ofd.FileName);
            if (fn != null) FileName = new FileInfo(fn);
            ParseRobotFile(ofd.FileName);
        }

        public void ParseRobotFile(string filename, int style = -1)
        {
            FileName = new FileInfo(filename);

            Positions.Clear();
            var fileLines = File.ReadAllLines(filename);

            for (var i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(";FOLD SG.P_") | fileLines[i].Contains(";FOLD S.PLIN "))
                    Positions.Add(new Weld(fileLines[i], i, Positions.Count + 1, filename, style));
                else
                {
                    if (fileLines[i].ToUpper().Contains(";FOLD PTP"))
                        ParsePosition(fileLines[i], i);
                    if (fileLines[i].ToUpper().Contains(";FOLD LIN"))
                        ParsePosition(fileLines[i], i);
                }
            }
        }

        public Position ParsePosition(string line, int linenumber)
        {
            var p = new Position {Filename = FileName.Name};

            line = line.Replace(";FOLD", string.Empty);
            line = line.Substring(0, line.IndexOf(";", StringComparison.Ordinal)).Trim();
            var spl = line.Trim().Split(' ');
            p.MotionType = (PositionType)Enum.Parse(typeof(PositionType), spl[0]);
            p.Name = spl[1];
            p.Velocity = Convert.ToDouble(spl[4]);
            p.IsContinuous = string.Equals(spl[2], "CONT", StringComparison.OrdinalIgnoreCase);
            p.LineNumber = linenumber;
            return p;
        }

        public Weld ParseRobotLine(string line, int linenumber)
        {
            //TODO Need to be able to determine if Weld is Spot or servo
            var w = new Weld
            {
                Name = GetRegexMatch(Settings.Default.KukaWeldNameRegex, line),
                Sequence = Positions.Count + 1,
                Velocity = Convert.ToDouble(GetRegexMatch(Settings.Default.VelocityRegex, line)),
                LineNumber = linenumber,
                Schedule = GetRegexMatch(Settings.Default.WeldScheduleRegex, line),
                Id = GetRegexMatch(Settings.Default.WeldIdRegex, line),
                Thickness = GetRegexMatch(Settings.Default.WeldThicknessRegex, line),
                Force = GetRegexMatch(Settings.Default.WeldForceRegex, line),
                Gun1 = GetRegexMatch(Settings.Default.Gun1Regex, line),
                Gun2 = GetRegexMatch(Settings.Default.Gun2Regex, line),
                Gun3 = GetRegexMatch(Settings.Default.Gun3Regex, line),
                Gun4 = GetRegexMatch(Settings.Default.Gun4Regex, line),
                Equalizer1 = GetRegexMatch(Settings.Default.Eqlzr1Regex, line).Trim() == "X",
                Equalizer2 = GetRegexMatch(Settings.Default.Eqlzr2Regex, line).Trim() == "X",
                Equalizer3 = GetRegexMatch(Settings.Default.Eqlzr3Regex, line).Trim() == "X",
                Equalizer4 = GetRegexMatch(Settings.Default.Eqlzr4Regex, line).Trim() == "X"
            };

            GetRegexMatch(Settings.Default.AnticpRegex, line);

            return w;
        }

        public static string GetRegexMatch(string regexString, string line)
        {
            var regex = new Regex(regexString);
            return GetRegexMatch(regex, line);
        }

        private static string GetRegexMatch(Regex regex, string line)
        {
            var match = regex.Match(line);
            while (match.Success)
            {
                return match.Groups[1].ToString();
            }
            return string.Empty;
        }

        private double GetVelocity(string line)
        {
            var rgx = new Regex("Vel=([0-9.-]*)");
            var match = rgx.Match(line);
            while (match.Success)
            {
                return Convert.ToDouble(match.Groups[1].ToString());
            }

            return -1;
        }

        private bool CanFindWelds()
        {
            return true;
        }

        #endregion FindWeldsCommand
    }
}