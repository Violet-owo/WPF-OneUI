using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style Tooltip.
    /// </summary>
    public class SamsungTooltip : ToolTip
    {
        static SamsungTooltip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTooltip), new FrameworkPropertyMetadata(typeof(SamsungTooltip)));
        }
    }
}
