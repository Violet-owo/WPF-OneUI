using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class SamsungMenuItem : MenuItem
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SamsungMenuItem), new PropertyMetadata(new CornerRadius(8)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        static SamsungMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungMenuItem), new FrameworkPropertyMetadata(typeof(SamsungMenuItem)));
        }
    }
}
