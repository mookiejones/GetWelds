/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:GetWelds"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GetWelds.Design;
using Microsoft.Practices.ServiceLocation;

namespace GetWelds.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

           if (ViewModelBase.IsInDesignModeStatic)
           {
               // Create design time view services and models
               SimpleIoc.Default.Register<IDataService, DesignDataService>();
           }
           else
           {
               // Create run time view services and models
               SimpleIoc.Default.Register<IDataService, DataService>();
           }
            SimpleIoc.Default.Register<ParseDirectoriesViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            var model = OptionsViewModel.Deserialize();
            
            SimpleIoc.Default.Register<OptionsViewModel>();

        }
        [SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }


        [SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public ParseDirectoriesViewModel Directories
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ParseDirectoriesViewModel>();
            }
        }
        [SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public OptionsViewModel Options
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OptionsViewModel>();
            }
        }

        public static void Cleanup()
        {



            // TODO Clear the ViewModels
        }
    }
}