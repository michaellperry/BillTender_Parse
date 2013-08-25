﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Parse;
using BillTender.ViewModels;
using UpdateControls.XAML;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BillTender.Views
{
    public sealed partial class SignUpControl : UserControl
    {
        public SignUpControl()
        {
            this.InitializeComponent();
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ForView.Unwrap<SignUpViewModel>(DataContext);
            if (viewModel == null)
                return;

            if (PasswordTextBox.Password != ConfirmPasswordTextBox.Password)
            {
                return;
            }

            try
            {
                viewModel.LastError = string.Empty;
                viewModel.Busy = true;

                var user = new ParseUser
                {
                    Username = UserNameTextBox.Text,
                    Email = EmailTextBox.Text,
                    Password = PasswordTextBox.Password
                };

                await user.SignUpAsync();

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
