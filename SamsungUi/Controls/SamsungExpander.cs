using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style expander.
    /// </summary>
    public class SamsungExpander : Expander
    {
        // --- Dependency Properties ---
        public static readonly DependencyProperty IconContentProperty =
            DependencyProperty.Register(nameof(IconContent), typeof(object), typeof(SamsungExpander), new PropertyMetadata(null));

        // --- Properties ---

        public object IconContent
        {
            get => GetValue(IconContentProperty);
            set => SetValue(IconContentProperty, value);
        }

        // --- Initialization ---

        static SamsungExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungExpander), new FrameworkPropertyMetadata(typeof(SamsungExpander)));
        }

        // --- Event Handlers ---

        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            
            // Se l'expander è aperto e intercetta un evento di scorrimento (anche se il mouse è fuori, 
            // perché il Popup cattura l'input), blocchiamo l'evento in modo che non "bubbli" 
            // fino allo ScrollViewer della pagina sottostante.
            if (IsExpanded && !e.Handled)
            {
                e.Handled = true;
            }
        }
    }
}
