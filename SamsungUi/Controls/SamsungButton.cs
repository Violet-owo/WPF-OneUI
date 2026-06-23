using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public enum ButtonVariant
    {
        Normal,
        Primary,
        Ghost
    }

    /// <summary>
    /// A Samsung One UI style button.
    /// Supports Normal, Primary, and Ghost variants to adapt to different visual contexts.
    /// </summary>
    public class SamsungButton : Button
    {
        // --- Dependency Properties ---
        public static readonly DependencyProperty VariantProperty =
            DependencyProperty.Register(
                nameof(Variant),
                typeof(ButtonVariant),
                typeof(SamsungButton),
                new PropertyMetadata(ButtonVariant.Normal));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SamsungButton),
                new PropertyMetadata(new CornerRadius(20)));

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(
                nameof(IsLoading),
                typeof(bool),
                typeof(SamsungButton),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register(
                nameof(IconData),
                typeof(System.Windows.Media.Geometry),
                typeof(SamsungButton),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(SamsungButton),
                new PropertyMetadata(16.0));

        // --- Properties ---

        public ButtonVariant Variant
        {
            get => (ButtonVariant)GetValue(VariantProperty);
            set => SetValue(VariantProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public System.Windows.Media.Geometry IconData
        {
            get => (System.Windows.Media.Geometry)GetValue(IconDataProperty);
            set => SetValue(IconDataProperty, value);
        }

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
