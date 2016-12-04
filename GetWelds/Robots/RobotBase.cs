using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace GetWelds.Robots
{

    public class RobotBase : AbstractRobot
    {
        public override void GetMainPrograms(string regexstring)
        {
            throw new NotImplementedException();
        }

        public override Type Weld
        {
            get { return new Position().GetType(); }
        }

        public override void ProcessRobotFiles()
        {
          //  throw new NotImplementedException();
        }

        public override void ParseZipFiles(IEnumerable<FileInfo> zips)
        {
           // throw new NotImplementedException();
        }

        protected override string MainRegexString
        {
            get { return string.Empty; }
        }

        protected override void GetRobotName()
        {
    //        throw new NotImplementedException();
        }

        protected override void GetProcessData()
        {
  //          throw new NotImplementedException();
        }

        protected override void GetConfigData()
        {
//            throw new NotImplementedException();
        }

        public override Position ParsePosition(string line, int linenumber, string file, int style)
        {
            return new Position();
           // throw new NotImplementedException();
        }

        protected override void AddStylePrograms(IEnumerable<string> filenames, int style, string programname)
        {
           // throw new NotImplementedException();
        }

        protected override void GetStyleProgram(string programname, int stylenumber)
        {

//            throw new NotImplementedException();
        }

        public override AbstractWeld GetWeld(Match match, int linenumber, int sequence, string filename, int style)
        {
            return new BaseWeld();
//            throw new NotImplementedException("Contact Mookie to implement this for this type of robot");
        }

        public override AbstractWeld GetStudWeld(string line, int linenumber, int sequence, string filename, int style)
        {
            return new BaseWeld();
        }

        public override Regex PtpWeldRegex
        {
           get{return new Regex(string.Empty); }
        }

        public override Regex LinWeldRegex
        {
            get { return new Regex(string.Empty); }
        }

        public override Regex LinStudWeldRegex
        {
            get { return new Regex(string.Empty); }
        }

        public override Regex PtpStudWeldRegex
        {
            get { return new Regex(string.Empty); }
        }

        public override string FileExtension
        {
            get { return string.Empty; }
        }



        public override Regex RivetWeldRegex
        {
            get { throw new NotImplementedException(); }
        }
    }

  
}
