using System;
using System.Windows;

namespace SamsungUi.Appearance
{
    public enum ThemeType
    {
        Light,
        Dark
    }

    public static class ThemeManager
    {
        // --- Methods ---
        public static void ApplyTheme(ThemeType theme)
        {
            var app = Application.Current;
            if (app == null) return;

            var themeUri = theme == ThemeType.Light 
                ? new Uri("pack://application:,,,/SamsungUi;component/Themes/ColorsLight.xaml", UriKind.Absolute)
                : new Uri("pack://application:,,,/SamsungUi;component/Themes/ColorsDark.xaml", UriKind.Absolute);

            // Find and remove the existing color dictionary
            ResourceDictionary? existingThemeDict = null;
            foreach (var dict in app.Resources.MergedDictionaries)
            {
                if (dict.Source != null && dict.Source.ToString().Contains("Colors"))
                {
                    existingThemeDict = dict;
                    break;
                }
            }

            if (existingThemeDict != null)
            {
                app.Resources.MergedDictionaries.Remove(existingThemeDict);
            }

            // Add the new color dictionary
            app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = themeUri });
        }
    }
}
