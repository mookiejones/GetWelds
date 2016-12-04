using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GetWelds.ViewModels;
using GetWelds.Properties;
using Ionic.Zip;
using System.Xml.Serialization;
using System.Text;
using GetWelds.Helpers;

namespace GetWelds.Robots
{

    public class Kuka : AbstractRobot
    {
        public Kuka()
        {
        }

        public Kuka(DirectoryInfo dir) : base(dir) { }
        public Kuka(IEnumerable<FileInfo> files) : base(files) { }



        public override Type Weld
        {
            get { throw new NotImplementedException(); }
        }


        public override void ProcessRobotFiles()
        {

          //  foreach (var style in Styles)
          //  {
          //
          //      var regex = new Regex(Settings.Default.STYLEPROGRAMREGEX);
          //
          //
          //      var lines = File.ReadAllLines(style.FullName);
          //
          //
          //      foreach (
          //          var matches in lines.Select(line => regex.Match(line)).Where(matches => matches.Groups.Count > 1))
          //      {
          //          var f = matches.Groups[1].ToString().Replace(";", String.Empty).Trim();
          //
          //
          //          var file = new FileInfo(Path.Combine(style.DirectoryName, f + Settings.Default.KUKAEXTENSION));
          //          var result = style.AddProgram(file, this);
          //          if (result)
          //              style.Programs.Add(f);
          //      }
          //  }
          //
          //  Styles.RemoveAll(style => style.Positions.Count == 0);
          //

           
        }

      
        public override void ParseZipFiles(IEnumerable<FileInfo> zips)
        {
            throw new NotImplementedException();
        }

        protected override string MainRegexString
        {
            get { return Settings.Default.STYLEPROGRAMREGEX; }
        }

        protected override void GetRobotName()
        {

            var text = string.Empty;
            
            // find file
            if (Zip != null)
            {
                var am = Zip.Entries.Single(entry => entry.FileName.Contains(StringResources.KUKA_am));
                var temp =  Zip.ReadEntryLines(am);
              //  var temp = Zip.GetFileText("am.ini").Split('\r');
                text = temp.Single(line => line.Contains(StringResources.KukaRobname));
            }
            else
            {
                // Must be a file
                var file = Files.First(f => f.Name == "am.ini");
               //  text = File.ReadAllText(file.FullName).Split('\r');
//                text = temp.Single(line => line.Contains(StringResources.KukaRobname));
            }
//            text = temp.Single(line => line.Contains(StringResources.KukaRobname));
            
            Name = text.Substring(text.IndexOf("=", StringComparison.Ordinal) + 1);
        }

        protected override void GetProcessData()
        {

            var config = Zip.GetFileText("krc/r1/system/$config.dat");
            var tools = new Regex(Settings.Default.GetProcessRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

            var searches = OptionsViewModel.Instance.SearchExpressions;
           


            var matches = tools.Match(config);
            while (matches.Success)
            {
                if (matches.Groups[1].ToString() == "PROCESS1")
                {
                    Process1 = matches.Groups[2].ToString();
                }
                else if (matches.Groups[1].ToString() == "PROCESS2")
                {
                    Process2 = matches.Groups[2].ToString();
                }

                matches = matches.NextMatch();
            }

            foreach (var search in searches)
                AddOptions(search.Expression, RegexOptions.Multiline|RegexOptions.IgnoreCase, config, search.Name);

            var ffr = new Regex(@"(PROCESS[1-2]+)=.*FFR +#([^,]+)",RegexOptions.Multiline);
                   matches=ffr.Match(config);
            while(matches.Success)
            {
                var option = new SearchParam
                {
                    Name = $"{matches.Groups[1].Value} FFR  Enabled",
                    Value =
                        string.Equals(matches.Groups[2].ToString(), "WITH", StringComparison.OrdinalIgnoreCase)
                            .ToString()
                            .ToUpperInvariant()
                };
                OptionalValues.Add(option);
                matches = matches.NextMatch();
            }
        }


      
        private void AddOptions(string search,RegexOptions options,string text,string format)
        {

            var regex=new Regex(search,options);
            var matches=regex.Match(text);
            while(matches.Success)
            {
                var option = new SearchParam
                {
                    Name = string.Format(format, matches.Groups[1]),
                    Value = matches.Groups[2].ToString()
                };
                OptionalValues.Add(option);
                matches=matches.NextMatch();
            }
        }

        private string GetFileText(string filename)
        {
            if (Zip != null)
               return Zip.GetFileText(filename);

            // Change filename for Directory Files
            filename = filename.Replace("/", "\\");
           

            var file = Files.Single(f => f.FullName.ToLowerInvariant().Contains(filename));
            return File.ReadAllText(file.FullName);

        }

        protected override void GetConfigData()
        {
            var config = GetFileText("krc/r1/system/$config.dat");


            Tools = new ObservableCollection<Tool>();

            var tools = new Regex(Settings.Default.ToolDataRegex, RegexOptions.IgnorePatternWhitespace);
            var matches = tools.Match(config);
            while (matches.Success)
            {
                var t = new Tool { Number = Convert.ToInt16(matches.Groups[1].ToString()) };

                var toolinfo = new Regex(Settings.Default.KUKATOOLSPLIT, RegexOptions.IgnorePatternWhitespace);
                var toolMatches = toolinfo.Matches(matches.Groups[2].ToString());

                t.X = Convert.ToDouble(toolMatches[0].Value);
                t.Y = Convert.ToDouble(toolMatches[1].Value);

              //  t.Z = Convert.ToDouble(toolMatches[2].Value);
                t.A = Convert.ToDouble(toolMatches[3].Value);
                t.B = Convert.ToDouble(toolMatches[4].Value);
                t.C = Convert.ToDouble(toolMatches[5].Value);

                if (!t.IsEmpty)
                    Tools.Add(t);
                matches = matches.NextMatch();
            }
        }

        public override Position ParsePosition(string line, int linenumber, string file, int style)
        {
            var p = new Position { Style = style, Filename = file };
            line = line.Replace(";FOLD", string.Empty);
            line = line.Substring(0, line.IndexOf(";", StringComparison.Ordinal)).Trim();
            var spl = line.Trim().Split(' ');
            p.MotionType = (PositionType)Enum.Parse(typeof(PositionType), spl[0]);
            p.Name = spl[1];

            var veld = new Regex("[^=]*=/s*([0-9]+)", RegexOptions.IgnorePatternWhitespace);
            var matches = veld.Match(line);
            while (matches.Success)
            {
                matches = matches.NextMatch();
            }
            //TODO Need a better way to get the line
            var vel = new Regex("[^=]*=/s*([0-9]+)").Matches(line);
            // p.Velocity = Convert.ToDouble(vel.Groups[0].ToString());
            //        //    p.Velocity = Convert.ToDouble(spl[4]);
            p.IsContinuous = string.Equals(spl[2], "CONT", StringComparison.OrdinalIgnoreCase);
            p.LineNumber = linenumber;
            return p;
        }


        public ICollection<AbstractWeld> ParseRobotFile(ZipEntry zip,int style=-1)
        {
            var fileLines = Zip.GetFileLines(zip.FileName);

            var filename = new FileInfo(zip.FileName).Name;
            return ParseRobotFileLines(fileLines,filename, style);
        }
        private ICollection<AbstractWeld> ParseRobotFileLines(string[] fileLines,string filename, int style=-1)
        {

            //TODO Get Zones



            var fileName = new FileInfo(filename);
            var result = new List<AbstractWeld>();
            for (var i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(StringResources.EnterZone))
                {
                    var zone = new Zone
                    {
                        ZoneFilePosition = i,
                        LineBefore = fileLines[i - 1]
                    };

                    var linebefore = fileLines[i - 1];
                  if (linebefore.Contains(StringResources.KUKAEndfold))
                  {
                      var j = i - 2;
                  
                      while (!fileLines[j].Contains(StringResources.KUKAFold)&&j>0)
                          j--;
                  
                      zone.LineBefore = fileLines[j];
                  
                  }

                  //HACK
                  var dec = fileLines[i].Replace(";FOLD", string.Empty);
                  dec = dec.Replace(@";%{PE}%MKUKATPUSER", string.Empty);

                  zone.Declaration = dec;
                  var builder = new StringBuilder();
                  foreach (var value in fileLines)
                  {
                      if (value.Trim().StartsWith(";"))
                      builder.AppendLine(value);
                  }

                    zone.EntryFilename = filename;

                    Zones.Add(zone);


                }

                SelectedZone = Zones.FirstOrDefault();

                AbstractWeld position = null;
                if (fileLines[i].Contains(";FOLD SG.P_") | fileLines[i].Contains(";FOLD S.PLIN "))
                    position = ParseRobotLine(fileLines[i], i, result.Count + 1, filename, style);
                else
                {
                    if (fileLines[i].ToUpper().Contains(";FOLD PTP"))
                        position = ParsePosition(fileLines[i], i, fileName.Name);
                    if (fileLines[i].ToUpper().Contains(";FOLD LIN"))
                        position = ParsePosition(fileLines[i], i, fileName.Name);
                }

                if (position != null)
                {
                    position.Style = style;
                    result.Add(position);
                }

            }
            return result;
        }


        public ICollection<AbstractWeld> ParseRobotFile(string filename, int style = -1)
        {


            var fileLines = File.ReadAllLines(filename);
            return ParseRobotFileLines(fileLines,filename, style);


        }

        public AbstractWeld ParseRobotLine(string line, int linenumber,int sequence,string filename,int style)
        {
            //TODO Need to be able to determine if Weld is Spot or servo
            var w = new RobotWeld(filename, style)
            {
                Name = GetWeldViewModel.GetRegexMatch(Settings.Default.KukaWeldNameRegex, line),
                Sequence = sequence,
                Velocity = Convert.ToDouble(GetWeldViewModel.GetRegexMatch(Settings.Default.VelocityRegex, line)),
                LineNumber = linenumber,
                Schedule = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldScheduleRegex, line),
                Id = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldIdRegex, line),
                Thickness = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldThicknessRegex, line),
                Force = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldForceRegex, line),
                Gun1 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun1Regex, line),
                Gun2 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun2Regex, line),
                Gun3 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun3Regex, line),
                Gun4 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun4Regex, line),
                Equalizer1 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr1Regex, line).Trim() == "X",
                Equalizer2 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr2Regex, line).Trim() == "X",
                Equalizer3 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr3Regex, line).Trim() == "X",
                Equalizer4 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr4Regex, line).Trim() == "X"
            };

            GetWeldViewModel.GetRegexMatch(Settings.Default.AnticpRegex, line);

            return w;
        }

        public AbstractWeld ParsePosition(string line, int linenumber,string filename)
        {
            var p = new RobotWeld(filename,-1);
            
            line = line.Replace(";FOLD", string.Empty);
            line = line.Substring(0, line.IndexOf(";", StringComparison.Ordinal)).Trim();
            var spl = line.Trim().Split(' ');
            p.MotionType = (PositionType)Enum.Parse(typeof(PositionType), spl[0]);
            p.Name = spl[1];
//            p.Velocity = Convert.ToDouble(spl[4]);
            p.IsContinuous = string.Equals(spl[2], "CONT", StringComparison.OrdinalIgnoreCase);
            p.LineNumber = linenumber;
            return p;
        }

        protected override void AddStylePrograms(IEnumerable<string> filenames, int styleno, string programname)
        {
            var style = new StyleProgramViewModel
            {
                Style = styleno,
                Name = programname,
                StyleProgramName = programname
            };


            foreach (var filename in filenames)
            {
                var name = filename.ToLowerInvariant();
                if (Zip!=null)
                {

                    var file = Zip.GetZipEntry(name);
                    if (file == null)
                        continue;

                    var positions = ParseRobotFile(file, styleno);
                    if (positions.Count > -1)
                    {
                        foreach (var position in positions)
                        {
                        //TODO Dont need any longer    position.Style = styleno;
                            style.Positions.Add(position);

                        }
                    }
                    continue;

                }
                if (Files.Any(f => f.Name.ToLowerInvariant() == name.ToLowerInvariant()))
                {
                    var file = Files.FirstOrDefault(f => f.Name.ToLowerInvariant() == name);

                    var positions = ParseRobotFile(file.FullName, styleno);
                    if (positions.Count > -1)
                    {
                        foreach (var position in positions)
                        {
                            position.Style = styleno;
                            style.Positions.Add(position);

                        }
                    }
                }
            }
                Styles.Add(style);




        }

        protected override void GetStyleProgram(string programname, int stylenumber)
        {
            //TODO find a better way to Implement this
            if (IsIgnored(stylenumber) | stylenumber > 10)
                return;

            TempPath = Path.GetTempPath();
            GetRobotName();
            var programs = new List<string>();
            programname = programname.Trim().Replace(@"(", string.Empty).Trim().ToLower();

            programname += ".src";

            if (programname.StartsWith("default"))
                return;
            string style;
            if (Zip != null)
            {

            

                var pg = Zip.Entries.First(zip => zip.FileName.ToLower().Contains(programname));

                style = Zip.ReadEntryText(pg);

            }
            else
            {
                var path = Files.Single(f => f.Name.ToLowerInvariant() == programname.ToLowerInvariant());
                style = File.ReadAllText(path.FullName);
                _temp = path.DirectoryName;

            }

          

            var regex = new Regex(Settings.Default.ProgramREGEX, RegexOptions.Multiline);

            var matches = regex.Match(style);
            while (matches.Success)
            {

                var program = matches.Groups[1].ToString().Replace(@"(", string.Empty);
                programs.Add(program.Trim()+FileExtension);

                matches = matches.NextMatch();
            }

            AddStylePrograms(programs, stylenumber, programname);
        }

        private string _temp;

        public override AbstractWeld GetWeld(Match match, int linenumber, int sequence, string filename, int style)
        {
            var weld = new RobotWeld(filename, style);

            var line = match.ToString();

            // Is Line A WeldGun
            weld.IsServoWeld = GetWeldViewModel.GetRegexMatch(Settings.Default.IsServoWeldKuka, line).Length > 0;
            weld.IsSpotWeld = !weld.IsServoWeld;
//            weld.Line = line;
            //TODO Need to be able to determine if Weld is Spot or servo
            Name = GetWeldViewModel.GetRegexMatch(Settings.Default.KukaWeldNameRegex, line);

            weld.Sequence = sequence;
            weld.Filename = filename;

            weld.Velocity = Convert.ToDouble(GetWeldViewModel.GetRegexMatch(Settings.Default.VelocityRegex, line));
            weld.LineNumber = linenumber;
            weld.Schedule = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldScheduleRegex, line);
            weld.Id = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldIdRegex, line);
            weld.Thickness = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldThicknessRegex, line);
            weld.Force = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldForceRegex, line);

            weld.Gun1 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun1Regex, line);
            weld.Gun2 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun2Regex, line);
            weld.Gun3 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun3Regex, line);
            weld.Gun4 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun4Regex, line);
            weld.Equalizer1 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr1Regex, line).Trim() == "X";
            weld.Equalizer2 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr2Regex, line).Trim() == "X";
            weld.Equalizer3 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr3Regex, line).Trim() == "X";
            weld.Equalizer4 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr4Regex, line).Trim() == "X";

            var antic = GetWeldViewModel.GetRegexMatch(Settings.Default.AnticpRegex, line);
            
            return weld;
        }

        public override AbstractWeld GetStudWeld(string line, int linenumber, int sequence, string filename, int style)
        {
            throw new NotImplementedException();
        }

      

        public override void GetMainPrograms(string regexString)
        {

            var cell = Zip.Entries.Single(z => z.FileName.ToLower() == "krc/r1/cellstart.src");

            var celltext = Zip.ReadEntryText(cell);
//            var celltext = GetFileText("krc/r1/cellstart.src");
            var regex = GetRegex(regexString);

            var matches = regex.Match(celltext);
            while (matches.Success)
            {


                GetStyleProgram(matches.Groups[2].ToString(), Convert.ToInt32(matches.Groups[1].ToString()));
                matches = matches.NextMatch();
            }
        }

        [XmlType("KukaWeld")]
        public class RobotWeld : AbstractWeld
        {
            /// <summary>
            /// Parameterless Constructor for Serialization
            /// </summary>
            public RobotWeld() { }
            public override Position Add(string line, int i, int position, string name, int style)
            {
                throw new NotImplementedException();
            }
            public RobotWeld(string filename, int style)
                : base(filename, style)
            {
            }

            public RobotWeld(string line, int linenumber, int sequence, string filename, int style)
                : base(filename, style)
            {
                Line = line;
                LineNumber = linenumber;
                Sequence = sequence;
                Filename = filename;
                Style = style;
            }
        }

        public void ParseRobot(IEnumerable<FileInfo> files)
        {

        }

        public override Regex PtpWeldRegex
        {
            get { return new Regex(StringResources.KUKAServoWeldPTP, RegexOptions.IgnoreCase); }
        }

        public override Regex LinWeldRegex
        {
            get { return new Regex(Settings.Default.KukaWeldNameRegex, RegexOptions.IgnoreCase|RegexOptions.Multiline); }
        }

        public override Regex LinStudWeldRegex
        {
            get { return new Regex(StringResources.KUKAStudWeldLIN, RegexOptions.IgnoreCase); }
        }

        public override Regex PtpStudWeldRegex
        {
            get { return new Regex(StringResources.KUKAStudWeldPTP, RegexOptions.IgnoreCase); }
        }

        public override string FileExtension
        {
            get { return Settings.Default.KUKAEXTENSION; }
        }



        public override Regex RivetWeldRegex
        {
            get { throw new NotImplementedException(); }
        }
    }

}
