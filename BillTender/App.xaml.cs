using System;
using BillTender.Settings.Views;
using BillTender.ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillTender
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            ParseConfig.Initialize();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (!rootFrame.Navigate(typeof(BillTender.Budget.Views.BudgetPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();

            SettingsPane.GetForCurrentView()
                .CommandsRequested += App_CommandsRequested;
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var locator = Resources["Locator"] as ViewModelLocator;
            if (locator == null)
                return;

            var account = new SettingsCommandHandler
                <AccountFlyout>(
                "AccountSettings",
                "Account",
                f => f.Back);
            var preferences = new SettingsCommandHandler
                <PreferencesFlyout>(
                "PreferencesSettings",
                "Preferences",
                f => f.Back);

            args.Request.ApplicationCommands.Add(
                account.SettingsCommand);
            if (locator.CanSetPreferences)
                args.Request.ApplicationCommands.Add(
                    preferences.SettingsCommand);
        }
    }
}
