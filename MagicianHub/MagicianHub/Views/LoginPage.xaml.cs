using System;
using Windows.UI.Xaml;

namespace MagicianHub.Views
{
    public sealed partial class LoginPage
    {
        private static readonly Type TypeOfThis = typeof(LoginPage);

        public LoginPage()
        {
            InitializeComponent();
        }

        public bool IsWrongPassword
        {
            get => (bool) GetValue(IsWrongPasswordProperty);
            set => SetValue(IsWrongPasswordProperty, value);
        }

        public static readonly DependencyProperty IsWrongPasswordProperty =
            DependencyProperty.Register(
                nameof(IsWrongPassword),
                typeof(bool),
                TypeOfThis,
                new PropertyMetadata(false)
            );

        public bool IsInValidation
        {
            get => (bool) GetValue(IsInValidationProperty);
            set => SetValue(IsInValidationProperty, value);
        }

        public static readonly DependencyProperty IsInValidationProperty =
            DependencyProperty.Register(
                nameof(IsInValidation),
                typeof(bool),
                TypeOfThis,
                new PropertyMetadata(false)
            );
    }
}
