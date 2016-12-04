using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using GetWelds.Resources;
using GalaSoft.MvvmLight.Command;
using System.Xml.Serialization;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using GetWelds.Robots;
using GetWelds.Helpers;
using System.ComponentModel;
using GetWelds.Robots.OptionsClasses;
using GetWelds.Messages;
namespace GetWelds.ViewModels
{
    public class OptionsViewModel:ViewModelBase
    {
        
        #region · Fields ·
        public const string OPTIONSFILE = "c:\\getweldoptions.xml";
        #endregion
        
        #region · Properties ·

  
        #region RobotOptions
        /// <summary>
        /// The <see cref="RobotOptions" /> property's name.
        /// </summary>
        public const string ROBOT_OPTIONS_PROPERTY_NAME = "RobotOptions";

        private Options _robotOptions = new Options();

        /// <summary>
        /// Sets and gets the RobotOptions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Options RobotOptions
        {
            get
            {
                return _robotOptions;
            }

            set
            {
                if (_robotOptions == value)
                {
                    return;
                }

                RaisePropertyChanging(ROBOT_OPTIONS_PROPERTY_NAME);
                _robotOptions = value;
                RaisePropertyChanged(ROBOT_OPTIONS_PROPERTY_NAME);
            }
        }
        #endregion
        
      
   

        #region SearchParamFileName
        /// <summary>
        /// The <see cref="SearchParamFileName" /> property's name.
        /// </summary>
        public const string SEARCH_PARAM_FILE_NAME_PROPERTY_NAME = "SearchParamFileName";

        /// <summary>
        /// Default Parameter File
        /// </summary>
        private string _searchParamFilename = "Resources\\DefaultParameters.xml";

        /// <summary>
        /// Sets and gets the SearchParamFileName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SearchParamFileName
        {
            get
            {
                return _searchParamFilename;
            }

            set
            {
                if (_searchParamFilename == value)
                {
                    return;
                }

                RaisePropertyChanging(SEARCH_PARAM_FILE_NAME_PROPERTY_NAME);
                _searchParamFilename = value;
                RaisePropertyChanged(SEARCH_PARAM_FILE_NAME_PROPERTY_NAME);
            }
        }
        #endregion
        
        public static OptionsViewModel Instance { get; private set; }
        #endregion

        #region SearchExpressions
        /// <summary>
        /// The <see cref="SearchExpressions" /> property's name.
        /// </summary>
        public const string SEARCH_PARAMS_PROPERTY_NAME = "SearchExpressions";

        private ObservableCollection<SearchParam> _searchExpressions = new ObservableCollection<SearchParam>();

        /// <summary>
        /// Sets and gets the SearchParams property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<SearchParam> SearchExpressions
        {
            get
            {
                return _searchExpressions;
            }

            set
            {
                if (_searchExpressions == value)
                {
                    return;
                }

                RaisePropertyChanging(SEARCH_PARAMS_PROPERTY_NAME);
                _searchExpressions = value;
                RaisePropertyChanged(SEARCH_PARAMS_PROPERTY_NAME);
            }
        }
        #endregion
        
      
        #region · Commands ·


        #region SaveSearchParams

        private RelayCommand _saveSearchParams;
        /// <summary>
        /// Gets the SaveSearchParams.
        /// </summary>
        public RelayCommand SaveSearchParams
        {
            get
            {
                return _saveSearchParams
                    ?? (_saveSearchParams = new RelayCommand(ExecuteSaveSearchParams));
            }
        }

        private void ExecuteSaveSearchParams()
        {

            var fm = new FileMessage(LabelResources.SaveSearchParameterDialog, LabelResources.SearchParamFilter, Messages.MessageType.Save, false);
            var gfm = new GetFileMessage(fm, SaveSearchParameters);

            Messenger.Default.Send(gfm);

        }

        private void SaveSearchParameters(FileMessage msg)
        {
            if (!msg.IsValid) return;
            SerializeSearchParams(msg.FileName);

            var text = File.ReadAllText(msg.FileName);
            File.WriteAllText(SearchParamFileName,text);
        }
        #endregion

        #region LoadSearchParams

        private RelayCommand _loadSearchParams;
        /// <summary>
        /// Gets the LoadSearchParams.
        /// </summary>
        public RelayCommand LoadSearchParams
        {
            get
            {
                return _loadSearchParams
                    ?? (_loadSearchParams = new RelayCommand(ExecuteLoadSearchParams));
            }
        }


        private void GotSearchParams(FileMessage msg)
        {
            if (msg.IsValid)
            {
                DeserializeSearchParams(msg.FileName);
                Properties.Settings.Default.ParametersFile = msg.FileName;
                Properties.Settings.Default.Save();
            }


        }
        private void ExecuteLoadSearchParams()
        {

            var msg = new FileMessage(LabelResources.LoadSearchParameters,LabelResources.SearchParamFilter,Messages.MessageType.Open,false);

            var gfm = new GetFileMessage(msg, GotSearchParams);

            Messenger.Default.Send(gfm);
        }
        #endregion
        #endregion

        private void SerializeSearchParams(string filename)
        {

            SearchParamFileName = filename;
            var serial = new XmlSerializer(typeof(ObservableCollection<SearchParam>));
            using (var stream = new StreamWriter(SearchParamFileName))
            {
                serial.Serialize(stream, SearchExpressions);
            }
        }

        private void DeserializeSearchParams(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                    return;

                SearchParamFileName = filename;
                var serial = new XmlSerializer(typeof(ObservableCollection<SearchParam>));
                using (var stream = new StreamReader(SearchParamFileName))
                {
                    SearchExpressions = (ObservableCollection<SearchParam>)serial.Deserialize(stream);
                    Console.WriteLine();
                }
            }
            catch(Exception ex)
            {

                var d = new DialogMessage(ex.ToString(), null) {Caption = "Could not load xml"};
                Messenger.Default.Send(d);
            }

        }

        public OptionsViewModel()
        {

            Instance = this;

            var fi = new FileInfo(Properties.Settings.Default.ParametersFile);
            if (fi.Exists)
            {
                SearchParamFileName = fi.FullName;
            }
                DeserializeSearchParams(SearchParamFileName);
            
          

     
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

        public static OptionsViewModel Deserialize()
        {
            var file = OPTIONSFILE;
            if (!File.Exists(file))
                return null;
            var serial = new XmlSerializer(typeof(OptionsViewModel));
            using (var stream = new StreamReader(file))
            {
                return (OptionsViewModel)serial.Deserialize(stream);
            }
        }
        
        #region · Save Values to Xml ·


        private RelayCommand<ObservableCollection<AbstractRobot>> _saveToXmlCommand;

        /// <summary>
        /// Gets the SaveToXmlCommand.
        /// </summary>
        public RelayCommand<ObservableCollection<AbstractRobot>> SaveToXmlCommand
        {
            get
            {
                return _saveToXmlCommand
                    ?? (_saveToXmlCommand = new RelayCommand<ObservableCollection<AbstractRobot>>(ExecuteSaveToXmlCommand));
            }
        }


        #endregion


      

        private void ExecuteSaveToXmlCommand(ObservableCollection<AbstractRobot> robots)
        {

            var fm = new FileMessage(LabelResources.ExportFilesMessage, LabelResources.SearchParamFilter,
                Messages.MessageType.Save, false) {Robots = robots};
            var gfm = new GetFileMessage(fm, SaveToXml);

            Messenger.Default.Send(gfm);

        }

        private void SaveToXml(FileMessage msg)
        {
            Serializer.Serialize(msg.Robots, msg.FileName);
        }
}

    public class Options:INotifyPropertyChanged
    {

        public Options()
        {
            // Get path of executable.
            //TODO Make sure that debug files get transferred with app.
            var basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); 
               
           var options = Deserialize(Path.Combine(basePath,"FanucOptions.xml"),new FanucOptionsClass());
            FanucOptions=options??new FanucOptionsClass();


            //TODO Get KUKA Options

//            KukaOptions = Deserialize(Path.Combine(basePath, "KukaOptions.xml"));
 //           ABBOptions = Deserialize(Path.Combine(basePath, "ABBOptions.xml"));
  //          NachiOptions = Deserialize(Path.Combine(basePath, "NachiOptions.xml"));
   //         KawasakiOptions = Deserialize(Path.Combine(basePath, "KawasakiOptions.xml"));

            // FanucOptions = Deserialize(FanucOptions,)
        }
  

        ~Options()
        {
            SerializeOptions("FanucOptions.xml", FanucOptions);
            //SerializeOptions("KukaOptions.xml", KukaOptions);
        }


        private IOptionsClass _fanucOptions;
        private IOptionsClass _kukaOptions;
        private IOptionsClass _nachiOptions;
        private IOptionsClass _abbOptions;
        private IOptionsClass _kawasakiOptions;
        public IOptionsClass FanucOptions { get { return _fanucOptions; } set { if (_fanucOptions != value) { _fanucOptions = value; RaisePropertyChanged("FanucOptions"); } } }
        public IOptionsClass KukaOptions { get { return _kukaOptions; } set { if (_kukaOptions != value) { _kukaOptions = value; RaisePropertyChanged("KukaOptions"); } } }
        public IOptionsClass NachiOptions { get { return _nachiOptions; } set { if (_nachiOptions != value) { _nachiOptions = value; RaisePropertyChanged("NachiOptions"); } } }
        public IOptionsClass ABBOptions { get { return _abbOptions; } set { if (_abbOptions != value) { _abbOptions = value; RaisePropertyChanged("ABBOptions"); } } }
        public IOptionsClass KawasakiOptions { get { return _kawasakiOptions; } set { if (_kawasakiOptions != value) { _kawasakiOptions = value; RaisePropertyChanged("KawasakiOptions"); } } }

        private IOptionsClass Deserialize(string filename,IOptionsClass optionsClass)
        {

         
            if (!File.Exists(filename))
            {
                return null;
            }

            var serial = new XmlSerializer(optionsClass.GetType());
            using (var stream = new StreamReader(filename))
            {

                var options =  (IOptionsClass)serial.Deserialize(stream);

                if (string.IsNullOrEmpty(options.JointString))
                    options.JointString = optionsClass.JointString;

                if (string.IsNullOrEmpty(options.StudString))
                    options.StudString = optionsClass.StudString;

                if (string.IsNullOrEmpty(options.LinearString))
                    options.LinearString = optionsClass.LinearString;

                if (string.IsNullOrEmpty(options.NutWeldString))
                    options.NutWeldString = optionsClass.NutWeldString;

                if (string.IsNullOrEmpty(options.RivetString))
                    options.RivetString = optionsClass.RivetString;


                if (string.IsNullOrEmpty(options.ServoWeldString))
                    options.ServoWeldString = optionsClass.ServoWeldString;


                if (string.IsNullOrEmpty(options.StyleName))
                    options.StyleName = optionsClass.StyleName;


                return options;
            }

           
        }
        private void SerializeOptions(string filename, IOptionsClass options)
        {
            var serial = new XmlSerializer(options.GetType());
            using (var stream = new StreamWriter(filename))
            {
                serial.Serialize(stream, options);
            }


        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public  class OptionsClass:IOptionsClass
    {
        public string StyleName { get; set; }
        public string StudString { get; set; }
        public string LinearString { get; set; }
        public string JointString { get; set; }
        public string RivetString { get; set; }
        public string NutWeldString { get; set; }



        public string ServoWeldString
        {get;set;}
    }

    public interface IOptionsClass
    {
        string StyleName { get; set; }
        string StudString { get; set; }
        string LinearString { get; set; }
        string JointString { get; set; }
        string RivetString { get; set; }
        string NutWeldString { get; set; }

        string ServoWeldString { get; set; }
    }

   
 
}