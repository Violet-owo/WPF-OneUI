#pragma warning disable CS8600, CS8601, CS8602, CS8603, CS8604, CS8618, CS8622, CS8625
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SamsungUi.Controls
{
    public class TextToVisibilityConverter : IValueConverter
    {
        public static readonly TextToVisibilityConverter Instance = new TextToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;
            return string.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
