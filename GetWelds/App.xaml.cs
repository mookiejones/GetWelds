using System;
using System.IO;
using GetWelds.ViewModels;
using GalaSoft.MvvmLight.Threading;

namespace GetWelds
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {

            DispatcherHelper.Initialize();
           
            var args = Environment.GetCommandLineArgs();

            var wvm = new GetWeldViewModel();

            for (var i = 1; i < args.Length;i++)
            {

                wvm.ParseRobotFile(args[i],1);

                var savefile = Path.ChangeExtension(args[i], ".xml");

                wvm.ExportToXMLFile(savefile);
            }

            if (args.Length>1)
            Current.Shutdown();
        }
    }
}
