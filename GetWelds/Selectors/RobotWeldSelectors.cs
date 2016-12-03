using System.Windows;
using System.Windows.Controls;
using GetWelds.Robots;

namespace GetWelds.Selectors
{
    public class RobotWeldSelectors:DataTemplateSelector
    {       
        public DataTemplate None { get; set; }

        public DataTemplate Fanuc { get; set; }
        public DataTemplate Kuka { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return None;
            if (item is Fanuc)
                return Fanuc;
            if (item is Kuka)
                return Kuka;
            return null;
        }
    }
}
