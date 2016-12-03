using System.Windows;
using System.Windows.Controls;

namespace GetWelds
{
    /// <summary>
    /// Interaction logic for WeldControl.xaml
    /// </summary>
    public partial class WeldControl
    {
        public WeldControl()
        {
            InitializeComponent();
        }
   

        private void DataGrid_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DataGrid_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var data = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (data != null)
                foreach (var file in data)
                {
                    var model = this.DataContext as GetWeldViewModel;
                    if (model != null) model.ParseRobotFile(file);
                }
        }
       
    }
}
