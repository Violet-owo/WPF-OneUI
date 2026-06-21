using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style RadioButton.
    /// </summary>
    public class SamsungRadioButton : RadioButton
    {
        // --- Initialization ---

        static SamsungRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungRadioButton), new FrameworkPropertyMetadata(typeof(SamsungRadioButton)));
        }
    }
}
