using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style text box.
    /// Supports the IsSearchBar variant to be used as a search bar.
    /// </summary>
    public class SamsungTextBox : TextBox
    {
        // --- Dependency Properties ---
        public static readonly DependencyProperty IsSearchBarProperty =
            DependencyProperty.Register(
                nameof(IsSearchBar),
                typeof(bool),
                typeof(SamsungTextBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(SamsungTextBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SamsungTextBox),
                new PropertyMetadata(new CornerRadius(16)));

        // --- Properties ---

        public bool IsSearchBar
        {
            get => (bool)GetValue(IsSearchBarProperty);
            set => SetValue(IsSearchBarProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // --- Initialization ---

        static SamsungTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTextBox), new FrameworkPropertyMetadata(typeof(SamsungTextBox)));
        }
    }
}
