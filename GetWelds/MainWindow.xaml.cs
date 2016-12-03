using GalaSoft.MvvmLight.Messaging;
using GetWelds.Controls;
using GetWelds.Messages;
using GetWelds.ViewModels;
using System.IO;
using System.Xml.Serialization;
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
            this.Closing += MainWindow_Closing;
            Messenger.Default.Register<GetFileMessage>(this,GetMessage);
        }

      

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          
        }

        private void GetMessage(GetFileMessage msg)
        {
            msg.GetMessage(this);

        }

        private void ShowAbout(object sender, System.Windows.RoutedEventArgs e)
        {
            var about = new ShowAbout();
            about.Owner = this;
            about.ShowInTaskbar = false;
            about.ShowDialog();
         }

     



    }
}
