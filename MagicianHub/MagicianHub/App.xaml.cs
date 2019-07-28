using MagicianHub.Extensions;
using MagicianHub.Github;
using MagicianHub.Notifications;
using MagicianHub.Protocol;
using MagicianHub.Settings;
using MagicianHub.Views;
using Microsoft.QueryStringDotNET;
using Octokit;
using System;
using System.Diagnostics;
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
            SettingsManager.LoadSettings();
            Logger.Logger.Init();
            Logger.Logger.Log.Info("Logger initialized");
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
            Logger.Logger.Log.Info("Extending view into title-bar done");
        }

        private void OnLaunchedOrActivated(
            LaunchActivatedEventArgs launchActivatedEventArgs,
            IActivatedEventArgs activatedEventArgs,
            bool isActivated)
        {
            ExtendAcrylicIntoTitleBar();
            // todo: REMOVE IT! IT ONLY FOR LANGUAGE TEST!
            ApplicationLanguages.PrimaryLanguageOverride = "en-US";
            Logger.Logger.Log.Info($"Starting with {ApplicationLanguages.PrimaryLanguageOverride} language");
#if DEBUG
            if (Debugger.IsAttached)
            {
                Logger.Logger.Log.Debug($"Debugger attached");
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Logger.Logger.Log.Info("Creating base application frame ...");
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (!isActivated && launchActivatedEventArgs.PreviousExecutionState ==
                    ApplicationExecutionState.Terminated
                )
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

            Logger.Logger.Log.Debug("Initializing core window extension ...");
            Window.Current.CoreWindow.InitExtension();
            Logger.Logger.Log.Debug("Initializing core window extension done");

            if (activatedEventArgs is
                ToastNotificationActivatedEventArgs toastActivationArgs
            )
            {
                QueryString args = QueryString.Parse(toastActivationArgs.Argument);
                Logger.Logger.Log.Debug($"Application launched \\ activated via notify ({args})");
                Logger.Logger.Log.Debug("Processing notify query ...");
                NotificationBaseHandler.ProcessNotify(args);
                Logger.Logger.Log.Debug("Processing notify query done");
                return;
            }

            Window.Current.Activate();
            Logger.Logger.Log.Info("Creating base application frame done");

            if (activatedEventArgs?.Kind == ActivationKind.Protocol)
            {
                var eventArgs = activatedEventArgs as ProtocolActivatedEventArgs;
                Logger.Logger.Log.Debug($"Application launched \\ activated via protocol ({eventArgs?.Uri})");
                Logger.Logger.Log.Debug("Processing protocol query ...");
                ProtocolBaseHandler.ProcessQuery(eventArgs);
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            Logger.Logger.Log.Debug("Shutting down application ...");
            var deferral = e.SuspendingOperation.GetDeferral();
            SettingsManager.SaveSettings();
            Logger.Logger.Log.Debug("// Thanks for using, goodbye");
            deferral.Complete();
        }
    }
}