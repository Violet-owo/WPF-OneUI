using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style ListBoxItem.
    /// </summary>
    public class SamsungListBoxItem : ListBoxItem
    {
        // --- Initialization ---

        static SamsungListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungListBoxItem), new FrameworkPropertyMetadata(typeof(SamsungListBoxItem)));
        }
    }
}
