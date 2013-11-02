using BillTender.Families.ViewModels;
using UpdateControls.XAML;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

namespace BillTender.Families.Views
{
    public sealed partial class FamilySelectorView : UserControl, IDataContextChangedHandler<FamilySelectorView>
    {
        private Action _familyEditedCompleted;
        private Action _familyEditedCancelled;

        public FamilySelectorView()
        {
            DataContextChangedHelper<FamilySelectorView>.Bind(this);
            this.InitializeComponent();
        }

        public void DataContextChanged(FamilySelectorView sender, DependencyPropertyChangedEventArgs e)
        {
            var oldViewModel = ForView.Unwrap<FamilySelectorViewModel>(e.OldValue);
            if (oldViewModel != null)
            {
                oldViewModel.FamilyEdited -= FamilyEdited;
            }

            var newViewModel = ForView.Unwrap<FamilySelectorViewModel>(e.NewValue);
            if (newViewModel != null)
            {
                newViewModel.Load();
                newViewModel.FamilyEdited += FamilyEdited;
            }
        }

        void FamilyEdited(object sender, FamilyEditedEventArgs args)
        {
            _familyEditedCompleted = args.Completed;
            _familyEditedCancelled = args.Cancelled;

            FamilyDialog.Title = args.NewFamily ? "New Family" : "Edit Family";
            FamilyDialog.DataContext = ForView.Wrap(args.Family);
            FamilyDialog.IsOpen = true;
        }

        private void FamilyDialog_Ok(object sender, RoutedEventArgs e)
        {
            if (_familyEditedCompleted != null)
                _familyEditedCompleted();
            FamilyDialogDismissed();
        }

        private void FamilyDialog_Cancel(object sender, RoutedEventArgs e)
        {
            if (_familyEditedCancelled != null)
                _familyEditedCancelled();
            FamilyDialogDismissed();
        }

        private void FamilyDialog_BackButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_familyEditedCancelled != null)
                _familyEditedCancelled();
            FamilyDialogDismissed();
        }

        private void FamilyDialogDismissed()
        {
            _familyEditedCompleted = null;
            _familyEditedCancelled = null;
            FamilyDialog.IsOpen = false;
        }
    }
}
