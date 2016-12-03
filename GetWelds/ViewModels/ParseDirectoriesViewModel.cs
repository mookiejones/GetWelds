using System.Security.Cryptography;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GetWelds.ViewModels;
using GetWelds.Robots;
using GetWelds.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using GetWelds.Converters;
using GetWelds.Model;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Windows.Data;

namespace GetWelds.ViewModels
{
    public enum RobotType { Kuka, Fanuc, ABB, Nachi, Kawasaki, None }

    public class ParseDirectoriesViewModel : ViewModelBase
    {


        

        
    
            

        /// <summary>
        /// The <see cref="StatusText" /> property's name.
        /// </summary>
        public const string StatusTextPropertyName = "StatusText";

        private string _statusText = String.Empty;

        /// <summary>
        /// Sets and gets the StatusText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string StatusText
        {
            get
            {
                return _statusText;
            }

            set
            {
                if (_statusText == value)
                {
                    return;
                }

                RaisePropertyChanging(StatusTextPropertyName);
                _statusText = value;
                RaisePropertyChanged(StatusTextPropertyName);
            }
        }
        #region RobotType

        /// <summary>
        /// The <see cref="RobotType" /> property's name.
        /// </summary>
        public const string RobotTypePropertyName = "RobotType";

        private RobotType _robotType = RobotType.None;

        /// <summary>
        /// Sets and gets the RobotType property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public RobotType RobotType
        {
            get
            {
                return _robotType;
            }

            set
            {
                if (_robotType == value)
                {
                    return;
                }

                RaisePropertyChanging(RobotTypePropertyName);
                _robotType = value;
                RaisePropertyChanged(RobotTypePropertyName);
            }
        }

        #endregion RobotType

        #region · Properties ·


        #region IsSearching
        /// <summary>
        /// The <see cref="IsSearching" /> property's name.
        /// </summary>
        public const string IsSearchingPropertyName = "IsSearching";

        private bool _isSearching = false;

        /// <summary>
        /// Sets and gets the IsSearching property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsSearching
        {
            get
            {
                return _isSearching;
            }

            set
            {
                if (_isSearching == value)
                {
                    return;
                }
                RaisePropertyChanging(IsSearchingPropertyName);
                _isSearching = value;
                RaisePropertyChanged(IsSearchingPropertyName);

            }
        }
        #endregion
        

        #region SelectedRobot

        /// <summary>
        /// The <see cref="SelectedRobot" /> property's name.
        /// </summary>
        public const string SelectedRobotPropertyName = "SelectedRobot";

        private AbstractRobot _selectedRobot = new RobotBase();

        /// <summary>
        /// Sets and gets the SelectedRobot property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public AbstractRobot SelectedRobot
        {
            get
            {
                return _selectedRobot;
            }

            set
            {
                if (_selectedRobot == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedRobotPropertyName);
                _selectedRobot = value;
                RaisePropertyChanged(SelectedRobotPropertyName);
            }
        }

        #endregion SelectedRobot

        #endregion · Properties ·

        #region · Collections ·

        

        #region Robots
        /// <summary>
        /// The <see cref="Robots" /> property's name.
        /// </summary>
        public const string RobotsPropertyName = "Robots";

        private ObservableCollection<AbstractRobot> _robots = new ObservableCollection<AbstractRobot>();

        /// <summary>
        /// Sets and gets the Robots property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<AbstractRobot> Robots
        {
            get
            {
                return _robots;
            }

            set
            {
                if (_robots == value)
                {
                    return;
                }

                RaisePropertyChanging(RobotsPropertyName);
                _robots = value;
                RaisePropertyChanged(RobotsPropertyName);
            }
        }
        #endregion
        

        #endregion · Collections ·

        #region · Fields ·

        private readonly DirectoryInfo _directory = default(DirectoryInfo);

        #endregion · Fields ·

        
        #region · Commands ·

        #region ClearItemsCommand

        private RelayCommand _clearItemsCommand;
        /// <summary>
        /// Gets the ClearItemsCommand.
        /// </summary>
        public RelayCommand ClearItemsCommand
        {
            get
            {
                return _clearItemsCommand
                    ?? (_clearItemsCommand = new RelayCommand(ExecuteClearItemsCommand));
            }
        }

        private void ExecuteClearItemsCommand()
        {
            Robots.Clear();
        }
        #endregion
        #endregion
    
            

        public ParseDirectoriesViewModel()
        {
            if (IsInDesignMode)
            {
               // Deserialize();
            }
       if (!IsInDesignMode)
                    DropCommand = new RelayCommand<DragEventArgs>(OnDrop);
        }

        private void Deserialize()
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<AbstractRobot>));
            using (var reader = new XmlTextReader("robots.xml"))
            {
                if (serializer.CanDeserialize(reader))
                    Robots = (ObservableCollection<AbstractRobot>)serializer.Deserialize(reader);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void ParseZips()
        {
            // Find Zip files in directory
            var zips = from zip in _directory.GetFiles("*.zip", SearchOption.AllDirectories) select zip;
            foreach (var zip in zips)
            {
                ParseZipFile(zip.FullName);
            }
        }

        //TODO Differentiate between welding and non welding robots
        private void ParseZipFile(string filename)
        {
            try
            {
               //using (var zip = new RobotZipFile(filename))
               //{
               //    //TODO Come back to this
               //    //  var r = new Robot(zip);
               //    //  Robots.Add(r);
               //    //  RaisePropertyChanged("Robots");
               //}
                // SerializeRobots();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void SerializeRobots()
        {
            var serializer = new XmlSerializer(typeof(List<AbstractRobot>));
            Stream fs = new FileStream("robots.xml", FileMode.Open);
            using (var writer = new StreamWriter(fs))
            {
                serializer.Serialize(writer, Robots);
            }

            serializer = null;
        }

        #region · Drag Command ·

        #endregion · Drag Command ·

       private RelayCommand<DragEventArgs> _dropCommand;
     
       public RelayCommand<DragEventArgs> DropCommand { get; private set; }

        private void OnDrop(DragEventArgs e)
        {

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                IsSearching = true;
            });
            
            if (e.Data == null)
                return;
            var obj = e.Data.GetData(DataFormats.FileDrop) as string[];
           
            if (obj == null) return;

            var items = obj.ToList();

            // What kind of data is it?
            var len = items.Count;

            var dropdata = items.FirstOrDefault();

            if (dropdata == null)
                throw new NotImplementedException("drop data is null");


            var fileDrop = DropType.None;

            FileInfo file = null;
 


                         

            if (len>0)
            {

                var result = new List<AbstractRobot>();
            
              // Check to see if data is a file
                     file = new FileInfo(dropdata);
                     fileDrop = file.Exists ? DropType.File : Directory.Exists(dropdata) ? DropType.Directory : DropType.None;

                // Need to find out file types
                if (file.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
                    fileDrop = DropType.Zip;

                if (fileDrop == DropType.None)
                {
                    MessageBox.Show("File is not Directory,Folder, or Zip");
                    return;
                    
                }

               
                var zips = from item in items
                            from path in Directory.GetFiles(item, "*.zip", SearchOption.AllDirectories)
                            let entry = new FileInfo(path)
                            where entry.Exists
                            select entry;
                bool taskok;
                if (zips.Any())
                {


                    var count = zips.ToList().Count;
      
                    IsSearching = false;
                    foreach (var zip in zips)
                        Robots.Add(ParseZip(zip));
               

                             if (count==0)
                                  IsSearching = false;
                    return;
                }


                var files = from item in items
                    from path in Directory.GetFileSystemEntries(item, "*.*", SearchOption.AllDirectories)
                    let entry = new FileInfo(path)
                    where entry.Exists && !zips.Any(zip => zip.FullName==entry.FullName)
                    select entry;

            //   if (fileDrop == DropType.Zip)
            //   {
            //       ParseZip(file);
            //       return;
            //   }

                // Directories
                var entries = from item in items
                    from directory in Directory.GetDirectories(item, "*.*", SearchOption.TopDirectoryOnly)
                    select new DirectoryInfo(directory);

                    ParseDirectories(entries.ToList());


                  
            }
         

            // if it is directories, then i need to get all the files out to see what type of robot it is.
            if (fileDrop == DropType.Directory)
            {
                var directoryInfo = new DirectoryInfo(dropdata);
             //   ParseDirectory(directoryInfo);


                switch (fileDrop)
                {
                   
                    case DropType.File:
                        // DataContext = new GetWeldViewModel(data);
                        break;

                    case DropType.Zip:
                        ParseZip(file);
                        break;

                    default:
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            IsSearching = false;
                        });
                        return;
                }
            }
            Console.WriteLine("Complete");
          
           
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = ParseZip((FileInfo)e.Argument);

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var robot = e.Result as AbstractRobot;
            Robots.Add(robot);
            StatusText = String.Format("Completed Parsing {0}", robot.Name);
        }
        private void ParseDirectories(IEnumerable<DirectoryInfo> directories)
        {
            foreach(var directory in directories)
                Robots.Add(ParseDirectory(directory));
        }
        private AbstractRobot ParseDirectory(DirectoryInfo directory)
        {

            Console.WriteLine("Parsing Directory {0}", directory.Name);

            

                var fileInfos = directory.GetFiles("*.*", SearchOption.AllDirectories).ToList();

                var files = (from file in fileInfos select new RobotFile(file)).ToList();
                var robot = GetRobotType(files);
            
           

            robot.Name = directory.Name; ;

            // Remove the parenthesis
            if (robot.Name.Contains("("))
                robot.Name = robot.Name.Replace("(", string.Empty);
            if (robot.Name.Contains(")"))
                robot.Name = robot.Name.Replace(")", string.Empty);


            robot.GetSystems(files);
            robot.ParseRobot(files);

            return robot;
            


        }

       
        private AbstractRobot GetRobotType(RobotZipFile zip)
        {
            if (zip.Entries.Any(z => String.Equals(Path.GetExtension(z.FileName),".src",StringComparison.OrdinalIgnoreCase)))
                return new Kuka();

            if (zip.Entries.Any(z => Path.GetExtension(z.FileName).ToLowerInvariant() == ".ls"))
                return new Fanuc();

            return null;
        }
        private AbstractRobot GetRobotType(List<RobotFile> files)
        {
            // What file types does it contain?
            if (files.Any(f => f.Extension.ToLowerInvariant() == ".src"))
            {
                return new Kuka();
            }

            if (files.Any(f => f.Extension.ToLowerInvariant() == ".ls"))
            {
                return new Fanuc();
            }
            throw new NotImplementedException("Cant find robot type");
            return new RobotBase();
        }

        #region DragCommand

        private RelayCommand<object> _dragCommand;

        /// <summary>
        /// Gets the DragCommand.
        /// </summary>
        public RelayCommand<object> DragCommand
        {
            get
            {
                return _dragCommand
                    ?? (_dragCommand = new RelayCommand<object>(ExecuteDragCommand));
            }
        }

        private void ExecuteDragCommand(object data)
        {
        }

        #endregion DragCommand

        private AbstractRobot ParseZip(FileInfo file)
        {
            var zip = new RobotZipFile(file.FullName);
            return ParseZip(zip);
        }


        private AbstractRobot ParseZip(FileInfo file,string temp)
        {
            var zip = new RobotZipFile(file.FullName);
            var name = Path.GetFileNameWithoutExtension(file.FullName);
            var dir = new DirectoryInfo(Path.Combine(temp, name));
            if (!dir.Exists)
                dir.Create();

            return ParseZip(zip);

        }
        object o = new object();
        private AbstractRobot ParseZip(RobotZipFile zip)
        {

         var robot = GetRobotType(zip);
         if (robot == null)
             throw new NotImplementedException("Could not determine robot type");
         robot.Zip = zip;
            robot.Name = Path.GetFileNameWithoutExtension(zip.Name);
            robot.GetSystems(zip);
            robot.ParseRobot(zip);

            return robot;

        }

        /// <summary>
        /// Unregisters this instance from the Messenger class.
        /// <para>To cleanup additional resources, override this method, clean
        /// up and then call base.Cleanup().</para>
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
        }
    }

 
}