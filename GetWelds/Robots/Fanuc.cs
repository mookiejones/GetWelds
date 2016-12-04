using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GetWelds.ViewModels;
using GetWelds.Properties;
using GetWelds.Converters;
using System.Xml.Serialization;

namespace GetWelds.Robots
{
    [System.Xml.Serialization.XmlInclude(typeof(RobotWeld))]
    public class Fanuc : AbstractRobot
    {

        #region · Constructor ·

        public Fanuc(List<RobotFile> files)
        {

            Files = files;
        }

        #endregion

        public Fanuc()
        {
        }


     

        
        #region · Overriden Methods ·




        public override void GetMainPrograms(string regexstring)
        {
                Styles = new List<StyleProgramViewModel>();
                var regex = new Regex(regexstring, RegexOptions.IgnoreCase);
                IEnumerable<RobotFile> files = new List<RobotFile>();

                files = from file in Files
                        where regex.IsMatch(file.Name)
                        select file;


                var styles = from file in Files
                             where regex.IsMatch(file.Name)
                             select new StyleProgramViewModel
                             {
                                 
                             Style=Convert.ToInt32(GetStyleNumber(file.Name)),
                             
                             
                   Name = file.Name,
                   StyleProgramName = file.Name,
                   DirectoryName = file.DirectoryName,
                   FullName = file.FullName,
                   Text = File.ReadAllText(file.FullName)
              
                             };

            Styles.AddRange(styles.ToList());


        }

        public override AbstractWeld GetWeld(Match match, int linenumber, int sequence, string filename, int style)
        {
            var weld = new RobotWeld(filename, style)
            {
                IsServoWeld = true,
                Line = match.ToString(),
                Sequence = sequence,
                Velocity = Convert.ToDouble(match.Groups[3].ToString()),
                LineNumber = linenumber,
                Schedule = match.Groups[6].ToString(),
                Force = match.Groups[5].ToString(),
                Name = GetWeldName(match.Groups[2].ToString()),
                Id = GetWeldId(match.Groups[2].ToString()),
                StartDistance = Convert.ToInt32(match.Groups[5].ToString()),
                EndDistance = Convert.ToInt32(match.Groups[7].ToString()),
                Filename = filename
            };
            return weld;
         
        }

        public override AbstractWeld GetStudWeld(string line, int linenumber, int sequence, string filename, int style)
        {
            throw new NotImplementedException();
        }


        public override void ProcessRobotFiles()
        {

            
            foreach (var style in Styles)
            {

                var regex = new Regex(Settings.Default.FANUCPROGRAMCALL);

               
                var lines = File.ReadAllLines(style.FullName);


                foreach (var matches in lines.Select(line => regex.Match(line)).Where(matches => matches.Groups.Count > 1))
                {
                    var f = matches.Groups[1].ToString().Replace(";", string.Empty).Trim();


                    var file = new FileInfo(Path.Combine(style.DirectoryName, f + Settings.Default.FANUCEXTENSION));
                    var result = style.AddProgram(file, this);
                    if (result)
                        style.Programs.Add(f);
                }
            }

            Styles.RemoveAll(style => style.Positions.Count == 0);


        }

     

        public override void ParseZipFiles(IEnumerable<FileInfo> zips)
        {
            throw new NotImplementedException();
        }

        protected override void GetRobotName()
        {
            //TODO Find out if i can get RobotName from file

        }

        protected override void GetProcessData()
        {
            //TODO Try to Implement this
        }

        protected override void GetConfigData()
        {
            //TODO Try to Implement this
            /*
            var config = Files.FirstOrDefault(f => f.Name.ToUpperInvariant() == "DEFINER.LS");


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
                t.Z = Convert.ToDouble(toolMatches[2].Value);
                t.A = Convert.ToDouble(toolMatches[3].Value);
                t.B = Convert.ToDouble(toolMatches[4].Value);
                t.C = Convert.ToDouble(toolMatches[5].Value);

                if (!t.IsEmpty)
                    Tools.Add(t);
                matches = matches.NextMatch();
            }
             * */
        }

        public override Position ParsePosition(string line, int linenumber, string file, int style)
        {
            throw new NotImplementedException();
        }

        protected override void AddStylePrograms(IEnumerable<string> filenames, int style, string programname)
        {
            throw new NotImplementedException();
        }

        protected override void GetStyleProgram(string programname, int stylenumber)
        {
            throw new NotImplementedException();
        }

        #endregion

        
        #region · Overriden Properties ·


        public override Regex PtpWeldRegex
        {
            get { return new Regex(OptionsViewModel.Instance.RobotOptions.FanucOptions.ServoWeldString,RegexOptions.IgnoreCase); }
        }

        public override Regex LinWeldRegex
        {
            get { return new Regex(OptionsViewModel.Instance.RobotOptions.FanucOptions.ServoWeldString, RegexOptions.IgnoreCase); }
        }


        public override Regex RivetWeldRegex
        {
            get { return new Regex(OptionsViewModel.Instance.RobotOptions.FanucOptions.RivetString,RegexOptions.IgnoreCase); }
        }


        public override Regex LinStudWeldRegex
        {
            get { return new Regex(OptionsViewModel.Instance.RobotOptions.FanucOptions.StudString, RegexOptions.IgnoreCase); }
        }

        public override Regex PtpStudWeldRegex
        {
            get { return LinStudWeldRegex; }
        }



        public override string FileExtension
        {
            get { return Settings.Default.FANUCEXTENSION; }
        }


        public override Type Weld
        {
            get { return typeof(RobotWeld); }
        }

        #endregion
    
            

        public void ParseRobot(DirectoryInfo dir)
        {

        }

   


        public string GetWeldName(string pos)
        {
            // Find Colon
            var colonPos = pos.IndexOf(':');

            if (colonPos == -1)
                return pos;

            // get result 
            var result = pos.Substring(0, colonPos);

            return result;
        }

        public string GetWeldId(string pos)
        {

            // Find Colon
            var colonPos = pos.IndexOf(':');

            // increment position
            colonPos++;

            // get result 
            var result = pos.Substring(colonPos);

            return result;

        }

        [XmlType("FanucWeld")]
        public class RobotWeld : AbstractWeld
        {
            public RobotWeld()
            {
            }


            public RobotWeld(string filename, int style) : base(filename, style)
            {
              
            }

               public  RobotWeld(string line, int linenumber, int sequence, string filename, int style)
            : base(filename, style)
        {
           
        }


               public override Position Add(string line, int linenumber, int sequence, string filename, int style)
               {
                   Filename = filename;
                   Style = style;


                // Is Line A WeldGun
                IsServoWeld = GetWeldViewModel.GetRegexMatch(Settings.Default.IsServoWeldKuka, line).Length > 0;
                IsSpotWeld = !IsServoWeld;
                Line = line;
                //TODO Need to be able to determine if Weld is Spot or servo
                Name = GetWeldViewModel.GetRegexMatch(Settings.Default.KukaWeldNameRegex, line);

                Sequence = sequence;
                Filename = filename;

                Velocity = Convert.ToDouble(GetWeldViewModel.GetRegexMatch(Settings.Default.VelocityRegex, line));
                LineNumber = linenumber;
                Schedule = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldScheduleRegex, line);
                Id = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldIdRegex, line);
                Thickness = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldThicknessRegex, line);
                Force = GetWeldViewModel.GetRegexMatch(Settings.Default.WeldForceRegex, line);

                Gun1 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun1Regex, line);
                Gun2 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun2Regex, line);
                Gun3 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun3Regex, line);
                Gun4 = GetWeldViewModel.GetRegexMatch(Settings.Default.Gun4Regex, line);

                Equalizer1 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr1Regex, line).Trim() == "X";
                Equalizer2 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr2Regex, line).Trim() == "X";
                Equalizer3 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr3Regex, line).Trim() == "X";
                Equalizer4 = GetWeldViewModel.GetRegexMatch(Settings.Default.Eqlzr4Regex, line).Trim() == "X";

                GetWeldViewModel.GetRegexMatch(Settings.Default.AnticpRegex, line);

                   return this;
               }
        }



        protected override string MainRegexString
        {
            get
            {
                return OptionsViewModel.Instance.RobotOptions.FanucOptions.StyleName;
            }
        }

      
    }

}
