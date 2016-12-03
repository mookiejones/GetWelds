using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.IO;
using Path = System.IO.Path;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Text.RegularExpressions;

namespace GetWelds
{
    public class Global
    {
    }
    
    public class GetWeldViewModel:ViewModelBase
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

        // Ped Welder Line
        //FOLD S.PLIN Slwp5251_b67o, Tool=1, Base=2, IPOType=Base, Vel=1m/s, Acc=100%, Gun1=1, Open_BU1=[X ], Eqlzr1=[X ], Gun2=0, Open_BU2=[ ], Eqlzr2=[ ], Gun3=0, Open_BU3=[ ], Eqlzr3=[ ], Gun4=0, Open_BU4=[ ], Eqlzr4=[ ], WeldId=67, Anticip=[ ];%{PE}%MKUKATPUSER
        
     //  #region · Weld Parsing Regex ·
     //  Regex WeldIdRegex = new Regex("WeldId=([0-9]+)");
     //  Regex VelocityRegex = new Regex("Vel=([0-9.-]*)");
     //  Regex WeldNameRegex = new Regex(";FOLD SG?.P_?[PLTIN]+ [SG_]{1,3}([^, ]+),([^\r]*)");
     //  Regex WeldThicknessRegex = new Regex("Part=([0-9.]+) ");
     //  Regex WeldScheduleRegex = new Regex("WeldSchd=([0-9]*)");
     //  Regex WeldForceRegex = new Regex("Force=([0-9.]*) ");
     //  Regex OpenBackup1Regex = new Regex("Open_BU1=([^/]]*)/]");
     //  Regex OpenBackup2Regex = new Regex("Open_BU2=([^/]]*)/]");
     //  Regex OpenBackup3Regex = new Regex("Open_BU3=([^/]]*)/]");
     //  Regex OpenBackup4Regex = new Regex("Open_BU4=([^/]]*)/]");
     //  Regex Gun1Regex=new Regex("Gun1=([0-9]+)");
     //  Regex Gun2Regex=new Regex("Gun2=([0-9]+)");
     //  Regex Gun3Regex=new Regex("Gun3=([0-9]+)");
     //  Regex Gun4Regex=new Regex("Gun4=([0-9]+)");
     //  Regex Eqlzr1Regex = new Regex("Eqlzr1=([^/]]*)/]");
     //  Regex Eqlzr2Regex = new Regex("Eqlzr2=([^/]]*)/]");
     //  Regex Eqlzr3Regex = new Regex("Eqlzr3=([^/]]*)/]");
     //  Regex Eqlzr4Regex = new Regex("Eqlzr4=([^/]]*)/]");
     //  Regex AnticpRegex = new Regex("Anticip=([^/]]*)/]");
     //  #endregion                           
                                                                 
        string GetRandomMotion
        {
             get{
                var rnd = new Random();
                return rnd.Next(-580,580).ToString();
            }
        }
        string GetRandom
        {
            get{
                var rnd = new Random();
                return rnd.Next(-180,180).ToString();
            }
        }
        string GetThickness
        {
            get
            {
                var rnd = new Random();
                return rnd.Next().ToString();
            }
        }
        string GetWeldForce
        {
            get
            {
                var rnd = new Random();
                return rnd.Next(400, 600).ToString();
            }
        }
        string GetWeldSchedule
        {
            get
            {
                var rnd = new Random();
                return rnd.Next(1, 30).ToString();
            }
        }
        #region Constructor
        public GetWeldViewModel()
        {

            Positions = new List<Position>();
            if (IsInDesignMode)
            {
                Random rnd1 = new Random();
                for (var i = 0; i < 20; i++)
                {
                    var p = new Weld { Sequence=i, Schedule=GetWeldSchedule, Thickness=GetThickness,Force=GetWeldForce,ID=GetWeldForce, Name=String.Format("Position{0}",i),  A = GetRandom, B = GetRandom, C = GetRandom, X = GetRandomMotion, Y = GetRandomMotion, Z = GetRandomMotion, E1 = "20" };

                    Positions.Add(p);
                }// Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
            }
           
        }
        public GetWeldViewModel(string[] files)
        {
            Positions = new List<Position>();
                ParseFiles(files);
        }
        #endregion
        
        #region Properties

        #endregion

        #region Members
        #endregion

        #region Properties

    
        private FileInfo _fileName = null;

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
                _fileName = value;RaisePropertyChanged("FileName") ;
            }
        }


        private bool _showOnlyWelds = true;
        public bool ShowOnlyWelds{get { return _showOnlyWelds; }set { _showOnlyWelds = value;RaisePropertyChanged("ShowOnlyWelds"); }}       
        
      

        

        #region Positions
        /// <summary>
        /// The <see cref="Positions" /> property's name.
        /// </summary>
        public const string PositionsPropertyName = "Positions";

        private List<Position> _positions = new List<Position>();

        /// <summary>
        /// Sets and gets the Positions property.
        /// Changes to that property's value raise the PropertyChanged event. 
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

                RaisePropertyChanging(PositionsPropertyName);
                _positions = value;
                RaisePropertyChanged(PositionsPropertyName);
            }
        }
        #endregion
        
        private ICommand _exportToXML;
        public ICommand ExportToXMLCommand { get { return _exportToXML ?? (_exportToXML = new RelayCommand(ExportXML, CanExportCSV)); } }
        #endregion

        void ExportXML()
        {

            var sfd = new SaveFileDialog {Filter = "XML File (*.xml)|*.xml"};
            var result = sfd.ShowDialog();
            if (result != true) return;
            ExportToXMLFile(sfd.FileName);
        }

        public void ExportToXMLFile(string filename)
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<Weld>));
            var writer = new StreamWriter(filename);
            serializer.Serialize(writer, Positions);

            writer.Close();
        }


      
        public ICommand ExportToXML
        {
            get { return _exportToXML; }
            set { _exportToXML = value; }
        }

        protected bool CanExportCSV()
        {
            return true;
        }
      
        #region FindWeldsCommand

        private ICommand _findWeldsCommand;
        public ICommand FindWeldsCommand{get{return _findWeldsCommand??(_findWeldsCommand = new RelayCommand(FindWelds,CanFindWelds));}}

        void FindWelds()
        {
            Positions.Clear();
            var ofd = new OpenFileDialog
            {
                Filter = "KUKA Source Files *.src(*.src)|*.src",
                Title = "Select Robot Program"
            };

            var result = ofd.ShowDialog();


            if (result==false) return;
                FileName = new FileInfo(Path.GetFileName(ofd.FileName));
                ParseRobotFile(ofd.FileName);
        }


        public void ParseRobotFile(string filename,int style =-1)
        {

            FileName = new FileInfo(filename);

            Positions.Clear();
            var fileLines = File.ReadAllLines(filename);


           
            for (var i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(";FOLD SG.P_") | fileLines[i].Contains(";FOLD S.PLIN "))
                    Positions.Add(new Weld(fileLines[i], i, Positions.Count + 1,filename,style));
                else
                {
                    if (fileLines[i].ToUpper().Contains(";FOLD PTP"))
                        ParsePosition(fileLines[i], i);
                    if (fileLines[i].ToUpper().Contains(";FOLD LIN"))
                        ParsePosition(fileLines[i], i);

                }

            }
        }


        public  Position ParsePosition(string line, int linenumber)
        {
            var p = new Position();
            p.Filename = FileName.Name;
            
            line = line.Replace(";FOLD", String.Empty);
            line = line.Substring(0, line.IndexOf(";", StringComparison.Ordinal)).Trim();
                var spl = line.Trim().Split(' ');
                p.MotionType = (PositionType)Enum.Parse(typeof (PositionType), spl[0]);
                p.Name = spl[1];
                p.Velocity = Convert.ToDouble(spl[4]);
                p.IsContinuous = String.Equals(spl[2], "CONT",StringComparison.OrdinalIgnoreCase);
                p.LineNumber = linenumber;
            return p;
        }


        public  Weld ParseRobotLine(string line,int linenumber)
        {

            //TODO Need to be able to determine if Weld is Spot or servo
           var w = new Weld();
           w.Name = GetRegexMatch(Properties.Settings.Default.KukaWeldNameRegex, line);
               
                w.Sequence = Positions.Count+1;



                w.Velocity = Convert.ToDouble(GetRegexMatch(Properties.Settings.Default.VelocityRegex, line));
                w.LineNumber = linenumber;            
                w.Schedule = GetRegexMatch(Properties.Settings.Default.WeldScheduleRegex, line);
                w.ID = GetRegexMatch(Properties.Settings.Default.WeldIdRegex, line);
                w.Thickness = GetRegexMatch(Properties.Settings.Default.WeldThicknessRegex, line);
                w.Force = GetRegexMatch(Properties.Settings.Default.WeldForceRegex, line);
           
            w.Gun1 = GetRegexMatch(Properties.Settings.Default.Gun1Regex, line);
            w.Gun2 = GetRegexMatch(Properties.Settings.Default.Gun2Regex, line);
            w.Gun3 = GetRegexMatch(Properties.Settings.Default.Gun3Regex, line);
            w.Gun4 = GetRegexMatch(Properties.Settings.Default.Gun4Regex, line);

            w.Equalizer1 = GetRegexMatch(Properties.Settings.Default.Eqlzr1Regex, line).Trim()=="X";
            w.Equalizer2 = GetRegexMatch(Properties.Settings.Default.Eqlzr2Regex, line).Trim() == "X";
            w.Equalizer3 = GetRegexMatch(Properties.Settings.Default.Eqlzr3Regex, line).Trim() == "X";
            w.Equalizer4 = GetRegexMatch(Properties.Settings.Default.Eqlzr4Regex, line).Trim() == "X";

            var antic = GetRegexMatch(Properties.Settings.Default.AnticpRegex, line);
            
            return w;
        }
        public static string GetRegexMatch(string RegexString, string line)
        {
            var regex = new Regex(RegexString);
            return GetRegexMatch(regex, line);
        }
        static string GetRegexMatch(Regex regex, string line)
        {
            Match match = regex.Match(line);
                while(match.Success)
                {
                    return match.Groups[1].ToString();
                }
                return string.Empty;
        }
        double GetVelocity(string line)
        {
            var rgx = new Regex("Vel=([0-9.-]*)");
            Match match = rgx.Match(line);
            while (match.Success)
            {
                return Convert.ToDouble(match.Groups[1].ToString());
      
            }

            return -1;
           
        }
        bool CanFindWelds()
        {
            return true;
        }
        #endregion

    }

 
   
  
  
}
