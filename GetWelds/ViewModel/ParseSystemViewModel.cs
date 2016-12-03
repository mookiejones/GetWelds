using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GetWelds.Model;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace GetWelds.ViewModel
{

    
    public class ParseSystemViewModel:ViewModelBase
    {
        
        #region · Fields ·
        private DirectoryInfo _rootDirectory;
        #endregion
     
        public DirectoryInfo RootDirectory { get { return _rootDirectory; } set { _rootDirectory = value; RaisePropertyChanged("RootDirectory"); } }
      
        public ObservableCollection<FileInfo> ProgramFiles{get;set;}


        #region · Command members ·
        private RelayCommand _getRootDirectoryCommand;
        public RelayCommand GetRootDirectoryCommand { get { return _getRootDirectoryCommand ?? (_getRootDirectoryCommand = new RelayCommand(GetRootDirectory, CanGetRootDirectory)); } }

        private void GetRootDirectory()
        {
            var fbd = new VistaFolderBrowserDialog();
            fbd.Description = "Select Root Directory for all robots";
            if (fbd.ShowDialog() == true)
            {
                RootDirectory = new DirectoryInfo(fbd.SelectedPath);
            }
        }

        private bool CanGetRootDirectory()
        {
            return true;
        }
        #endregion

      


     
            
        public ObservableCollection<Position> Positions{get;set;}
        private RelayCommand _refreshProgramsCommand;
        public RelayCommand RefreshProgramsCommand { get { return _refreshProgramsCommand??(_refreshProgramsCommand=new RelayCommand(ExecuteRefreshPrograms,CanExecuteRefreshPrograms)); } }

        private bool CanExecuteRefreshPrograms()
        {
            return RootDirectory.Exists;
        }

        private void ExecuteRefreshPrograms()
        {
            FindPrograms();
        }

       
        void FindPrograms()
        {
            var files = from directory in RootDirectory.GetDirectories("*R1*") select directory;

            
        }

          public void ParseRobotFile(string filename)
        {

           var FileName = new FileInfo(filename);

            Positions.Clear();
            var fileLines = File.ReadAllLines(filename);

            for (var i = 0; i < fileLines.Length; i++)
            {
                //if (fileLines[i].Contains(";FOLD SG.P_"))
                //    Positions.Add( ParseRobotLine(fileLines[i], i));
                //else
                //{
                //    //if (fileLines[i].ToUpper().Contains(";FOLD PTP"))
                //    //    Positions.Add(ParsePosition(fileLines[i], i));
                //    //if (fileLines[i].ToUpper().Contains(";FOLD LIN"))
                //    //    Positions.Add(GetWeldViewModel.ParsePosition(fileLines[i], i));
                //}
            }
          }
      


        public ParseSystemViewModel()
        {
            ProgramFiles = new ObservableCollection<FileInfo>();
            Positions=new ObservableCollection<Position>();
        }


    }

    public class KTPOWeldRobot
    {
        public ObservableCollection<KTPOWeldRobotProgram> RobotPrograms { get; set; }

        public void GetPrograms()
        {
            throw new System.NotImplementedException();
        }
    
        public KTPOWeldRobot(string cellstartfilepath)
        {

        }

    }

}
