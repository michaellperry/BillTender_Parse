﻿using System;
using BillTender.Families.Models;
using BillTender.Families.Views;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Callisto.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using BillTender.Budget.Views;
using BillTender.Budget.Models;

namespace BillTender.Helpers
{
    public static class DialogManager
    {
        public static void ShowFamilyDialog(
            string title,
            Family family,
            Action completed = null,
            Action cancelled = null)
        {
            var frame = (Frame)Window.Current.Content;
            var page = (Page)frame.Content;
            var grid = (Grid)page.Content;

            var dialog = new CustomDialog();
            dialog.BackButtonVisibility = Visibility.Visible;
            dialog.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xaf, 0xbe, 0xe2));
            FamilyDialog familyDialog = new FamilyDialog();
            dialog.Content = familyDialog;
            familyDialog.Ok += delegate
            {
                dialog.IsOpen = false;
                grid.Children.Remove(dialog);
                if (completed != null)
                    completed();
            };
            familyDialog.Cancel += delegate
            {
                dialog.IsOpen = false;
                grid.Children.Remove(dialog);
                if (cancelled != null)
                    cancelled();
            };
            dialog.BackButtonClicked += delegate
            {
                dialog.IsOpen = false;
                grid.Children.Remove(dialog);
                if (cancelled != null)
                    cancelled();
            };

            dialog.Title = title;
            dialog.DataContext = family;

            grid.Children.Add(dialog);
            dialog.IsOpen = true;
        }

        public static void ShowBillDialog(
            Bill bill,
            Action completed = null,
            Action cancelled = null)
        {
            Popup billPopup = new Popup()
            {
                ChildTransitions = new TransitionCollection { new PopupThemeTransition() }
            };
            BillDetailView detail = new BillDetailView()
            {
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height,
                DataContext = bill
            };
            detail.Ok += delegate
            {
                billPopup.IsOpen = false;
                if (completed != null)
                    completed();
            };
            detail.Cancel += delegate
            {
                billPopup.IsOpen = false;
                if (cancelled != null)
                    cancelled();
            };
            billPopup.Child = detail;
            billPopup.IsOpen = true;
        }
    }
}