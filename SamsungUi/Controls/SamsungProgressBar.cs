using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Indicatore di progresso in stile Samsung One UI.
    /// Imposta <see cref="IsRing"/> = true per usare lo stile ad anello circolare animato.
    /// </summary>
    [TemplatePart(Name = "PART_Indicator", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_Track", Type = typeof(FrameworkElement))]
    public class SamsungProgressBar : ProgressBar
    {
        public static readonly DependencyProperty IsRingProperty =
            DependencyProperty.Register(
                nameof(IsRing),
                typeof(bool),
                typeof(SamsungProgressBar),
                new PropertyMetadata(false));

        public bool IsRing
        {
            get => (bool)GetValue(IsRingProperty);
            set => SetValue(IsRingProperty, value);
        }

        static SamsungProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungProgressBar),
                new FrameworkPropertyMetadata(typeof(SamsungProgressBar)));
        }
    }
}
