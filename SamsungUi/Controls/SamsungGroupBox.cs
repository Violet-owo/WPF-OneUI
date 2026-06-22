using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style GroupBox with rounded corners and clean typography.
    /// </summary>
    public class SamsungGroupBox : GroupBox
    {
        // --- Dependency Properties ---
        public static readonly DependencyProperty IconContentProperty =
            DependencyProperty.Register(nameof(IconContent), typeof(object), typeof(SamsungGroupBox), new PropertyMetadata(null));

        // --- Properties ---

        public object IconContent
        {
            get => GetValue(IconContentProperty);
            set => SetValue(IconContentProperty, value);
        }

        // --- Initialization ---

        static SamsungGroupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungGroupBox), new FrameworkPropertyMetadata(typeof(SamsungGroupBox)));
        }
    }
}
