using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a list view item in a Samsung-style gallery, containing an image, main text, and sub text.
    /// </summary>
    public class SamsungListViewImageText : Control
    {
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(SamsungListViewImageText), new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.Register(nameof(MainText), typeof(string), typeof(SamsungListViewImageText), new PropertyMetadata(string.Empty));

        public string MainText
        {
            get => (string)GetValue(MainTextProperty);
            set => SetValue(MainTextProperty, value);
        }

        public static readonly DependencyProperty SubTextProperty =
            DependencyProperty.Register(nameof(SubText), typeof(string), typeof(SamsungListViewImageText), new PropertyMetadata(string.Empty));

        public string SubText
        {
            get => (string)GetValue(SubTextProperty);
            set => SetValue(SubTextProperty, value);
        }

        static SamsungListViewImageText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungListViewImageText), new FrameworkPropertyMetadata(typeof(SamsungListViewImageText)));
        }
    }
}
