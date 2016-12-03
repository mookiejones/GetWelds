using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace GetWelds.ViewModels.WizardPages
{
    public class WizardPage1:ViewModelBase
    {
        
        #region · Properties ·
        

        #region RobotTypes
        /// <summary>
        /// The <see cref="RobotTypeCollection" /> property's name.
        /// </summary>
        public const string RobotTypesPropertyName = "RobotTypes";

        private ICollection<string> _robotTypeCollection = new List<string>{"KUKA,Fanuc"};        

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
        
        #endregion
    
            


    }
}
