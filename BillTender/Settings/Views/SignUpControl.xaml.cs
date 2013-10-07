using System;
using BillTender.Settings.ViewModels;
using UpdateControls.XAML;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillTender.Settings.Views
{
    public sealed partial class SignUpControl : UserControl
    {
        public SignUpControl()
        {
            this.InitializeComponent();
        }

        private async void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ForView.Unwrap<SignUpViewModel>(
                DataContext);
            if (viewModel == null)
                return;

            if (PasswordTextBox.Password !=
                ConfirmPasswordTextBox.Password)
            {
                viewModel.LastError =
                    "The passwords do not match";
                return;
            }

            try
            {
                viewModel.LastError = string.Empty;
                viewModel.Busy = true;

                // TODO: Sign up here.

                UserNameTextBox.Text = string.Empty;
                EmailTextBox.Text = string.Empty;
                PasswordTextBox.Password = string.Empty;
                ConfirmPasswordTextBox.Password = string.Empty;

                viewModel.SignedUp();
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
