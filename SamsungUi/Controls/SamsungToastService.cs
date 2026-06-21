using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Classe interna utilizzata per proiettare il toast sopra l'interfaccia utente.
    /// Utilizza il layer Adorner per garantire che il toast sia sempre in primo piano
    /// senza ricorrere a finestre di sistema.
    /// </summary>
    public class ToastAdorner : System.Windows.Documents.Adorner
    {
        private UIElement _child;

        public ToastAdorner(UIElement adornedElement, UIElement child) : base(adornedElement)
        {
            _child = child;
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

    /// <summary>
    /// Servizio statico per visualizzare notifiche effimere (toast) in stile Samsung One UI.
    /// I toast vengono visualizzati nella parte inferiore dello schermo e scompaiono automaticamente.
    /// </summary>
    public static class SamsungToastService
    {
        /// <summary>
        /// Mostra un nuovo messaggio toast sullo schermo.
        /// </summary>
        /// <param name="message">Il testo principale da visualizzare nel toast.</param>
        /// <param name="actionText">Testo opzionale per un pulsante di azione a destra (es. "ANNULLA").</param>
        /// <param name="actionCallback">Azione eseguita al click del pulsante opzionale.</param>
        /// <param name="autoClose">Indica se il toast deve chiudersi da solo dopo alcuni secondi.</param>
        public static void Show(string message, string actionText = null, Action actionCallback = null, bool autoClose = true)
        {
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow == null || mainWindow.Content as UIElement == null) return;
                
                var rootElement = mainWindow.Content as UIElement;
                var adornerLayer = System.Windows.Documents.AdornerLayer.GetAdornerLayer(rootElement);
                if (adornerLayer == null) return;

                var overlayGrid = new Grid
                {
                    Background = Brushes.Transparent,
                    IsHitTestVisible = false // The toast itself will handle hits
                };

                // Retrieve the primary color from resources, fallback to Blue
                var primaryBrush = (Brush)Application.Current.TryFindResource("OneUiPrimaryBrush") ?? Brushes.DodgerBlue;

                var border = new Border
                {
                    CornerRadius = new CornerRadius(24),
                    Background = new SolidColorBrush(Color.FromArgb(230, 100, 100, 100)), // Dark translucent
                    Padding = new Thickness(24, 14, 24, 14),
                    Opacity = 0,
                    RenderTransform = new TranslateTransform(0, 30), // Start lower
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(0, 0, 0, 60),
                    IsHitTestVisible = true // Enable clicks on the toast
                };

                var panel = new StackPanel { Orientation = Orientation.Horizontal };
                
                var textBlock = new TextBlock
                {
                    Text = message,
                    Foreground = Brushes.White,
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = string.IsNullOrEmpty(actionText) ? new Thickness(0) : new Thickness(0, 0, 16, 0)
                };
                panel.Children.Add(textBlock);

                ToastAdorner adorner = null;

                if (!string.IsNullOrEmpty(actionText))
                {
                    var actionBtn = new Button
                    {
                        Content = actionText.ToUpper(),
                        Foreground = primaryBrush,
                        Background = Brushes.Transparent,
                        BorderThickness = new Thickness(0),
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        VerticalAlignment = VerticalAlignment.Center,
                        Cursor = System.Windows.Input.Cursors.Hand,
                        Template = CreateTransparentButtonTemplate()
                    };
                    actionBtn.Click += (s, e) =>
                    {
                        actionCallback?.Invoke();
                        if (adorner != null) adornerLayer.Remove(adorner);
                    };
                    panel.Children.Add(actionBtn);
                    overlayGrid.IsHitTestVisible = true; // Need to hit test the overlay if we have a button
                }

                border.Child = panel;
                overlayGrid.Children.Add(border);
                
                adorner = new ToastAdorner(rootElement, overlayGrid);
                adornerLayer.Add(adorner);

                // Animazione apparizione
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(350)) 
                    { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
                var slideUp = new DoubleAnimation(30, 0, TimeSpan.FromMilliseconds(350)) 
                    { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
                
                border.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                border.RenderTransform.BeginAnimation(TranslateTransform.YProperty, slideUp);

                if (autoClose)
                {
                    // Auto-close: 3 secondi per label normale, 6 per action toast (così c'è tempo per cliccare)
                    int delayMs = string.IsNullOrEmpty(actionText) ? 3000 : 6000;
                    await Task.Delay(delayMs);
                    
                    if (adornerLayer.GetAdorners(rootElement) != null)
                    {
                        var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(250)) 
                            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn } };
                        fadeOut.Completed += (s, e) => {
                            try { adornerLayer.Remove(adorner); } catch { }
                        };
                        border.BeginAnimation(UIElement.OpacityProperty, fadeOut);
                    }
                }
            });
        }

        private static ControlTemplate CreateTransparentButtonTemplate()
        {
            var template = new ControlTemplate(typeof(Button));
            var border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.BackgroundProperty, Brushes.Transparent);
            var content = new FrameworkElementFactory(typeof(ContentPresenter));
            content.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            content.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            border.AppendChild(content);
            template.VisualTree = border;
            return template;
        }
    }
}
