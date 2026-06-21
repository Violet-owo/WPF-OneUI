using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Campo di testo in stile Samsung One UI.
    /// Supporta la variante IsSearchBar per essere utilizzato come barra di ricerca.
    /// </summary>
    public class SamsungTextBox : TextBox
    {
        public static readonly DependencyProperty IsSearchBarProperty =
            DependencyProperty.Register(
                nameof(IsSearchBar),
                typeof(bool),
                typeof(SamsungTextBox),
                new PropertyMetadata(false));

        public bool IsSearchBar
        {
            get => (bool)GetValue(IsSearchBarProperty);
            set => SetValue(IsSearchBarProperty, value);
        }

        static SamsungTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTextBox), new FrameworkPropertyMetadata(typeof(SamsungTextBox)));
        }
    }
}
