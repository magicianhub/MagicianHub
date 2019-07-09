using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MagicianHub.Extensions;
using MagicianHub.Notifications;
using MagicianHub.Views;
using Microsoft.QueryStringDotNET;

namespace MagicianHub
{
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated) return;
            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(LoginPage), e.Arguments);
            }

            Window.Current.CoreWindow.InitExtension();
            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            if (!(e is ToastNotificationActivatedEventArgs toastActivationArgs)) return;
            QueryString args = QueryString.Parse(toastActivationArgs.Argument);
            NotificationBaseHandler.ProcessNotify(args);
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
