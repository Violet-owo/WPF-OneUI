using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// ListBoxItem in stile Samsung One UI.
    /// </summary>
    public class SamsungListBoxItem : ListBoxItem
    {
        static SamsungListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungListBoxItem), new FrameworkPropertyMetadata(typeof(SamsungListBoxItem)));
        }
    }
}
