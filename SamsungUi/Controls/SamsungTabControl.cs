using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// TabControl in stile Samsung One UI.
    /// </summary>
    public class SamsungTabControl : TabControl
    {
        static SamsungTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTabControl), new FrameworkPropertyMetadata(typeof(SamsungTabControl)));
        }
    }
}
