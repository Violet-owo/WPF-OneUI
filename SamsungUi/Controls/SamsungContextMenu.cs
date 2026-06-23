using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class SamsungContextMenu : ContextMenu
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SamsungContextMenu), new PropertyMetadata(new CornerRadius(12)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        static SamsungContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungContextMenu), new FrameworkPropertyMetadata(typeof(SamsungContextMenu)));
        }
    }
}
