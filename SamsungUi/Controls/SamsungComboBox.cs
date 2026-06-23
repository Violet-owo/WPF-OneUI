using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class SamsungComboBox : ComboBox
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(SamsungComboBox),
                new PropertyMetadata(string.Empty));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SamsungComboBox),
                new PropertyMetadata(new CornerRadius(16)));

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
