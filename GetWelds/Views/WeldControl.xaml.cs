using GetWelds.ViewModels;

namespace GetWelds.Views
{
    /// <summary>
    /// Interaction logic for WeldControl.xaml
    /// </summary>
    public partial class WeldControl
    {
        private GetWeldViewModel _model;
        public WeldControl()
        {
            InitializeComponent();
            _model = DataContext as GetWeldViewModel;
        }
   

     
    }

    public enum DropType
    {
        None,
        File,
        Directory,
        Directories,
        Zip
    }
}
