using System.Windows;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Small gallery grid item. Renders the image directly via OnRender with rounded corners.
    /// </summary>
    public class SamsungGridViewSmall : FrameworkElement
    {
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(SamsungGridViewSmall),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double width = double.IsInfinity(availableSize.Width) ? 100 : availableSize.Width;
            double height = double.IsInfinity(availableSize.Height) ? 100 : availableSize.Height;
            return new Size(width, System.Math.Max(height, 100));
        }

        protected override void OnRender(DrawingContext dc)
        {
            var rect = new Rect(0, 0, ActualWidth, ActualHeight);
            var clip = new RectangleGeometry(rect, 20, 20);

            dc.DrawRoundedRectangle(GetBackground(), null, rect, 20, 20);

            if (ImageSource != null)
            {
                dc.PushClip(clip);
                dc.DrawImage(ImageSource, GetFillRect(ImageSource, rect));
                dc.Pop();
            }
        }

        private Brush GetBackground()
        {
            return TryFindResource("OneUiControlBackgroundBrush") as Brush
                ?? new SolidColorBrush(Color.FromRgb(240, 240, 240));
        }

        private static Rect GetFillRect(ImageSource src, Rect bounds)
        {
            if (src.Width <= 0 || src.Height <= 0 || bounds.Width <= 0 || bounds.Height <= 0)
                return bounds;

            double srcAspect = src.Width / src.Height;
            double dstAspect = bounds.Width / bounds.Height;

            if (srcAspect > dstAspect)
            {
                double w = bounds.Height * srcAspect;
                return new Rect(bounds.X + (bounds.Width - w) / 2, bounds.Y, w, bounds.Height);
            }
            else
            {
                double h = bounds.Width / srcAspect;
                return new Rect(bounds.X, bounds.Y + (bounds.Height - h) / 2, bounds.Width, h);
            }
        }
    }
}
