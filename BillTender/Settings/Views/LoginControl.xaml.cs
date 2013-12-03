using System;
using BillTender.Settings.ViewModels;
using Parse;
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

                var user = await ParseUser.LogInAsync(
                    UserNameTextBox.Text,
                    PasswordTextBox.Password);

                ParseInstallation.CurrentInstallation["user"] = user;
                await ParseInstallation.CurrentInstallation.SaveAsync();

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
