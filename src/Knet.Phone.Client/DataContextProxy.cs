namespace Knet.Phone.Client
{
    using System.Windows;

    public class DataContextProxy : FrameworkElement
    {
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register("DataContext", typeof(object), typeof(DataContextProxy), new PropertyMetadata(default(object)));

        public object DataContext
        {
            get
            {
                return (object)GetValue(DataContextProperty);
            }
            set
            {
                SetValue(DataContextProperty, value);
            }
        }
    }
}
