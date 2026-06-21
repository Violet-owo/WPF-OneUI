using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Rappresenta un interruttore (toggle switch) con grafica e animazioni in stile Samsung One UI.
    /// Si basa sul comportamento nativo di <see cref="CheckBox"/>.
    /// </summary>
    public class SamsungToggleSwitch : CheckBox
    {
        static SamsungToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungToggleSwitch), new FrameworkPropertyMetadata(typeof(SamsungToggleSwitch)));
        }
    }
}
