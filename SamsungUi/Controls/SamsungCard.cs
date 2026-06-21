using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Rappresenta un contenitore a forma di Card tipico del design Samsung One UI.
    /// Utilizzato per raggruppare elementi correlati con bordi arrotondati, padding standard e un'ombra soffice.
    /// </summary>
    public class SamsungCard : ContentControl
    {
        static SamsungCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungCard), new FrameworkPropertyMetadata(typeof(SamsungCard)));
            PaddingProperty.OverrideMetadata(typeof(SamsungCard), new FrameworkPropertyMetadata(new Thickness(24)));
        }
    }
}
