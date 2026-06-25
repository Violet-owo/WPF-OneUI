#pragma warning disable CS8600, CS8601, CS8602, CS8603, CS8604, CS8618, CS8622, CS8625
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

        /// <summary>
        /// Identifies the OverlayStyleKey dependency property.
        /// </summary>
        public static readonly DependencyProperty OverlayStyleKeyProperty =
            DependencyProperty.Register(nameof(OverlayStyleKey), typeof(string), typeof(SamsungModal), new PropertyMetadata("SamsungModalOverlayStyle"));

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

        /// <summary>
        /// Gets or sets the Resource Key of the style to apply to the overlay container.
        /// Defaults to "SamsungModalOverlayStyle".
        /// </summary>
        public string OverlayStyleKey
        {
            get => (string)GetValue(OverlayStyleKeyProperty);
            set => SetValue(OverlayStyleKeyProperty, value);
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
                Style = (Style)Application.Current.TryFindResource(OverlayStyleKey ?? "SamsungModalOverlayStyle"),
                DataContext = this.DataContext
            };

            _adorner = new OverlayAdorner(contentToAdorn, overlayContainer);
            adornerLayer.Add(_adorner);
        }

        private async void CloseModal()
        {
            if (_adorner != null)
            {
                var container = (_adorner as OverlayAdorner)?.Child as ContentControl;
                if (container != null)
                {
                    // Optionally find specific parts
                    container.ApplyTemplate();
                    var dialogContainer = container.Template.FindName("DialogContainer", container) as UIElement;
                    var overlay = container.Template.FindName("Overlay", container) as UIElement;

                    if (dialogContainer != null && overlay != null)
                    {
                        var fadeOut = new System.Windows.Media.Animation.DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                        var scaleOut = new System.Windows.Media.Animation.DoubleAnimation(0.2, TimeSpan.FromSeconds(0.2))
                        {
                            EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseIn }
                        };

                        overlay.BeginAnimation(UIElement.OpacityProperty, fadeOut);
                        dialogContainer.BeginAnimation(UIElement.OpacityProperty, fadeOut);

                        if (dialogContainer.RenderTransform is System.Windows.Media.TransformGroup tg)
                        {
                            foreach (var transform in tg.Children)
                            {
                                if (transform is System.Windows.Media.ScaleTransform stg)
                                {
                                    stg.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, scaleOut);
                                    stg.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, scaleOut);
                                }
                            }
                        }
                        else if (dialogContainer.RenderTransform is System.Windows.Media.ScaleTransform st)
                        {
                            st.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, scaleOut);
                            st.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, scaleOut);
                        }

                        await System.Threading.Tasks.Task.Delay(200);
                    }
                }

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
        public UIElement Child => _child;

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
