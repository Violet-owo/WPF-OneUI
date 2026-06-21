using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// RadioButton in stile Samsung One UI.
    /// </summary>
    public class SamsungRadioButton : RadioButton
    {
        static SamsungRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungRadioButton), new FrameworkPropertyMetadata(typeof(SamsungRadioButton)));
        }
    }
}
