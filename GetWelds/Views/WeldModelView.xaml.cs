using System.Windows;
using System.Windows.Controls;

namespace GetWelds.Views
{
    /// <summary>
    /// Interaction logic for WeldModelView.xaml
    /// </summary>
    public partial class WeldModelView : UserControl
    {
        public WeldModelView()
        {
            InitializeComponent();
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            var gl = new GridLength(1.0, GridUnitType.Auto);


            Grid.ColumnDefinitions[1].Width = gl;
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            var gl = new GridLength(1.0, GridUnitType.Star);


            Grid.ColumnDefinitions[1].Width = gl;
        }
    }
}
