using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace MagicianHub.ViewModels
{
    public class TwoFactorAuthModeDialogViewModel : ViewModelBase
    {
        public TwoFactorAuthModeDialogViewModel()
        {
            ContinueCommand = new RelayCommand(Continue);
            CancelCommand = new RelayCommand(Cancel);
            VerifyMethods = new ObservableCollection<string>
            {
                "via authentication app",
                "via phone number"
            };
        }

        public ICommand ContinueCommand { get; }
        private void Continue()
        {
            var locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            if (locator == null) return;
            Authorization.Authorization.SendVerificationCode();
        }

        public ICommand CancelCommand { get; }
        private void Cancel()
        {
            var locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            if (locator == null) return;
            locator.LoginPageInstance.IsInValidation = false;
            locator.LoginPageInstance.AccessToken = "";
        }

        private ObservableCollection<string> _verifyMethods;
        public ObservableCollection<string> VerifyMethods
        {
            get => _verifyMethods;
            set
            {
                if (value == _verifyMethods) return;
                _verifyMethods = value;
                RaisePropertyChanged(nameof(VerifyMethods));
            }
        }
    }
}
