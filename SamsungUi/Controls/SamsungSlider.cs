using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style Slider with support for ticks and fluid styling.
    /// </summary>
    public class SamsungSlider : Slider
    {
        // --- Initialization ---

        static SamsungSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungSlider), new FrameworkPropertyMetadata(typeof(SamsungSlider)));
        }
    }
}
