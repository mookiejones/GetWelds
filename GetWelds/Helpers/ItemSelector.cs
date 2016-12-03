using System.Windows;
using System.Windows.Controls;
namespace GetWelds.Converters
{
    public class ItemSelector:DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return base.SelectTemplate(item, container);
        }


    }
}
