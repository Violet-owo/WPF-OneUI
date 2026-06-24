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
        
        /// <summary>
        /// Identifies the <see cref="IsSearchBar"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSearchBarProperty =
            DependencyProperty.Register(
                nameof(IsSearchBar),
                typeof(bool),
                typeof(SamsungTextBox),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="Placeholder"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(SamsungTextBox),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SamsungTextBox),
                new PropertyMetadata(new CornerRadius(16)));

        // --- Properties ---

        /// <summary>
        /// Gets or sets a value indicating whether the text box acts as a search bar (typically showing a search icon).
        /// </summary>
        /// <value><c>true</c> if it is a search bar; otherwise, <c>false</c>.</value>
        public bool IsSearchBar
        {
            get => (bool)GetValue(IsSearchBarProperty);
            set => SetValue(IsSearchBarProperty, value);
        }

        /// <summary>
        /// Gets or sets the placeholder text displayed when the text box is empty.
        /// </summary>
        /// <value>A string representing the placeholder text.</value>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        /// Gets or sets the corner radius of the text box.
        /// </summary>
        /// <value>A <see cref="System.Windows.CornerRadius"/> value. Default is 16.</value>
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
