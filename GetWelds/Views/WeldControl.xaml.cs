using GetWelds.ViewModels;

namespace GetWelds.Views
{
    /// <summary>
    /// Interaction logic for WeldControl.xaml
    /// </summary>
    public partial class WeldControl
    {
        private GetWeldViewModel model;
        public WeldControl()
        {
            InitializeComponent();
            model = DataContext as GetWeldViewModel;
        }
   

     
    }

    public enum DropType
    {
        None,
        File,
        Directory,
        Directories,
        Zip,
    }
}
