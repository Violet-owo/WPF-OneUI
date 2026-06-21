using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// CheckBox in stile Samsung One UI con angoli arrotondati e transizioni fluide.
    /// </summary>
    public class SamsungCheckBox : CheckBox
    {
        static SamsungCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungCheckBox), new FrameworkPropertyMetadata(typeof(SamsungCheckBox)));
        }
    }
}
