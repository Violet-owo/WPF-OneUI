using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a Card container typical of Samsung One UI design.
    /// Used to group related elements with rounded corners, standard padding, and a soft shadow.
    /// </summary>
    public class SamsungCard : ContentControl
    {
        // --- Initialization ---

        static SamsungCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungCard), new FrameworkPropertyMetadata(typeof(SamsungCard)));
            PaddingProperty.OverrideMetadata(typeof(SamsungCard), new FrameworkPropertyMetadata(new Thickness(24)));
        }
    }
}
