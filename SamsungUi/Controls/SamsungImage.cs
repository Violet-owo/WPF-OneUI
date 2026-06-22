using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style Image that supports rounded corners.
    /// </summary>
    public class SamsungImage : Control
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(SamsungImage), new PropertyMetadata(null));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(SamsungImage), new PropertyMetadata(new CornerRadius(16)));

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(SamsungImage), new PropertyMetadata(Stretch.UniformToFill));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        static SamsungImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungImage), new FrameworkPropertyMetadata(typeof(SamsungImage)));
        }
    }
}
