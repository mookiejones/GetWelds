using System;
using GalaSoft.MvvmLight.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GetWelds.Model;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;
using System.Windows.Data;
namespace GetWelds.ViewModel
{
    public class ParseDirectoriesViewModel : ViewModelBase
    {
        
        #region · Properties ·

        
        #region · SelectedRobot ·
        /// <summary>
        /// The <see cref="SelectedRobot" /> property's name.
        /// </summary>
        public const string SelectedRobotPropertyName = "SelectedRobot";

        private Robot _selectedRobot = new Robot();

        /// <summary>
        /// Sets and gets the SelectedRobot property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public Robot SelectedRobot
        {
            get
            {
                return _selectedRobot;
            }
            set
            {
                if (Set(SelectedRobotPropertyName, ref _selectedRobot, value, true))
                {
                    value.SelectedStyle= value.Styles.FirstOrDefault();
                }
            }
        }
        #endregion
        

        #endregion

        
        #region · Collections ·

        
        #region · Robots ·
        /// <summary>
        /// The <see cref="Robots" /> property's name.
        /// </summary>
        public const string RobotsPropertyName = "Robots";

        private List<Robot> _robots = new List<Robot>();

        /// <summary>
        /// Sets and gets the Robots property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public List<Robot> Robots
        {
            get
            {
                return _robots;
            }
            set
            {
                Set(RobotsPropertyName, ref _robots, value, true);
            }
        }
        #endregion
        

        
        #endregion
    

        #region · Fields ·
        DirectoryInfo _directory = null;
    
        #endregion

        private int index = 0;

       
        public ParseDirectoriesViewModel()
        {
            if (IsInDesignMode)
            {
                Deserialize();  
            }
        }
        void Deserialize()
        {
            var serializer = new XmlSerializer(typeof(List<Robot>));
            using (var reader = new XmlTextReader("robots.xml"))
            {
                var can = serializer.CanDeserialize(reader);
                Robots = (List<Robot>)serializer.Deserialize(reader);
            }
        }
        void ParseDirectories()
        {


            // Find Zip files in directory
             var zips = from zip in _directory.GetFiles("*.zip", SearchOption.AllDirectories) select zip;
            foreach (var zip in zips)
            {
                index++;               
                ParseZipFile(zip.FullName);
            }


       // var nonweldingrobots = from robot
       //                        in Robots
       //                        where robot.Styles == 0
       //                        select robot;



                         

         

        }

        void ParseZipFile(string filename)
        {

            try
            {
                using (var zip = new RobotZipFile(filename))
                {
                    var r = new Robot(zip);
                    Robots.Add(r);
                    RaisePropertyChanged("Robots");
                }

               // SerializeRobots();
            }
            catch (Exception ex)
            {
               
            }

            


        }

        void SerializeRobots()
        {
            var serializer = new XmlSerializer(typeof(List<Robot>));
            Stream fs = new FileStream("robots.xml", FileMode.Open);
            using (var writer = new StreamWriter(fs))
            {
                serializer.Serialize(writer, Robots);
            }

            serializer = null;
        }




        #region DropCommand
      

        private RelayCommand<string> _dropCommand;

        /// <summary>
        /// Gets the DropCommand.
        /// </summary>
        public RelayCommand<string> DropCommand
        {
            get
            {
                return _dropCommand
                    ?? (_dropCommand = new RelayCommand<string>(ExecuteDropCommand));
            }
        }

        private void ExecuteDropCommand(string dropdata)
        {


            if (dropdata == null) return;

            DropType fileDrop = DropType.None;
            FileInfo file = null;
            if (string.IsNullOrEmpty(dropdata)) return;


            
            // What kind of data is it?
            int len = dropdata.Length;


            // Check to see if data is file
            if (len > 0)
            {
                // SingleFile                
                file = new FileInfo(dropdata);

                fileDrop = file.Exists ? DropType.File : Directory.Exists(dropdata) ? DropType.Directory : DropType.None;

                if (file.Extension.Equals(".zip", System.StringComparison.OrdinalIgnoreCase))
                    fileDrop = DropType.Zip;

                if (fileDrop == DropType.None)
                {
                    MessageBox.Show("File is not Directory,Folder, or Zip");
                    return;
                }
            }

            switch (fileDrop)
            {
                case DropType.Directory:
                    ParseDirectory(dropdata);
                    break;
                case DropType.File:
                   // DataContext = new GetWeldViewModel(data);
                    break;
                case DropType.Zip:
                    ParseZip(file);
                    break;
                default:
                    return;
            }

        }
        void ParseZip(FileInfo file)
        {

        }
        #endregion
        void ParseDirectory(string directoryname)
        {
            var directory = new DirectoryInfo(directoryname);


            if (directory != null)
            {
                _directory = directory;

                ParseDirectories();
            }
        }

        ~ParseDirectoriesViewModel()
        {
           // var serial = new System.Xml.Serialization.XmlSerializer(typeof(List<Robot>));
           // var reader = new StreamWriter("c:\\robots.xml");
           // serial.Serialize(reader, Robots);
        }
    }


 


}