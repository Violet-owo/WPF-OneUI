using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Rappresenta una finestra di dialogo modale personalizzata stile Samsung One UI.
    /// Utilizza il layer Adorner per oscurare la finestra principale e centrare il contenuto
    /// senza ricorrere a finestre di popup native o a elementi bloccanti, per una massima fluidità.
    /// </summary>
    public class SamsungModal : ContentControl
    {
        static SamsungModal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungModal), new FrameworkPropertyMetadata(typeof(SamsungModal)));
        }

        /// <summary>
        /// Identifica la dependency property per lo stato di apertura/chiusura della modale.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(SamsungModal), new PropertyMetadata(false, OnIsOpenChanged));

        /// <summary>
        /// Ottiene o imposta un valore che indica se la modale è visibile a schermo.
        /// Può essere agganciato in binding (es. ad un ViewModel).
        /// </summary>
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        private Adorner _adorner;
        private object _savedContent;

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var modal = (SamsungModal)d;
            if ((bool)e.NewValue)
                modal.ShowModal();
            else
                modal.CloseModal();
        }

        private void ShowModal()
        {
            if (_adorner != null) return;
            var window = Window.GetWindow(this) ?? Application.Current.MainWindow;
            if (window == null) return;

            var contentToAdorn = window.Content as UIElement;
            if (contentToAdorn == null) return;

            var adornerLayer = AdornerLayer.GetAdornerLayer(contentToAdorn);
            if (adornerLayer == null) return;

            _savedContent = this.Content;
            this.Content = null;

            var overlayContainer = new ContentControl
            {
                Content = _savedContent,
                Style = (Style)Application.Current.TryFindResource("SamsungModalOverlayStyle"),
                DataContext = this.DataContext
            };

            _adorner = new OverlayAdorner(contentToAdorn, overlayContainer);
            adornerLayer.Add(_adorner);
        }

        private void CloseModal()
        {
            if (_adorner != null)
            {
                var window = Window.GetWindow(this) ?? Application.Current.MainWindow;
                if (window != null && window.Content is UIElement contentToAdorn)
                {
                    var adornerLayer = AdornerLayer.GetAdornerLayer(contentToAdorn);
                    adornerLayer?.Remove(_adorner);
                }
                this.Content = _savedContent;
                _savedContent = null;
                _adorner = null;
            }
        }
    }

    /// <summary>
    /// Classe interna utilizzata per proiettare la modale e la patina oscurante sopra la finestra principale
    /// attraverso il sistema Adorner di WPF.
    /// </summary>
    public class OverlayAdorner : Adorner
    {
        private readonly UIElement _child;
        public OverlayAdorner(UIElement adornedElement, UIElement child) : base(adornedElement)
        {
            _child = child;
            AddLogicalChild(_child);
            AddVisualChild(_child);
        }
        protected override int VisualChildrenCount => 1;
        protected override Visual GetVisualChild(int index) => _child;
        protected override Size MeasureOverride(Size constraint)
        {
            _child.Measure(AdornedElement.RenderSize);
            return AdornedElement.RenderSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            _child.Arrange(new Rect(new Point(0, 0), AdornedElement.RenderSize));
            return AdornedElement.RenderSize;
        }
    }
}
