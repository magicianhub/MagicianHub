using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GraphQL;
using MagicianHub.Authorization;

namespace MagicianHub.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel()
        {
            AuthorizationCommand = new RelayCommand(DoAuthorization);
        }

        public ICommand AuthorizationCommand { get; }
        private void DoAuthorization()
        {
            if (Login.IsEmpty()) return;

            if (UseAccessToken)
            {
                if (AccessToken.IsEmpty()) return;
            }
            else
            {
                if (Password.IsEmpty()) return;
            }

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
                    case AuthorizationResponseTypes.NeedVerifyCode:
                        IsInLoginIn = false;
                        IsInValidation = true;
                        break;
                    case AuthorizationResponseTypes.WrongCredentials:
                        IsInLoginIn = false;
                        break;
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
    }
}
