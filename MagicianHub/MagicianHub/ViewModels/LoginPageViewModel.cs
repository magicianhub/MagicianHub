using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GraphQL;
using MagicianHub.Authorization;
using MagicianHub.Verification;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MagicianHub.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel()
        {
            AuthorizationCommand = new RelayCommand(DoAuthorization);
            VerificationCommand = new RelayCommand(DoVerification);
            VerificationRequestType = VerificationRequestTypes.Application;
            UseAccessToken = false;
        }

        public ICommand AuthorizationCommand { get; }
        private void DoAuthorization()
        {
            if (Login.IsEmpty()) return;
            if (UseAccessToken && AccessToken.IsEmpty()) return;
            if (!UseAccessToken && Password.IsEmpty()) return;

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
                        IsInLoginIn = false;
                        Debug.WriteLine(responseResult);
                        break;
                    case AuthorizationResponseTypes.NeedVerifyCodeByApp:
                        IsInLoginIn = false;
                        IsInValidation = true;
                        break;
                    case AuthorizationResponseTypes.WrongCredentials:
                        IsInLoginIn = false;
                        Login = string.Empty;
                        Password = string.Empty;
                        AuthorizationNotify.NotifyWrongPassword();
                        break;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public ICommand VerificationCommand { get; }
        private void DoVerification()
        {
            if (VerifyCode.IsEmpty()) return;
            IsInLoginIn = true;

            Authorization.Authorization.DoVerification(
                VerifyCode
            ).ContinueWith(task =>
            {
                var responseResult = task.Result;
                if (responseResult != null)
                {
                    UseAccessToken = true;
                    AccessToken = responseResult;
                    DoAuthorization();
                }

                if (responseResult == "Exception")
                {
                    Debug.WriteLine("Ex");
                }
                
            }, TaskScheduler.FromCurrentSynchronizationContext());
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
    }
}
