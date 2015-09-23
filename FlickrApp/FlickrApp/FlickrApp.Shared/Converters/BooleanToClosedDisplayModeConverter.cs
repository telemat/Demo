namespace FlickrApp.Converters
{
    #region Imports

    using System;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;

    #endregion

    public class BooleanToClosedDisplayModeConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isMinimal = (bool) value;
            return isMinimal ? AppBarClosedDisplayMode.Minimal : AppBarClosedDisplayMode.Compact;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (AppBarClosedDisplayMode) value == AppBarClosedDisplayMode.Minimal;
        }
    }
}