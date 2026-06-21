using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Slider in stile Samsung One UI con supporto per i tick e uno stile fluido.
    /// </summary>
    public class SamsungSlider : Slider
    {
        static SamsungSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungSlider), new FrameworkPropertyMetadata(typeof(SamsungSlider)));
        }
    }
}
