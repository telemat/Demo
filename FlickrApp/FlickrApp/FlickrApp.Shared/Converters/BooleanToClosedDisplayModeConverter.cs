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
#if WINDOWS_PHONE_APP
            return isMinimal ? AppBarClosedDisplayMode.Minimal : AppBarClosedDisplayMode.Compact;
#else
            return null;
#endif
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
#if WINDOWS_PHONE_APP
            return (AppBarClosedDisplayMode) value == AppBarClosedDisplayMode.Minimal;
#else
            return null;
#endif
        }
    }
}