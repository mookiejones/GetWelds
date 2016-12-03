using GalaSoft.MvvmLight;
using GetWelds.ViewModel;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GetWelds.Model
{


    public class StyleProgramViewModel:ViewModelBase
    {
       
        
        #region · Properties ·
        /// <summary>
        /// The <see cref="StyleProgramName" /> property's name.
        /// </summary>
        public const string StyleProgramNamePropertyName = "StyleProgramName";

        private string _styleProgramName = string.Empty;

        /// <summary>
        /// Sets and gets the StyleProgramName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string StyleProgramName
        {
            get
            {
                return _styleProgramName;
            }

            set
            {
                if (_styleProgramName == value)
                {
                    return;
                }

                RaisePropertyChanging(StyleProgramNamePropertyName);
                var oldValue = _styleProgramName;
                _styleProgramName = value;
                RaisePropertyChanged(StyleProgramNamePropertyName, oldValue, value, true);
            }
        }
        #endregion
            
        
        #region · Text ·
        /// <summary>
        /// The <see cref="Text" /> property's name.
        /// </summary>
        public const string TextPropertyName = "Text";

        private string _text = string.Empty;

        /// <summary>
        /// Sets and gets the Text property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                Set(TextPropertyName, ref _text, value, true);
            }
        }
        #endregion
        
        
        #region · ZipFile ·
        /// <summary>
        /// The <see cref="ZipFile" /> property's name.
        /// </summary>
        public const string ZipFilePropertyName = "ZipFile";
        [XmlIgnore]
        private ZipFile _zipFile = null;

        /// <summary>
        /// Sets and gets the ZipFile property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        [XmlIgnore]
        public ZipFile ZipFile
        {
            get
            {
                return _zipFile;
            }
            set
            {
                Set(ZipFilePropertyName, ref _zipFile, value, true);
            }
        }
      



        #region · RobotName ·
        /// <summary>
        /// The <see cref="RobotName" /> property's name.
        /// </summary>
        public const string RobotNamePropertyName = "RobotName";

        private string _robotName = string.Empty;

        /// <summary>
        /// Sets and gets the RobotName property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string RobotName
        {
            get
            {
                return _robotName;
            }
            set
            {
                Set(RobotNamePropertyName, ref _robotName, value, true);
            }
        }
        #endregion        
        
        #region · Style ·
        /// <summary>
        /// The <see cref="Style" /> property's name.
        /// </summary>
        public const string StylePropertyName = "Style";

        private int _style = -1;

        /// <summary>
        /// Sets and gets the Style property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public int Style
        {
            get
            {
                return _style;
            }
            set
            {
                Set(StylePropertyName, ref _style, value, true);
            }
        }
        #endregion
        
        #endregion

        
        #region · Collections ·
        
        #region · Positions ·

        /// <summary>
        /// The <see cref="Positions" /> property's name.
        /// </summary>
        public const string PositionsPropertyName = "Positions";

        private List<Weld> _positions = new List<Weld>();

        /// <summary>
        /// Sets and gets the Positions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public List<Weld> Positions
        {
            get
            {
                return _positions;
            }

            set
            {
                if (_positions == value)
                {
                    return;
                }

                RaisePropertyChanging(() => Positions);
                var oldValue = _positions;
                _positions = value;
                RaisePropertyChanged(() => Positions, oldValue, value, true);
            }
        }
   
        #endregion
        
        #endregion
    
            


       
        public StyleProgramViewModel(string stylename,string filepath)
        {

        }
        public StyleProgramViewModel()
        {
           
        }


    }




}
