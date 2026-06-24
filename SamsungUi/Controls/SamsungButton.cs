using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Specifies the visual variant of the <see cref="SamsungButton"/>.
    /// </summary>
    public enum ButtonVariant
    {
        /// <summary>
        /// The default button style with a secondary background color.
        /// </summary>
        Normal,

        /// <summary>
        /// The primary button style, typically used for the main action on a page. Uses the primary theme color.
        /// </summary>
        Primary,

        /// <summary>
        /// A transparent button that only shows text/icon, useful for less prominent actions.
        /// </summary>
        Ghost
    }

    /// <summary>
    /// A Samsung One UI style button.
    /// Supports Normal, Primary, and Ghost variants to adapt to different visual contexts.
    /// </summary>
    public class SamsungButton : Button
    {
        // --- Dependency Properties ---

        /// <summary>
        /// Identifies the <see cref="Variant"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VariantProperty =
            DependencyProperty.Register(
                nameof(Variant),
                typeof(ButtonVariant),
                typeof(SamsungButton),
                new PropertyMetadata(ButtonVariant.Normal));

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SamsungButton),
                new PropertyMetadata(new CornerRadius(20)));

        /// <summary>
        /// Identifies the <see cref="IsLoading"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(
                nameof(IsLoading),
                typeof(bool),
                typeof(SamsungButton),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IconData"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register(
                nameof(IconData),
                typeof(System.Windows.Media.Geometry),
                typeof(SamsungButton),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="IconSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(SamsungButton),
                new PropertyMetadata(16.0));

        // --- Properties ---

        /// <summary>
        /// Gets or sets the visual variant of the button.
        /// </summary>
        /// <value>The <see cref="ButtonVariant"/> that defines the button's appearance. Default is <see cref="ButtonVariant.Normal"/>.</value>
        /// <remarks>
        /// Changing this property will automatically update the button's background and foreground brushes based on the current theme.
        /// </remarks>
        public ButtonVariant Variant
        {
            get => (ButtonVariant)GetValue(VariantProperty);
            set => SetValue(VariantProperty, value);
        }

        /// <summary>
        /// Gets or sets the corner radius of the button, controlling how rounded the corners are.
        /// </summary>
        /// <value>A <see cref="System.Windows.CornerRadius"/> value. Default is 20.</value>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button is in a loading state.
        /// </summary>
        /// <value><c>true</c> if the button is loading; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// When <c>true</c>, the button's content is hidden, a spinner animation is shown, and the button becomes hit-test invisible (disabling clicks without changing the visual disabled state).
        /// </remarks>
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        /// <summary>
        /// Gets or sets the vector icon data to display inside the button alongside the text.
        /// </summary>
        /// <value>A <see cref="System.Windows.Media.Geometry"/> representing the icon path.</value>
        public System.Windows.Media.Geometry IconData
        {
            get => (System.Windows.Media.Geometry)GetValue(IconDataProperty);
            set => SetValue(IconDataProperty, value);
        }

        /// <summary>
        /// Gets or sets the size (width and height) of the displayed icon.
        /// </summary>
        /// <value>The size in logical pixels. Default is 16.0.</value>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        // --- Initialization ---

        static SamsungButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungButton), new FrameworkPropertyMetadata(typeof(SamsungButton)));
        }
    }
}
