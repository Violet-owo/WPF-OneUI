using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style combo box.
    /// Supports rounded corners and a customizable placeholder text when no item is selected.
    /// </summary>
    public class SamsungComboBox : ComboBox
    {
        /// <summary>
        /// Identifies the <see cref="Placeholder"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(SamsungComboBox),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the placeholder text displayed when the combo box selection is empty.
        /// </summary>
        /// <value>A string representing the placeholder text.</value>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SamsungComboBox),
                new PropertyMetadata(new CornerRadius(16)));

        /// <summary>
        /// Gets or sets the corner radius of the combo box.
        /// </summary>
        /// <value>A <see cref="System.Windows.CornerRadius"/> value. Default is 16.</value>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        static SamsungComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungComboBox), new FrameworkPropertyMetadata(typeof(SamsungComboBox)));
        }
    }
}
