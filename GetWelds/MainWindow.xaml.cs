using GalaSoft.MvvmLight.Messaging;
using GetWelds.Controls;
using GetWelds.Messages;

namespace GetWelds
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow:MahApps.Metro.Controls.MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            Messenger.Default.Register<GetFileMessage>(this,GetMessage);
        }


        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          
        }

        private void GetMessage(GetFileMessage msg)
        {
            msg.GetMessage(this);

        }

        private void ShowAbout(object sender, System.Windows.RoutedEventArgs e)
        {
            var about = new ShowAbout
            {
                Owner = this,
                ShowInTaskbar = false
            };
            about.ShowDialog();
         }

     



    }
}
