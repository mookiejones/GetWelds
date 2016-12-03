using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using GetWelds.Model;

namespace GetWelds.Robots
{
     
    public interface IRobot
    {
        void ParseRobot(IEnumerable<FileInfo> files);
        void GetSystems(IEnumerable<FileInfo> files);
        Regex LINWeldRegex { get;  }
        Regex PTPWeldRegex { get;  }

        StyleProgramViewModel SelectedStyle { get; set; }

        AbstractWeld GetWeld(Match match, int linenumber, int sequence, string filename, int style);
        List<StyleProgramViewModel> Styles { get; set; }
        ObservableCollection<Weld> Positions { get; set; }


        string Name { get; set; }
        string Process1 { get; set; }
        string Process2 { get; set; }
    }

}
