using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BillTender.Budget.ViewModels;
using UpdateControls.XAML;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BillTender.Budget.Views
{
    public sealed partial class BudgetPage : Page
    {
        public BudgetPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = ForView.Unwrap<BudgetViewModel>(DataContext);
            if (viewModel != null)
            {
                viewModel.BillEdited += BillEdited;
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var viewModel = ForView.Unwrap<BudgetViewModel>(DataContext);
            if (viewModel != null)
            {
                viewModel.BillEdited -= BillEdited;
            }

            base.OnNavigatedFrom(e);
        }

        private void BillEdited(object sender, BillEditedEventArgs args)
        {
            Popup billPopup = new Popup()
            {
                ChildTransitions = new TransitionCollection { new PopupThemeTransition() }
            };
            BillDetailView detail = new BillDetailView()
            {
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height,
                DataContext = args.Bill
            };
            detail.Ok += delegate
            {
                billPopup.IsOpen = false;
                if (args.Completed != null)
                    args.Completed();
            };
            detail.Cancel += delegate
            {
                billPopup.IsOpen = false;
                if (args.Cancelled != null)
                    args.Cancelled();
            };
            billPopup.Child = detail;
            billPopup.IsOpen = true;
        }
    }
}
