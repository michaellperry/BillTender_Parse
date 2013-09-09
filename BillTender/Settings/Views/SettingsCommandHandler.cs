using System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace BillTender.Settings.Views
{
    public class SettingsCommandHandler<TSettingsFlyout>
        where TSettingsFlyout : FrameworkElement, new()
    {
        private readonly Func<TSettingsFlyout, ButtonBase> _backButton;
        private readonly SettingsCommand _settingsCommand;

        private Popup _settingsPopup;

        public SettingsCommandHandler(string settingsCommandId, string label, Func<TSettingsFlyout, ButtonBase> backButton)
        {
            _backButton = backButton;

            _settingsCommand = new SettingsCommand(settingsCommandId, label, DisplayAccountSettings);
        }

        public SettingsCommand SettingsCommand
        {
            get { return _settingsCommand; }
        }

        private void DisplayAccountSettings(IUICommand command)
        {
            Frame frame = Window.Current.Content as Frame;
            double actualHeight = frame.ActualHeight;
            double actualWidth = frame.ActualWidth;

            var settingsFlyout = new TSettingsFlyout();
            settingsFlyout.Height = actualHeight;
            _backButton(settingsFlyout).Click += SettingsFlyout_BackClick;

            _settingsPopup = new Popup();
            _settingsPopup.Child = settingsFlyout;

            _settingsPopup.Closed += SettingsPopup_Closed;
            Window.Current.Activated += Current_Activated;
            _settingsPopup.IsLightDismissEnabled = true;

            _settingsPopup.SetValue(Canvas.LeftProperty,
                SettingsPane.Edge == SettingsEdgeLocation.Right
                    ? (actualWidth - settingsFlyout.Width)
                    : 0);
            _settingsPopup.SetValue(Canvas.TopProperty, 0);
            _settingsPopup.Height = actualHeight;
            _settingsPopup.Width = settingsFlyout.Width;

            _settingsPopup.ChildTransitions = new TransitionCollection();
            _settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right)
                    ? EdgeTransitionLocation.Right
                    : EdgeTransitionLocation.Left
            });

            _settingsPopup.IsOpen = true;
        }

        void SettingsFlyout_BackClick(object sender, RoutedEventArgs e)
        {
            _settingsPopup.IsOpen = false;

            // If the app is not snapped, then the back button shows the Settings pane again.
            if (ApplicationView.Value != ApplicationViewState.Snapped)
            {
                SettingsPane.Show();
            }
        }

        void SettingsPopup_Closed(object sender, object e)
        {
            Window.Current.Activated -= Current_Activated;
            _settingsPopup.Closed -= SettingsPopup_Closed;
            _settingsPopup = null;
        }

        void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                _settingsPopup.IsOpen = false;
            }
        }
    }
}
