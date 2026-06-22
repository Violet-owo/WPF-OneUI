using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public enum BadgeStyle
    {
        Default,
        Primary,
        Success,
        Warning,
        Error
    }

    public class SamsungBadge : ContentControl
    {
        static SamsungBadge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungBadge), new FrameworkPropertyMetadata(typeof(SamsungBadge)));
        }

        public static readonly DependencyProperty BadgeStyleProperty =
            DependencyProperty.Register("BadgeStyle", typeof(BadgeStyle), typeof(SamsungBadge), new PropertyMetadata(BadgeStyle.Default));

        public BadgeStyle BadgeStyle
        {
            get { return (BadgeStyle)GetValue(BadgeStyleProperty); }
            set { SetValue(BadgeStyleProperty, value); }
        }
    }
}
