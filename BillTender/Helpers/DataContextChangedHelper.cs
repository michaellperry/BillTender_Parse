using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

// Thanks to Jeremy Likness and András Velvárt.
// http://dotneteers.net/blogs/vbandi/archive/2013/01/23/datacontextchanged-event-for-winrt.aspx

namespace BillTender
{
    public static class DataContextChangedHelper<T> where T : FrameworkElement, IDataContextChangedHandler<T>
    {
        private const string INTERNAL_CONTEXT = "InternalDataContext";

        public static readonly DependencyProperty InternalDataContextProperty =
            DependencyProperty.Register(INTERNAL_CONTEXT,
                                        typeof(Object),
                                        typeof(T),
                                        new PropertyMetadata(null, _DataContextChanged));

        private static void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            T control = (T)sender;
            control.DataContextChanged(control, e);
        }

        public static void Bind(T control)
        {
            control.SetBinding(InternalDataContextProperty, new Binding());
        }
    }

    public interface IDataContextChangedHandler<T> where T : FrameworkElement
    {
        void DataContextChanged(T sender, DependencyPropertyChangedEventArgs e);
    }
}
