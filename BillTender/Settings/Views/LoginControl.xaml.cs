using System;
using Parse;
using BillTender.Settings.ViewModels;
using UpdateControls.XAML;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BillTender.Settings.Views
{
    public sealed partial class LogInControl : UserControl
    {
        public LogInControl()
        {
            this.InitializeComponent();
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ForView.Unwrap<LogInViewModel>(DataContext);
            if (viewModel == null)
                return;

            try
            {
                viewModel.LastError = string.Empty;
                viewModel.Busy = true;

                await ParseUser.LogInAsync(UserNameTextBox.Text, PasswordTextBox.Password);

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
