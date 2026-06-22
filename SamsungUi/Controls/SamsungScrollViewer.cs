using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style ScrollViewer.
    /// </summary>
    public class SamsungScrollViewer : ScrollViewer
    {
        static SamsungScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungScrollViewer), new FrameworkPropertyMetadata(typeof(SamsungScrollViewer)));
        }
    }
}
