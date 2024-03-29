﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MagicianHub.Authorization;
using MagicianHub.DataTypes;
using MagicianHub.Extensions;
using MagicianHub.Secure;
using MagicianHub.Settings;
using MagicianHub.Verification;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.System;
using static MagicianHub.Logger.Logger;

namespace MagicianHub.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public static LoginPageViewModel Instance;
        public LoginPageViewModel()
        {
            Instance = this;
            AuthorizationCommand = new RelayCommand(DoAuthorization);
            VerificationCommand = new RelayCommand(DoVerification);
            AuthenticateViaBrowserCommand = new RelayCommand(DoAuthorizationViaBrowser);
            RemoveSavedAccountCommand = new RelayCommand(RemoveSavedAccount);
            LoginViaSavedAccountCommand = new RelayCommand(LoginViaSavedAccount);
            RestorePasswordCommand = new RelayCommand(RestorePassword);
            AbortOperationCommand = new RelayCommand(AbortOperation);
            VerificationRequestType = VerificationRequestTypes.Application;
            if (SettingsManager.GetSettings().Auth.LoadSavedAccounts)
            {
                SavedAccounts = Models.SavedAccounts.GetSavedAccounts();
                SavedAccountsExists = SavedAccounts.Count != 0;
            }
            SelectedSavedAccountIndex = -1;
            if (SettingsManager.GetSettings().Auth.EnableAutoLogin)
            {
                AutoLoginWithSavedAccount();
            }
        }

        private void AutoLoginWithSavedAccount()
        {
            int targetAccountIndex = -1;
            for (int i = 0; i < SettingsManager.GetSettings().Auth.SavedAccounts.Length; i++)
            {
                if (SettingsManager.GetSettings().Auth.SavedAccounts[i].Nickname ==
                    SettingsManager.GetSettings().Auth.AutoLogInAccountByNickname
                )
                {
                    targetAccountIndex = i;
                }
            }
            if (!SavedAccountsExists) return;
            if (targetAccountIndex == -1) return;
            SelectedSavedAccountIndex = targetAccountIndex;
            LoginViaSavedAccountCommand.Execute(null);
        }

        private bool ValidateCredentials()
        {
            if (!UseAccessToken && string.IsNullOrWhiteSpace(Login)) return false;
            if (UseAccessToken && string.IsNullOrWhiteSpace(AccessToken)) return false;
            if (!UseAccessToken && string.IsNullOrWhiteSpace(Password)) return false;
            return true;
        }

        private void ThrowAuthFailedInAppNotify(
            bool isVerifyFailed,
            bool isTokenFailed = false,
            bool isUnexpectedResponse = false
        )
        {
            // This technological fasteners allows you to change
            // InAppAuthNotifyIsOpened to true again.
            if (InAppAuthNotifyIsOpened) InAppAuthNotifyIsOpened = false;

            if (isVerifyFailed)
            {
                InAppAuthNotifyText =
                    ResourceLoader.GetForCurrentView().GetString("Incorrect2FACode");
            }
            else if (isTokenFailed)
            {
                InAppAuthNotifyText =
                    ResourceLoader.GetForCurrentView().GetString("IncorrectToken");
            }
            else if (isUnexpectedResponse)
            {
                InAppAuthNotifyText =
                    ResourceLoader.GetForCurrentView().GetString("UnexpectedResponse");
            }
            else
            {
                InAppAuthNotifyText =
                    ResourceLoader.GetForCurrentView().GetString("IncorrectPassword");
            }

            InAppAuthNotifyIsOpened = true;
        }
        
        public ICommand AbortOperationCommand { get; set; }
        private void AbortOperation() => IsInLoginIn = false;

        public ICommand RestorePasswordCommand { get; set; }
        private async void RestorePassword()
        {
            var restorePasswordUrl = new Uri("https://github.com/password_reset");
            var result = await Launcher.LaunchUriAsync(restorePasswordUrl);
            if (!result)
            {
                Log.Error(
                    $"An error occurred while opening restore password uri ({restorePasswordUrl})"
                );
            }
        }

        public ICommand RemoveSavedAccountCommand { get; set; }
        private void RemoveSavedAccount()
        {
            var selectedAccountNickName = SettingsManager.GetSettings()
                .Auth.SavedAccounts[SelectedSavedAccountIndex].Nickname;

            Storage.RemoveSecuredCreds(selectedAccountNickName);

            SettingsManager.GetSettings().Auth.SavedAccounts =
                SettingsManager.GetSettings().Auth.SavedAccounts
                    .RemoveByIndex(SelectedSavedAccountIndex);
            SavedAccounts.RemoveAt(SelectedSavedAccountIndex);
            SavedAccountsExists = SavedAccounts.Count != 0;
        }

        public ICommand LoginViaSavedAccountCommand { get; set; }
        private void LoginViaSavedAccount()
        {
            var selectedAccountNickName = SettingsManager.GetSettings()
                .Auth.SavedAccounts[SelectedSavedAccountIndex].Nickname;
            var creds = Storage.GetSecuredCreds(selectedAccountNickName);

            Login = selectedAccountNickName;
            UseAccessToken = Storage.IsToken(creds.Password);
            if (UseAccessToken)
            {
                AccessToken = Storage.PasswordFromRawPassword(creds.Password);
            }
            else
            {
                Password = Storage.PasswordFromRawPassword(creds.Password);
            }
            AuthorizationCommand.Execute(null);
        }

        public ICommand AuthorizationCommand { get; }
        private void DoAuthorization()
        {
            if (!ValidateCredentials()) return;

            IsInLoginIn = true;
            Authorization.Authorization.DoAuthorizationAsync(
                Login,
                Password,
                AccessToken,
                UseAccessToken
            ).ContinueWith(task =>
            {
                var responseResult = task.Result;
                switch (responseResult)
                {
                    case AuthorizationResponseTypes.Success:
                        AuthorizationRequestDelay.DelayMsModifier = 0;
                        Storage.AddSecuredCreds(Login, UseAccessToken
                            ? $"{AccessToken}&token=true"
                            : Password
                        );
                        IsWrongPassword = false;
                        IsInLoginIn = false;
                        break;
                    case AuthorizationResponseTypes.NeedVerifyCodeByApp:
                        VerificationRequestType = VerificationRequestTypes.Application;
                        IsInLoginIn = false;
                        SavedAccountsExists = false;
                        SelectedSavedAccountIndex = -1;
                        IsInValidation = true;
                        IsWrongPassword = false;
                        break;
                    case AuthorizationResponseTypes.NeedVerifyCodeByPhone:
                        VerificationRequestType = VerificationRequestTypes.Phone;
                        IsInLoginIn = false;
                        SavedAccountsExists = false;
                        SelectedSavedAccountIndex = -1;
                        IsInValidation = true;
                        IsWrongPassword = false;
                        break;
                    case AuthorizationResponseTypes.WrongCredentials:
                        AuthorizationRequestDelay.RecalculateRequests();
                        IsInLoginIn = false;
                        AuthorizationNotify.NotifyWrongPassword(Login, Password);
                        Password = string.Empty;
                        IsWrongPassword = true;
                        ThrowAuthFailedInAppNotify(false);
                        break;
                    case AuthorizationResponseTypes.WrongAccessToken:
                        AuthorizationRequestDelay.RecalculateRequests();
                        IsInLoginIn = false;
                        AuthorizationNotify.NotifyWrongPassword(
                            login: Login,
                            token: AccessToken,
                            isUseToken: UseAccessToken
                        );
                        AccessToken = string.Empty;
                        IsWrongPassword = true;
                        ThrowAuthFailedInAppNotify(false, true);
                        break;
                    case AuthorizationResponseTypes.UnexpectedResponse:
                        AuthorizationRequestDelay.RecalculateRequests();
                        ThrowAuthFailedInAppNotify(
                            false,
                            false,
                            true
                        );
                        IsWrongPassword = true;
                        break;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public ICommand VerificationCommand { get; }
        private void DoVerification()
        {
            if (string.IsNullOrWhiteSpace(VerifyCode)) return;

            IsInLoginIn = true;
            Verification.Verification.DoVerificationAsync(
                VerifyCode
            ).ContinueWith(task =>
            {
                var (response, token) = task.Result;
                switch (response)
                {
                    case VerificationResponseTypes.Success:
                        VerificationRequestDelay.DelayMsModifier = 0;
                        UseAccessToken = true;
                        AccessToken = token;
                        DoAuthorization();
                        break;
                    case VerificationResponseTypes.WrongVerifyCode:
                        IsInLoginIn = false;
                        VerificationRequestDelay.RecalculateRequests();
                        ThrowAuthFailedInAppNotify(true);
                        VerificationNotify.NotifyWrongVerifyCode();
                        break;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public ICommand AuthenticateViaBrowserCommand { get; }
        private async void DoAuthorizationViaBrowser()
        {
            Login = string.Empty;
            IsInLoginIn = true;
            await Authorization.Authorization.DoAuthorizationViaBrowserAsync();
        }

        public async Task<string> GetTokenFromBrowserUri(string code)
        {
            var token = await Authorization.Authorization.ProcessBrowserCodeResponseAsync(code);
            return token;
        }

        private bool _isWrongPassword;
        public bool IsWrongPassword
        {
            get => _isWrongPassword;
            set
            {
                if (value == _isWrongPassword) return;
                _isWrongPassword = value;
                RaisePropertyChanged(nameof(IsWrongPassword));
            }
        }

        private bool _isInValidation;
        public bool IsInValidation
        {
            get => _isInValidation;
            set
            {
                if (value == _isInValidation) return;
                _isInValidation = value;
                RaisePropertyChanged(nameof(IsInValidation));
            }
        }

        private bool _isInLoginIn;
        public bool IsInLoginIn
        {
            get => _isInLoginIn;
            set
            {
                if (value == _isInLoginIn) return;
                _isInLoginIn = value;
                RaisePropertyChanged(nameof(IsInLoginIn));
            }
        }

        private bool _useAccessToken;
        public bool UseAccessToken
        {
            get => _useAccessToken;
            set
            {
                if (value == _useAccessToken) return;
                _useAccessToken = value;
                RaisePropertyChanged(nameof(UseAccessToken));
            }
        }

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                if (value == _login) return;
                _login = value;
                RaisePropertyChanged(nameof(Login));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        private string _accessToken;
        public string AccessToken
        {
            get => _accessToken;
            set
            {
                if (value == _accessToken) return;
                _accessToken = value;
                RaisePropertyChanged(nameof(AccessToken));
            }
        }

        private string _verifyCode;
        public string VerifyCode
        {
            get => _verifyCode;
            set
            {
                if (value == _verifyCode) return;
                _verifyCode = value;
                RaisePropertyChanged(nameof(VerifyCode));
            }
        }

        private VerificationRequestTypes _verificationRequestType;
        public VerificationRequestTypes VerificationRequestType
        {
            get => _verificationRequestType;
            set
            {
                if (value == _verificationRequestType) return;
                _verificationRequestType = value;
                RaisePropertyChanged(nameof(VerificationRequestType));
            }
        }

        private bool _inAppAuthNotifyIsOpened;
        public bool InAppAuthNotifyIsOpened
        {
            get => _inAppAuthNotifyIsOpened;
            set
            {
                if (value == _inAppAuthNotifyIsOpened) return;
                _inAppAuthNotifyIsOpened = value;
                RaisePropertyChanged(nameof(InAppAuthNotifyIsOpened));
            }
        }

        private string _inAppAuthNotifyText;
        public string InAppAuthNotifyText
        {
            get => _inAppAuthNotifyText;
            set
            {
                if (value == _inAppAuthNotifyText) return;
                _inAppAuthNotifyText = value;
                RaisePropertyChanged(nameof(InAppAuthNotifyText));
            }
        }

        private ObservableCollection<SavedAccounts> _savedAccounts;
        public ObservableCollection<SavedAccounts> SavedAccounts
        {
            get => _savedAccounts;
            set
            {
                if (value == _savedAccounts) return;
                _savedAccounts = value;
                RaisePropertyChanged(nameof(SavedAccounts));
            }
        }

        private bool _savedAccountsExists;
        public bool SavedAccountsExists
        {
            get => _savedAccountsExists;
            set
            {
                if (value == _savedAccountsExists) return;
                _savedAccountsExists = value;
                RaisePropertyChanged(nameof(SavedAccountsExists));
            }
        }

        private int _selectedSavedAccountIndex;
        public int SelectedSavedAccountIndex
        {
            get => _selectedSavedAccountIndex;
            set
            {
                if (value == _selectedSavedAccountIndex) return;
                _selectedSavedAccountIndex = value;
                RaisePropertyChanged(nameof(SelectedSavedAccountIndex));
                string loginViaLocalizedString = 
                    ResourceLoader.GetForCurrentView().GetString("LogInViaSavedAccount");
                if (value < 0) return;
                LoginViaAccountText = $"{loginViaLocalizedString} {SavedAccounts[value].Nickname}";
            }
        }

        private string _loginViaAccountText;
        public string LoginViaAccountText
        {
            get => _loginViaAccountText;
            set
            {
                if (value == _loginViaAccountText) return;
                _loginViaAccountText = value;
                RaisePropertyChanged(nameof(LoginViaAccountText));
            }
        }
    }
}
