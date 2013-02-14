namespace Knet.Phone.Client.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class CollectionCountVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isVisible = (int)value == 0;
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
