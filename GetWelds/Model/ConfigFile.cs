using System.Collections.Generic;

namespace GetWelds.Model
{
    public class ConfigFile
    {

    }


    public interface ISearchOptions
    {
        List<ISearch> SearchStrings { get; set; }
    }
    public interface ISearch
    {
        string SearchString { get; set; }

    }
}
