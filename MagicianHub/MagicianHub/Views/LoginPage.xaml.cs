using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace MagicianHub.Views
{
    public sealed partial class LoginPage
    {
        private static readonly Type TypeOfThis = typeof(LoginPage);
        private static CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

        public LoginPage()
        {
            InitializeComponent();
            Task.Run(Test);
        }

        private void Test()
        {
            Thread.Sleep(3000);
            _ = dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                Authorization.AuthorizationNotify.NotifyWrongPassword
            );
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
