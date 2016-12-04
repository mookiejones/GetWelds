using System;
using System.Xml.Serialization;
using GetWelds.Properties;
using GetWelds.ViewModels;

namespace GetWelds
{
    [System.Xml.Serialization.XmlInclude(typeof(Position))]
    public class Weld :AbstractWeld
    {
        

      
          
        #region · Properties ·

      
          

        #endregion · Properties ·

        public Weld() { }
        public Weld(string filename, int style)
            : base(filename, style)
        {
            Filename = filename;
        }

        public Weld(string line, int linenumber, int sequence, string filename, int style)
            : base(filename, style)
        {
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

            var antic = GetWeldViewModel.GetRegexMatch(Settings.Default.AnticpRegex, line);
        }

      
        private WeldGunType _weldGunType = WeldGunType.None;

        public override Position Add(string line, int i, int position, string name, int style)
        {
            throw new NotImplementedException();
        }
    }

    public enum WeldGunType { Servo, Spot, None }
}