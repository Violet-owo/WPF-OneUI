using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style Label.
    /// </summary>
    public class SamsungLabel : Label
    {
        static SamsungLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungLabel), new FrameworkPropertyMetadata(typeof(SamsungLabel)));
        }
    }
}
