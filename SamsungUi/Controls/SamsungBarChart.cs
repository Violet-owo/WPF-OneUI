using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a dynamic bar chart control.
    /// Inherits from ListBox to provide built-in item selection, hover, and collection management.
    /// Use with ChartSegment models.
    /// </summary>
    public class SamsungBarChart : ListBox
    {
        static SamsungBarChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungBarChart), new FrameworkPropertyMetadata(typeof(SamsungBarChart)));
        }

        public SamsungBarChart()
        {
            // Default styling
            Background = System.Windows.Media.Brushes.Transparent;
            BorderThickness = new Thickness(0);
            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
            ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
        }

        /// <summary>
        /// Identifies the <see cref="ValueUnit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueUnitProperty =
            DependencyProperty.Register(nameof(ValueUnit), typeof(string), typeof(SamsungBarChart), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the unit of measurement text to display alongside the bar values (e.g., "%", "GB", "hrs").
        /// </summary>
        public string ValueUnit
        {
            get => (string)GetValue(ValueUnitProperty);
            set => SetValue(ValueUnitProperty, value);
        }
    }
}
