using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Slycoder.UI
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            return (bool) value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            return value.ToString() != "0";
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            return (bool) value ? 1 : 0;
        }
    }
}