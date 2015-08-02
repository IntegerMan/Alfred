using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MattEland.Ani.Alfred.Win8
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Guard against null values
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            // Okay, we have an actual value. We'll inspect it and then handle it according to its type
            bool isVisible;

            if (value is bool)
            {
                // It's boolean! Great!
                isVisible = (bool)value;
            }
            else if (value is Visibility)
            {
                // What the heck? We'll judge based on if it is Visible already
                isVisible = ((Visibility)value == Visibility.Visible);
            }
            else
            {
                // It's some other value. We'll just trust bool.TryParse with it
                bool.TryParse(value.ToString(), out isVisible);
            }

            // Translate from our boolean to a Visibility value
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("ConvertBack is not supported by this converter");
        }
    }
}