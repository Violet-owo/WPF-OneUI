using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a custom modal dialog in Samsung One UI style.
    /// It uses the Adorner layer to dim the main window and center the content
    /// without resorting to native popup windows or blocking elements, for maximum fluidity.
    /// </summary>
    public class SamsungModal : ContentControl
    {
        // --- Initialization ---
        static SamsungModal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungModal), new FrameworkPropertyMetadata(typeof(SamsungModal)));
        }

        // --- Dependency Properties ---

        /// <summary>
        /// Identifies the IsOpen dependency property.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(SamsungModal), new PropertyMetadata(false, OnIsOpenChanged));

        // --- Properties ---

        /// <summary>
        /// Gets or sets a value indicating whether the modal is visible on screen.
        /// Can be data bound (e.g. to a ViewModel).
        /// </summary>
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        // --- Fields ---

        private Adorner _adorner;
        private object _savedContent;

        // --- Event Handlers & Callbacks ---

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var modal = (SamsungModal)d;
            if ((bool)e.NewValue)
                modal.ShowModal();
            else
                modal.CloseModal();
        }

        // --- Methods ---

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
    /// Internal class used to project the modal and the dimming overlay over the main window
    /// through the WPF Adorner system.
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
