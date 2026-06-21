using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// TabItem in stile Samsung One UI.
    /// </summary>
    public class SamsungTabItem : TabItem
    {
        static SamsungTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTabItem), new FrameworkPropertyMetadata(typeof(SamsungTabItem)));
        }
    }
}
