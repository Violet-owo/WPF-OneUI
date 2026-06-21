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

        // --- Properties ---

        public ButtonVariant Variant
        {
            get => (ButtonVariant)GetValue(VariantProperty);
            set => SetValue(VariantProperty, value);
        }

        // --- Initialization ---

        static SamsungButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungButton), new FrameworkPropertyMetadata(typeof(SamsungButton)));
        }
    }
}
