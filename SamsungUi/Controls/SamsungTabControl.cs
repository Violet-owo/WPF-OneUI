using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style TabControl.
    /// </summary>
    public class SamsungTabControl : TabControl
    {
        // --- Initialization ---

        static SamsungTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTabControl), new FrameworkPropertyMetadata(typeof(SamsungTabControl)));
        }
    }
}
