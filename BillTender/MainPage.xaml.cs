using BillTender.Settings.Views;
using BillTender.ViewModels;
using UpdateControls.XAML;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BillTender
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= MainPage_CommandsRequested;
        }

        void MainPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var viewModel = ForView.Unwrap<MainViewModel>(DataContext);
            if (viewModel == null)
                return;

            var account = new SettingsCommandHandler<AccountFlyout>(
                this,
                "AccountSettings",
                "Account",
                f => f.Back);
            var preferences = new SettingsCommandHandler<PreferencesFlyout>(
                this,
                "PreferencesSettings",
                "Preferences",
                f => f.Back);

            args.Request.ApplicationCommands.Add(account.SettingsCommand);
            if (viewModel.CanSetPreferences)
                args.Request.ApplicationCommands.Add(preferences.SettingsCommand);
        }

    }
}
