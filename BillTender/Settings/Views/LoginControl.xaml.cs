using System;
using BillTender.Settings.ViewModels;
using UpdateControls.XAML;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillTender.Settings.Views
{
    public sealed partial class LogInControl : UserControl
    {
        public LogInControl()
        {
            this.InitializeComponent();
        }

        private async void LogIn_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ForView.Unwrap<LogInViewModel>(
                DataContext);
            if (viewModel == null)
                return;

            try
            {
                viewModel.LastError = string.Empty;
                viewModel.Busy = true;

                // TODO: Log in here.

                UserNameTextBox.Text = string.Empty;
                PasswordTextBox.Password = string.Empty;

                viewModel.LoggedIn();
            }
            catch (Exception x)
            {
                viewModel.LastError = x.Message;
            }
            finally
            {
                viewModel.Busy = false;
            }
        }
    }
}
