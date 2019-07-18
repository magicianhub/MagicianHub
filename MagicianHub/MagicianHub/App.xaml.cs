using MagicianHub.Extensions;
using MagicianHub.Github;
using MagicianHub.Notifications;
using MagicianHub.Views;
using Microsoft.QueryStringDotNET;
using Octokit;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MagicianHub
{
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            GitHubClientBase.Instance = new GitHubClient(
                new ProductHeaderValue("MagicianHub", "0.0.1")
            );
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            OnLaunchedOrActivated(e, null, false);
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            OnLaunchedOrActivated(null, e, true);
        }

        private void ExtendAcrylicIntoTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private void OnLaunchedOrActivated(
            LaunchActivatedEventArgs launchActivatedEventArgs,
            IActivatedEventArgs activatedEventArgs,
            bool isActivated)
        {
            ExtendAcrylicIntoTitleBar();
            ApplicationLanguages.PrimaryLanguageOverride = "en-US";
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (!isActivated && launchActivatedEventArgs.PreviousExecutionState ==
                    ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (!isActivated)
            {
                if (launchActivatedEventArgs.PrelaunchActivated) return;
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(
                        typeof(LoginPage),
                        launchActivatedEventArgs.Arguments
                    );
                }
            }
            else
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(LoginPage));
                }
            }

            Window.Current.CoreWindow.InitExtension();
            Window.Current.Activate();

            if (isActivated)
            {
                if (!(activatedEventArgs is
                    ToastNotificationActivatedEventArgs toastActivationArgs)) return;
                QueryString args = QueryString.Parse(toastActivationArgs.Argument);
                NotificationBaseHandler.ProcessNotify(args);
            }
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