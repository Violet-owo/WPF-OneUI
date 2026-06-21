using System.Windows;
using System.Windows.Media;

namespace SamsungUi.Models
{
    public class ChartSegment : DependencyObject
    {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(ChartSegment), new PropertyMetadata(string.Empty));

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(ChartSegment), new PropertyMetadata(0.0));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register(nameof(Brush), typeof(Brush), typeof(ChartSegment), new PropertyMetadata(Brushes.Transparent));

        public Brush Brush
        {
            get => (Brush)GetValue(BrushProperty);
            set => SetValue(BrushProperty, value);
        }

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register(nameof(Percentage), typeof(double), typeof(ChartSegment), new PropertyMetadata(0.0));

        public double Percentage
        {
            get => (double)GetValue(PercentageProperty);
            set => SetValue(PercentageProperty, value);
        }
    }
}
