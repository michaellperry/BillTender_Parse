using System;
using Callisto.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace BillTender.Helpers
{
    public class DialogFrame
    {
        private readonly Action _completed;
        private readonly Action _cancelled;

        private CustomDialog _dialog;
        private Panel _layoutPanel;

        public DialogFrame(
            object content,
            string title,
            object dataContext,
            Color backgroundColor,
            Action completed,
            Action cancelled)
        {
            _completed = completed;
            _cancelled = cancelled;

            var frame = (ContentControl)Window.Current.Content;
            var page = (UserControl)frame.Content;
            _layoutPanel = (Panel)page.Content;

            _dialog = new CustomDialog()
            {
                Transitions = new TransitionCollection
                {
                    new PopupThemeTransition()
                },
                BackButtonVisibility = Visibility.Visible,
                Background = new SolidColorBrush(backgroundColor),
                Content = content,
                Title = title,
                DataContext = dataContext
            };

            _dialog.BackButtonClicked += OnCancel;
        }

        public void Open()
        {
            _layoutPanel.Children.Add(_dialog);
            _dialog.IsOpen = true;
        }

        public void Close()
        {
            _dialog.IsOpen = false;
            _layoutPanel.Children.Remove(_dialog);
        }

        public void OnOk(object sender, RoutedEventArgs args)
        {
            Close();
            if (_completed != null)
                _completed();
        }

        public void OnCancel(object sender, RoutedEventArgs args)
        {
            Close();
            if (_cancelled != null)
                _cancelled();
        }
    }
}
