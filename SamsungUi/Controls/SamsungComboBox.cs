using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class SamsungComboBox : ComboBox
    {
        static SamsungComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungComboBox), new FrameworkPropertyMetadata(typeof(SamsungComboBox)));
        }
    }
}
