using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MagicianHub.Verification;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;

namespace MagicianHub.ViewModels
{
    public class TwoFactorAuthModeDialogViewModel : ViewModelBase
    {
        private readonly LoginPageViewModel _loginPageViewModel;
        public TwoFactorAuthModeDialogViewModel(LoginPageViewModel loginPageViewModel)
        {
            _loginPageViewModel = loginPageViewModel;
            ContinueCommand = new RelayCommand(Continue);
            CancelCommand = new RelayCommand(Cancel);
            VerifyMethods = new ObservableCollection<string>
            {
                ResourceLoader.GetForCurrentView().GetString("ViaAuthApp"),
                ResourceLoader.GetForCurrentView().GetString("ViaPhone")
            };
        }

        public ICommand ContinueCommand { get; }
        private void Continue()
        {
            if (SelectedVerifyMode == 0)
            {
                _loginPageViewModel.VerificationRequestType =
                    VerificationRequestTypes.Application;
                Authorization.Authorization.SendVerificationCode(
                    VerificationRequestTypes.Application
                );
            }
            else
            {
                _loginPageViewModel.VerificationRequestType =
                    VerificationRequestTypes.Phone;
                Authorization.Authorization.SendVerificationCode(
                    VerificationRequestTypes.Phone
                );
            }
        }

        public ICommand CancelCommand { get; }
        private void Cancel()
        {
            _loginPageViewModel.IsInValidation = false;
            _loginPageViewModel.AccessToken = string.Empty;
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
