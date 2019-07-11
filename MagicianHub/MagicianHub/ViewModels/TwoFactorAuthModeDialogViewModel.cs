using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MagicianHub.Verification;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            if (SelectedVerifyMode == 0)
            {
                locator.LoginPageInstance.VerificationRequestType =
                    VerificationRequestTypes.Application;
                Authorization.Authorization.SendVerificationCode(
                    VerificationRequestTypes.Application
                );
            }
            else
            {
                locator.LoginPageInstance.VerificationRequestType =
                    VerificationRequestTypes.Phone;
                Authorization.Authorization.SendVerificationCode(
                    VerificationRequestTypes.Phone
                );
            }
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

        private int _selectedVerifyMode;
        public int SelectedVerifyMode
        {
            get => _selectedVerifyMode;
            set
            {
                if (value == _selectedVerifyMode) return;
                _selectedVerifyMode = value;
                RaisePropertyChanged(nameof(SelectedVerifyMode));
            }
        }
    }
}
