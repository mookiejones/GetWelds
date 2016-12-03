using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace GetWelds.ViewModels
{
    public class KTPOWeldRobotProgram
    {
        public void ParseRobotFile(List<string> filename, int style)
        {

            foreach (var file in filename)
            {
                var fileLines = File.ReadAllLines(file);

                for (var i = 0; i < fileLines.Length; i++)
                {
                    if (fileLines[i].Contains(";FOLD SG.P_") | fileLines[i].Contains(";FOLD S.PLIN "))
                        Positions.Add(new Weld(fileLines[i], i, Positions.Count + 1, file, style));
                    else
                    {
                        if (fileLines[i].ToUpper().Contains(";FOLD PTP"))
                            ParsePosition(fileLines[i], i);
                        if (fileLines[i].ToUpper().Contains(";FOLD LIN"))
                            ParsePosition(fileLines[i], i);
                    }
                }
            }
        }

        public Position ParsePosition(string line, int linenumber)
        {
            var p = new Position();

            line = line.Replace(";FOLD", String.Empty);
            line = line.Substring(0, line.IndexOf(";", StringComparison.Ordinal)).Trim();
            var spl = line.Trim().Split(' ');
            p.MotionType = (PositionType)Enum.Parse(typeof(PositionType), spl[0]);
            p.Name = spl[1];
            p.Velocity = Convert.ToDouble(spl[4]);
            p.IsContinuous = String.Equals(spl[2], "CONT", StringComparison.OrdinalIgnoreCase);
            p.LineNumber = linenumber;
            return p;
        }

        public int Style { get; set; }

        public ObservableCollection<Position> Positions
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
            }
        }

        public void GetPositions()
        {
            throw new NotImplementedException();
        }

        public KTPOWeldRobotProgram()
        {
            Positions = new ObservableCollection<Position>();
        }
    }
}