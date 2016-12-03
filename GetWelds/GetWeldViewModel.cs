using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;
using KUKA.Core.ViewModels;
using Microsoft.Win32;
using System.IO;
using Path = System.IO.Path;

namespace GetWelds
{
     
 
    public class GetWeldViewModel:ViewModelBase
    {
        #region Constructor

        #endregion
        
        #region Properties

        #endregion

        #region Members
        #endregion

        #region Properties

        private bool _showOnlyWelds = true;
        public bool ShowOnlyWelds{get { return _showOnlyWelds; }set { _showOnlyWelds = value;RaisePropertyChanged("ShowOnlyWelds"); }}
        private string _fileName;
        public string FileName{get { return _fileName; }set { _fileName = value;RaisePropertyChanged("FileName"); }}


        private List<Position> _positions = new List<Position>();
        public List<Position> Positions { get { return _positions; } set { _positions = value; RaisePropertyChanged("Positions"); } }

        private ICommand _exportToXML;
        public ICommand ExportToXMLCommand { get { return _exportToXML ?? (_exportToXML = new RelayCommand(ExportXML, CanExportCSV)); } }
        #endregion

        void ExportXML(object param)
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

        protected bool CanExportCSV(object param)
        {
            return true;
        }
      
        #region FindWeldsCommand

        private ICommand _findWeldsCommand;
        public ICommand FindWeldsCommand{get{return _findWeldsCommand??(_findWeldsCommand = new RelayCommand(FindWelds,CanFindWelds));}}

        void FindWelds(object param)
        {
            Positions.Clear();
            var ofd = new OpenFileDialog
            {
                Filter = "KUKA Source Files *.src(*.src)|*.src",
                Title = "Select Robot Program"
            };

            var result = ofd.ShowDialog();

            if (result == true)
            {
                FileName = Path.GetFileName(ofd.FileName);
                ParseRobotFile(ofd.FileName);
            }
        }

        public void ParseRobotFile(string filename)
        {
            Positions.Clear();
            var fileLines = File.ReadAllLines(filename);

            for (var i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(";FOLD SG.P_"))
                    Positions.Add(ParseRobotLine(fileLines[i],i));
                else
                {
                    if (fileLines[i].ToUpper().Contains(";FOLD PTP"))
                        ParsePosition(fileLines[i], i);
                    if (fileLines[i].ToUpper().Contains(";FOLD LIN"))
                        ParsePosition(fileLines[i], i);

                }
                


            }




        }

        Position ParsePosition(string line, int linenumber)
        {
            var p = new Position();
            
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


        Weld ParseRobotLine(string line,int linenumber)
        {
           var w = new Weld();
                var spl = line.Split(',');
            w.Sequence = Positions.Count+1;
                w.Name = spl[0].Contains(";FOLD SG.P_LIN") ? w.Name = spl[0].Replace(";FOLD SG.P_LIN", String.Empty).Trim() : w.Name = spl[0].Replace(";FOLD SG.P_PTP", String.Empty).Trim();
                w.Name = w.Name.Replace("SGlwp", String.Empty);
                var velocity = spl[1].Replace("Vel=", String.Empty);
                velocity = velocity.Contains("m/s") ? velocity.Replace("m/s", String.Empty) : velocity.Replace("%", String.Empty);
                w.LineNumber = linenumber;
                w.Velocity = Convert.ToDouble(velocity.Trim());
                w.Schedule= spl[6].Replace("WeldSchd=", String.Empty).Trim();
                w.ID= spl[7].Replace("WeldId=", String.Empty).Trim();
                w.Thickness = spl[8].Replace("Part=", String.Empty).Trim();
                w.Thickness = w.Thickness.Replace("mm", String.Empty).Trim();
                w.Force = spl[9].Replace("Force=", String.Empty).Trim();
                w.Force = w.Force.Replace("lbs", String.Empty).Trim();
            
            return w;
        }

        bool CanFindWelds(object param)
        {
            return true;
        }
        #endregion

    }

    public class Weld : Position
    {

       

        private string _force;
        public string Force{get { return _force; }set { _force = value;RaisePropertyChanged("Force"); }}

        private string _thickness;
        public string Thickness{get { return _thickness; }set { _thickness = value;RaisePropertyChanged("Thickness"); }}

        private int _sequence;
        public int Sequence{get { return _sequence; }set { _sequence = value;RaisePropertyChanged("Sequence"); }}
      

        private string _id;
        public string ID { get { return _id; } set { _id = value; RaisePropertyChanged("ID"); } }

        private string _schedule;
        public string Schedule{get { return _schedule; }set { _schedule = value;RaisePropertyChanged("Schedule"); }}
        private Position _position = new Position();
        public Position WeldPosition { get { return _position; } set { _position = value; RaisePropertyChanged("WeldPosition"); } }
    }

   
    public class Position : ViewModelBase
    {

        private bool _isContinuous;
        public bool IsContinuous { get { return _isContinuous; } set { _isContinuous = value; RaisePropertyChanged("IsContinuous"); } }

        private double _velocity;
        public double Velocity { get { return _velocity; } set { _velocity = value; RaisePropertyChanged("Velocity"); } }
        private string _name;
        public string Name{get { return _name; }set { _name = value;RaisePropertyChanged("PositionName"); }}
        private PositionType _motionType = PositionType.None;
        public PositionType MotionType{get { return _motionType; }set { _motionType = value;RaisePropertyChanged("MotionType"); }}

        private int _lineNumber;
        public int LineNumber { get { return _lineNumber; } set { _lineNumber = value; RaisePropertyChanged("LineNumber"); } }

        private string _x;
        public string X{get { return _x; }set { _x = value;RaisePropertyChanged("X"); }}
        private string _y;
        public string Y { get { return _y; } set { _y = value; RaisePropertyChanged("Y"); } }
        private string _z;
        public string Z { get { return _z; } set { _z = value; RaisePropertyChanged("Z"); } }
        private string _a;
        public string A { get { return _a; } set { _a = value; RaisePropertyChanged("A"); } }
        private string _b;
        public string  B{ get { return _b; } set { _b = value; RaisePropertyChanged("B"); } }
        private string _c;
        public string C { get { return _c; } set { _c = value; RaisePropertyChanged("C"); } }
        private string _s;
        public string S { get { return _s; } set { _s = value; RaisePropertyChanged("S"); } }
        private string _t;
        public string T { get { return _t; } set { _t = value; RaisePropertyChanged("T"); } }


        private string _e1;
        public string E1 { get { return _e1; } set { _e1 = value; RaisePropertyChanged("E1"); } }
        private string _e2;
        public string E2 { get { return _e2; } set { _e2 = value; RaisePropertyChanged("E2"); } }
       

    }

    public enum PositionType { PTP, LIN, CIRC,None }

  
}
