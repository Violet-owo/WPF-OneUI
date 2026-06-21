using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style CheckBox with rounded corners and fluid transitions.
    /// </summary>
    public class SamsungCheckBox : CheckBox
    {
        // --- Initialization ---

        static SamsungCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungCheckBox), new FrameworkPropertyMetadata(typeof(SamsungCheckBox)));
        }
    }
}
