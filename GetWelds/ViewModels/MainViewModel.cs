using System.Collections.Generic;
using System.Xml;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GetWelds.Robots;
using System.Xml.Serialization;
using GetWelds.Converters;
using System.Linq;
using GetWelds.Model;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
namespace GetWelds.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {


        #region Version
        /// <summary>
        /// The <see cref="Version" /> property's name.
        /// </summary>
        public const string VersionPropertyName = "Version";

        private string _version = string.Empty;

        /// <summary>
        /// Sets and gets the Version property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Version
        {
            get
            {
                return _version;
            }

            set
            {
                if (_version == value)
                {
                    return;
                }

                RaisePropertyChanging(VersionPropertyName);
                _version = value;
                RaisePropertyChanged(VersionPropertyName);
            }
        }
        #endregion
        
        
        #region · Fields ·
        #endregion



        
        #region · Properties ·

        

        #region ShowOptions
        /// <summary>
        /// The <see cref="ShowOptions" /> property's name.
        /// </summary>
        public const string ShowOptionsPropertyName = "ShowOptions";

        private bool _showOptions = false;

        /// <summary>
        /// Sets and gets the ShowOptions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool ShowOptions
        {
            get
            {
                return _showOptions;
            }

            set
            {
                if (_showOptions == value)
                {
                    return;
                }

                RaisePropertyChanging(ShowOptionsPropertyName);
                _showOptions = value;
                RaisePropertyChanged(ShowOptionsPropertyName);
            }
        }
        #endregion
        


        #endregion



        
        #region · ShowOptionsCommand ·

        private RelayCommand _openOptionsCommand;

        /// <summary>
        /// Gets the OpenOptionsCommand.
        /// </summary>
        public RelayCommand OpenOptionsCommand
        {
            get
            {
                return _openOptionsCommand
                    ?? (_openOptionsCommand = new RelayCommand(ExecuteOpenOptionsCommand));
            }
        }

        private void ExecuteOpenOptionsCommand()
        {
            ShowOptions = !ShowOptions;
        }
        #endregion
    
            

        #region OpenWeldsWizardCommand

        private RelayCommand _openWeldsWizardCommand;
        /// <summary>
        /// Gets the OpenWeldsWizard.
        /// </summary>
        public RelayCommand OpenWeldsWizardCommand
        {
            get
            {
                return _openWeldsWizardCommand
                    ?? (_openWeldsWizardCommand = new RelayCommand(ExecuteOpenWeldsWizard));
            }
        }

        private void ExecuteOpenWeldsWizard()
        {
            
        }
        #endregion
        

        #region SelectedRobotType
        /// <summary>
        /// The <see cref="SelectedRobotType" /> property's name.
        /// </summary>
        public const string SelectedRobotTypePropertyName = "SelectedRobotType";

        private string _selectedRobotType = string.Empty;

        /// <summary>
        /// Sets and gets the SelectedRobotType property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedRobotType
        {
            get
            {
                return _selectedRobotType;
            }

            set
            {
                if (_selectedRobotType == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedRobotTypePropertyName);
                _selectedRobotType = value;
                RaisePropertyChanged(SelectedRobotTypePropertyName);
            }
        }
        #endregion
        

        #region RobotTypes
        /// <summary>
        /// The <see cref="RobotTypeCollection" /> property's name.
        /// </summary>
        public const string RobotTypesPropertyName = "RobotTypes";

        private ICollection<string> _robotTypeCollection = new List<string> { "KUKA","Fanuc" };

        /// <summary>
        /// Sets and gets the RobotTypes property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ICollection<string> RobotTypeCollection
        {
            get
            {
                return _robotTypeCollection;
            }

            set
            {
                if (_robotTypeCollection == value)
                {
                    return;
                }

                RaisePropertyChanging(RobotTypesPropertyName);
                _robotTypeCollection = value;
                RaisePropertyChanged(RobotTypesPropertyName);
            }
        }
        #endregion


   //  private RelayCommand<DragEventArgs> _dropCommand;
   //
   //  /// <summary>
   //  /// Gets the DropCommand.
   //  /// </summary>
   //  public RelayCommand<DragEventArgs> DropCommand
   //  {
   //      get; private set;
   //
   //  }
   //
   //  private void ExecuteDropCommand(DragEventArgs parameter)
   //  {
   //      
   //  }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
           if (IsInDesignMode)
           {
               // Code runs in Blend --> create design time data.
           }
           else
           {
               Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

               Messenger.Default.Register<Message>(this,MessageRecieved);
           }

        }
            private void MessageRecieved(Message msg)
            {
              
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

    public class Message
    {
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        SearchStart,
        SearchEnd,
    }


}