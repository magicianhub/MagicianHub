using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace MagicianHub.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<LoginPageViewModel>();
        }

        public LoginPageViewModel LoginPageInstance => 
            ServiceLocator.Current.GetInstance<LoginPageViewModel>();
    }
}
