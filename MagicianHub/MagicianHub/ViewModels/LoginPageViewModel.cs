using GalaSoft.MvvmLight;

namespace MagicianHub.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel()
        {
            IsWrongPassword = false;
            _isInValidation = false;
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
    }
}
