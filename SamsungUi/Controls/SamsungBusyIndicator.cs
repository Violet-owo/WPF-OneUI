#pragma warning disable CS8600, CS8601, CS8602, CS8603, CS8604, CS8618, CS8622, CS8625
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style BusyIndicator (modal overlay).
    /// Blocks the UI and shows a loading spinner over the current application window.
    /// </summary>
    public static class SamsungBusyIndicator
    {
        // --- Fields ---
        private static System.Windows.Documents.Adorner _currentAdorner;

        // --- Methods ---

        public static void Show(string message = "Please wait...")
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (_currentAdorner != null) return;

                var mainWindow = Application.Current.MainWindow;
                if (mainWindow == null || mainWindow.Content as UIElement == null) return;
                
                var rootElement = mainWindow.Content as UIElement;
                var adornerLayer = System.Windows.Documents.AdornerLayer.GetAdornerLayer(rootElement);
                if (adornerLayer == null) return;

                var overlayGrid = new Grid
                {
                    Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, 0, 0, 0)), // Semi-transparent black
                    IsHitTestVisible = true // Block underlying UI
                };

                var cardBorder = new Border
                {
                    Background = (System.Windows.Media.Brush)Application.Current.TryFindResource("OneUiControlBackgroundBrush") ?? System.Windows.Media.Brushes.White,
                    CornerRadius = new CornerRadius(24),
                    Padding = new Thickness(32),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                cardBorder.SetResourceReference(UIElement.EffectProperty, "OneUiModalShadow");

                var stackPanel = new StackPanel { Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Center };
                
                var progressRing = new SamsungProgressBar 
                { 
                    IsRing = true,
                    IsIndeterminate = true,
                    Width = 48, 
                    Height = 48, 
                    Margin = new Thickness(0, 0, 0, 16)
                };
                
                var textBlock = new TextBlock
                {
                    Text = message,
                    FontSize = (double)(Application.Current.TryFindResource("OneUiSubtitleFontSize") ?? 16.0),
                    FontWeight = FontWeights.SemiBold,
                    Foreground = (System.Windows.Media.Brush)Application.Current.TryFindResource("OneUiTextPrimaryBrush") ?? System.Windows.Media.Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                stackPanel.Children.Add(progressRing);
                stackPanel.Children.Add(textBlock);
                cardBorder.Child = stackPanel;
                overlayGrid.Children.Add(cardBorder);

                _currentAdorner = new InternalBusyAdorner(rootElement, overlayGrid);
                adornerLayer.Add(_currentAdorner);
                
                // Fade in animation
                var fadeIn = new System.Windows.Media.Animation.DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250));
                overlayGrid.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            });
        }

        public static void Hide()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (_currentAdorner == null) return;
                
                var adornerLayer = System.Windows.Documents.AdornerLayer.GetAdornerLayer(_currentAdorner.AdornedElement);
                if (adornerLayer != null)
                {
                    var overlayGrid = (Grid)((InternalBusyAdorner)_currentAdorner).Child;
                    var fadeOut = new System.Windows.Media.Animation.DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
                    fadeOut.Completed += (s, e) => 
                    {
                        if (_currentAdorner != null)
                        {
                            adornerLayer.Remove(_currentAdorner);
                            _currentAdorner = null;
                        }
                    };
                    overlayGrid.BeginAnimation(UIElement.OpacityProperty, fadeOut);
                }
                else
                {
                    _currentAdorner = null;
                }
            });
        }

        // --- Nested Classes ---

        private class InternalBusyAdorner : System.Windows.Documents.Adorner
        {
            // --- Properties ---
            public UIElement Child { get; }

            // --- Constructors ---
            public InternalBusyAdorner(UIElement adornedElement, UIElement child) : base(adornedElement)
            {
                Child = child;
                AddVisualChild(Child);
            }

            // --- Overrides ---
            protected override int VisualChildrenCount => 1;
            protected override System.Windows.Media.Visual GetVisualChild(int index) => Child;

            protected override Size MeasureOverride(Size constraint)
            {
                Child.Measure(AdornedElement.RenderSize);
                return AdornedElement.RenderSize;
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                Child.Arrange(new Rect(new Point(0, 0), AdornedElement.RenderSize));
                return AdornedElement.RenderSize;
            }
        }
    }
}
