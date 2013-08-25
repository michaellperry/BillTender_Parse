using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRtBehaviors;

namespace BillTender.Behaviors
{
    public class BindVisualStateBehavior : Behavior<Page>
    {
        public static DependencyProperty StateNameProperty = DependencyProperty.Register(
            "StateName",
            typeof(string),
            typeof(BindVisualStateBehavior),
            new PropertyMetadata(String.Empty, VisualStatePropertyChanged));
        private bool _initialized = false;

        public string StateName
        {
            get { return (string)GetValue(StateNameProperty); }
            set { SetValue(StateNameProperty, value); }
        }

        public void UpdateVisualState(string visualState)
        {
            if (AssociatedObject != null && !String.IsNullOrEmpty(visualState))
            {
                VisualStateManager.GoToState(AssociatedObject, visualState, _initialized);
                _initialized = true;
            }
        }

        protected override void OnAttached()
        {
            UpdateVisualState(StateName);
            base.OnAttached();
        }

        private static void VisualStatePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((BindVisualStateBehavior)obj).UpdateVisualState((string)args.NewValue);
        }
    }
}
