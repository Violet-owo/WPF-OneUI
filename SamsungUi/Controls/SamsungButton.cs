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
    /// Pulsante in stile Samsung One UI.
    /// Supporta varianti Normal, Primary e Ghost per adattarsi a diversi contesti visivi.
    /// </summary>
    public class SamsungButton : Button
    {
        public static readonly DependencyProperty VariantProperty =
            DependencyProperty.Register(
                nameof(Variant),
                typeof(ButtonVariant),
                typeof(SamsungButton),
                new PropertyMetadata(ButtonVariant.Normal));

        public ButtonVariant Variant
        {
            get => (ButtonVariant)GetValue(VariantProperty);
            set => SetValue(VariantProperty, value);
        }

        static SamsungButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungButton), new FrameworkPropertyMetadata(typeof(SamsungButton)));
        }
    }
}
