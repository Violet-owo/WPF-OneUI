using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a toggle switch with Samsung One UI style graphics and animations.
    /// It is based on the native behavior of <see cref="CheckBox"/>.
    /// </summary>
    public class SamsungToggleSwitch : CheckBox
    {
        // --- Initialization ---

        static SamsungToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungToggleSwitch), new FrameworkPropertyMetadata(typeof(SamsungToggleSwitch)));
        }
    }
}
