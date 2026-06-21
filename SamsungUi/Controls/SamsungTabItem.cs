using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style TabItem.
    /// </summary>
    public class SamsungTabItem : TabItem
    {
        // --- Initialization ---

        static SamsungTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTabItem), new FrameworkPropertyMetadata(typeof(SamsungTabItem)));
        }
    }
}
