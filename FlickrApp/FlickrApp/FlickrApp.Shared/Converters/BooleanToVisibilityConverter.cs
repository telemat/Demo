namespace FlickrApp.Converters
{
    #region Imports

    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    #endregion

    public class BooleanToVisibilityConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isVisible = (bool) value;
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (Visibility) value == Visibility.Visible;
        }
    }
}