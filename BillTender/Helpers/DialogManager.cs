using System;
using BillTender.Budget.Models;
using BillTender.Budget.Views;
using BillTender.Families.Models;
using BillTender.Families.Views;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

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
            FamilyDialog familyDialog = new FamilyDialog();

            var dialogFrame = new DialogFrame(
                familyDialog,
                title,
                family,
                Color.FromArgb(0xff, 0xaf, 0xbe, 0xe2),
                completed,
                cancelled);

            familyDialog.Ok += dialogFrame.OnOk;
            familyDialog.Cancel += dialogFrame.OnCancel;

            dialogFrame.Open();
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

        public static void ShowInvitationDialog(
            InvitationModel invitation,
            Action completed = null,
            Action cancelled = null)
        {
            var invitationDialog = new InvitationDialog();

            var dialogFrame = new DialogFrame(
                invitationDialog,
                "Add member",
                invitation,
                Color.FromArgb(0xFF, 0xD8, 0xC3, 0xD4),
                completed,
                cancelled);

            invitationDialog.Ok += dialogFrame.OnOk;
            invitationDialog.Cancel += dialogFrame.OnCancel;

            dialogFrame.Open();
        }
    }
}