using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a Card container typical of Samsung One UI design.
    /// Used to group related elements with rounded corners, standard padding, and a soft shadow.
    /// </summary>
    public class SamsungCard : ContentControl
    {
        // --- Dependency Properties ---
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SamsungCard),
                new PropertyMetadata(new CornerRadius(26)));

        // --- Properties ---

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // --- Initialization ---

        static SamsungCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungCard), new FrameworkPropertyMetadata(typeof(SamsungCard)));
            PaddingProperty.OverrideMetadata(typeof(SamsungCard), new FrameworkPropertyMetadata(new Thickness(24)));
        }
    }
}
