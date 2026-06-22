using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style ListViewItem (inherits from ListBoxItem).
    /// Provides properties for an icon and handles the separator border.
    /// </summary>
    public class SamsungListViewItem : ListBoxItem
    {
        // --- Dependency Properties ---
        public static readonly DependencyProperty IconContentProperty =
            DependencyProperty.Register(nameof(IconContent), typeof(object), typeof(SamsungListViewItem), new PropertyMetadata(null));

        // --- Properties ---
        public object IconContent
        {
            get => GetValue(IconContentProperty);
            set => SetValue(IconContentProperty, value);
        }

        // --- Initialization ---
        static SamsungListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungListViewItem), new FrameworkPropertyMetadata(typeof(SamsungListViewItem)));
        }
    }
}
