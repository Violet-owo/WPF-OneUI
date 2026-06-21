using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style progress indicator.
    /// Set <see cref="IsRing"/> = true to use the animated circular ring style.
    /// </summary>
    [TemplatePart(Name = "PART_Indicator", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_Track", Type = typeof(FrameworkElement))]
    public class SamsungProgressBar : ProgressBar
    {
        // --- Dependency Properties ---
        public static readonly DependencyProperty IsRingProperty =
            DependencyProperty.Register(
                nameof(IsRing),
                typeof(bool),
                typeof(SamsungProgressBar),
                new PropertyMetadata(false));

        // --- Properties ---

        public bool IsRing
        {
            get => (bool)GetValue(IsRingProperty);
            set => SetValue(IsRingProperty, value);
        }

        // --- Initialization ---

        static SamsungProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungProgressBar),
                new FrameworkPropertyMetadata(typeof(SamsungProgressBar)));
        }
    }
}
