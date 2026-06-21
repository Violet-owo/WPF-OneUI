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

        // --- Properties ---

        public bool IsSearchBar
        {
            get => (bool)GetValue(IsSearchBarProperty);
            set => SetValue(IsSearchBarProperty, value);
        }

        // --- Initialization ---

        static SamsungTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTextBox), new FrameworkPropertyMetadata(typeof(SamsungTextBox)));
        }
    }
}
