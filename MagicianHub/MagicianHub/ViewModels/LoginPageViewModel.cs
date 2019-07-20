using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MagicianHub.Authorization;
using MagicianHub.Verification;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;

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
            VerificationRequestType = VerificationRequestTypes.Application;
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
            bool isUnexpectedResponse = false)
        {
            // This technological fasteners allows you to change InAppAuthNotifyIsOpened to true again.
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
                        IsInLoginIn = false;
                        Debug.WriteLine(responseResult);
                        break;
                    case AuthorizationResponseTypes.NeedVerifyCodeByApp:
                        VerificationRequestType = VerificationRequestTypes.Application;
                        IsInLoginIn = false;
                        IsInValidation = true;
                        break;
                    case AuthorizationResponseTypes.NeedVerifyCodeByPhone:
                        VerificationRequestType = VerificationRequestTypes.Phone;
                        IsInLoginIn = false;
                        IsInValidation = true;
                        break;
                    case AuthorizationResponseTypes.WrongCredentials:
                        AuthorizationRequestDelay.RecalculateRequests();
                        IsInLoginIn = false;
                        AuthorizationNotify.NotifyWrongPassword(Login, Password);
                        Login = string.Empty;
                        Password = string.Empty;
                        ThrowAuthFailedInAppNotify(false);
                        break;
                    case AuthorizationResponseTypes.WrongAccessToken:
                        AuthorizationRequestDelay.RecalculateRequests();
                        IsInLoginIn = false;
                        AuthorizationNotify.NotifyWrongPassword(
                            login:Login,
                            token:AccessToken,
                            isUseToken:UseAccessToken
                        );
                        Login = string.Empty;
                        AccessToken = string.Empty;
                        ThrowAuthFailedInAppNotify(false, true);
                        break;
                    case AuthorizationResponseTypes.UnexpectedResponse:
                        AuthorizationRequestDelay.RecalculateRequests();
                        ThrowAuthFailedInAppNotify(
                            false,
                            false,
                            true
                        );
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
                        UseAccessToken = true;
                        AccessToken = token;
                        DoAuthorization();
                        break;
                    case VerificationResponseTypes.WrongVerifyCode:
                        IsInLoginIn = false;
                        ThrowAuthFailedInAppNotify(true);
                        break;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public ICommand AuthenticateViaBrowserCommand { get; }
        private async void DoAuthorizationViaBrowser()
        {
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
    }
}
