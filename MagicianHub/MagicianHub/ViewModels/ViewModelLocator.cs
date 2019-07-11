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
            SimpleIoc.Default.Register<TwoFactorAuthModeDialogViewModel>();
        }

        public LoginPageViewModel LoginPageInstance => 
            ServiceLocator.Current.GetInstance<LoginPageViewModel>();
        
        public TwoFactorAuthModeDialogViewModel TwoFactorAuthModeDialogViewModel =>
            ServiceLocator.Current.GetInstance<TwoFactorAuthModeDialogViewModel>();
    }
}
