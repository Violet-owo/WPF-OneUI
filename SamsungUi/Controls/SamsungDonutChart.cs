    using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SamsungUi.Models;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a donut-style pie chart control.
    /// Displays proportional ring segments with hover effects and interactive tooltips.
    /// </summary>
    [TemplatePart(Name = "PART_DonutCanvas", Type = typeof(Canvas))]
    public class SamsungDonutChart : Control
    {
        // --- Initialization ---
        static SamsungDonutChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungDonutChart), new FrameworkPropertyMetadata(typeof(SamsungDonutChart)));
        }

        // --- Fields ---
        private Canvas? _donutCanvas;

        // --- Dependency Properties ---

        public static readonly DependencyProperty SegmentsProperty =
            DependencyProperty.Register(nameof(Segments), typeof(IEnumerable<ChartSegment>), typeof(SamsungDonutChart),
                new PropertyMetadata(null, OnSegmentsChanged));

        public IEnumerable<ChartSegment>? Segments
        {
            get => (IEnumerable<ChartSegment>?)GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }

        // --- Properties ---
        public static readonly DependencyProperty CenterTextProperty =
            DependencyProperty.Register(nameof(CenterText), typeof(string), typeof(SamsungDonutChart),
                new PropertyMetadata(string.Empty));

        public string CenterText
        {
            get => (string)GetValue(CenterTextProperty);
            set => SetValue(CenterTextProperty, value);
        }

        public static readonly DependencyProperty ValueUnitProperty =
            DependencyProperty.Register(nameof(ValueUnit), typeof(string), typeof(SamsungDonutChart),
                new PropertyMetadata(string.Empty));

        public string ValueUnit
        {
            get => (string)GetValue(ValueUnitProperty);
            set => SetValue(ValueUnitProperty, value);
        }

        public static readonly DependencyProperty CenterSubtextProperty =
            DependencyProperty.Register(nameof(CenterSubtext), typeof(string), typeof(SamsungDonutChart),
                new PropertyMetadata(string.Empty));

        public string CenterSubtext
        {
            get => (string)GetValue(CenterSubtextProperty);
            set => SetValue(CenterSubtextProperty, value);
        }

        /// <summary>The segment last clicked by the user. Drives the popup content.</summary>
        public static readonly DependencyProperty SelectedSegmentProperty =
            DependencyProperty.Register(nameof(SelectedSegment), typeof(ChartSegment), typeof(SamsungDonutChart),
                new PropertyMetadata(null));

        public ChartSegment? SelectedSegment
        {
            get => (ChartSegment?)GetValue(SelectedSegmentProperty);
            set => SetValue(SelectedSegmentProperty, value);
        }

        /// <summary>Controls whether the segment detail popup is currently visible.</summary>
        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register(nameof(IsPopupOpen), typeof(bool), typeof(SamsungDonutChart),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsPopupOpen
        {
            get => (bool)GetValue(IsPopupOpenProperty);
            set => SetValue(IsPopupOpenProperty, value);
        }

        // --- Event Handlers & Callbacks ---
        private static void OnSegmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungDonutChart chart)
            {
                if (e.OldValue is INotifyCollectionChanged oldColl)
                {
                    oldColl.CollectionChanged -= chart.OnSegmentsCollectionChanged;
                }
                if (e.NewValue is INotifyCollectionChanged newColl)
                {
                    newColl.CollectionChanged += chart.OnSegmentsCollectionChanged;
                }
                chart.UpdateChart();
            }
        }

        private void OnSegmentsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateChart();
        }

        // --- Methods ---
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _donutCanvas = GetTemplateChild("PART_DonutCanvas") as Canvas;
            UpdateChart();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateChart();
        }

        private void UpdateChart()
        {
            if (_donutCanvas == null) return;

            _donutCanvas.Children.Clear();

            var list = Segments?.ToList();
            if (list == null || list.Count == 0) return;

            double containerSize = Math.Min(_donutCanvas.ActualWidth, _donutCanvas.ActualHeight);
            if (containerSize <= 0) containerSize = 160; // fallback default size

            double center = containerSize / 2;
            double outerRadius = center * 0.95;
            double innerRadius = outerRadius * 0.75; // donut cutout ratio

            double total = list.Sum(s => s.Value);
            if (total <= 0) return;

            double currentAngle = 0;

            foreach (var seg in list)
            {
                double sweep = (seg.Value / total) * 360.0;
                seg.Percentage = (seg.Value / total) * 100;

                // Adjust sweep to avoid full circle arc segment bugs
                double finalSweep = Math.Min(sweep, 359.99);

                var geom = CreateDonutSegmentGeometry(center, outerRadius, innerRadius, currentAngle, currentAngle + finalSweep);

                var capturedSeg = seg;
                var path = new Path
                {
                    Data   = geom,
                    Fill   = seg.Brush,
                    Cursor = Cursors.Hand
                };

                // Hover effect
                path.MouseEnter += (s, e) => path.Opacity = 0.80;
                path.MouseLeave += (s, e) => path.Opacity = 1.0;

                // Click → open popup
                path.MouseLeftButtonUp += (s, e) =>
                {
                    SelectedSegment = capturedSeg;
                    IsPopupOpen = false;
                    IsPopupOpen = true;
                    e.Handled = true;
                };

                _donutCanvas.Children.Add(path);

                currentAngle += sweep;
            }

            // Adjust Canvas Size to center it
            _donutCanvas.Width = containerSize;
            _donutCanvas.Height = containerSize;
        }

        private Geometry CreateDonutSegmentGeometry(double centerPoint, double outerRadius, double innerRadius, double startAngle, double endAngle)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure();

            // Convert degrees to radians (offset by -90 so 0 is top center)
            double startRad = (startAngle - 90) * Math.PI / 180.0;
            double endRad = (endAngle - 90) * Math.PI / 180.0;

            Point pStartOuter = new Point(
                centerPoint + outerRadius * Math.Cos(startRad),
                centerPoint + outerRadius * Math.Sin(startRad)
            );

            Point pEndOuter = new Point(
                centerPoint + outerRadius * Math.Cos(endRad),
                centerPoint + outerRadius * Math.Sin(endRad)
            );

            Point pStartInner = new Point(
                centerPoint + innerRadius * Math.Cos(startRad),
                centerPoint + innerRadius * Math.Sin(startRad)
            );

            Point pEndInner = new Point(
                centerPoint + innerRadius * Math.Cos(endRad),
                centerPoint + innerRadius * Math.Sin(endRad)
            );

            figure.StartPoint = pStartInner;
            figure.Segments.Add(new LineSegment(pStartOuter, true));

            bool isLargeArc = (endAngle - startAngle) > 180.0;
            figure.Segments.Add(new ArcSegment(
                pEndOuter,
                new Size(outerRadius, outerRadius),
                0,
                isLargeArc,
                SweepDirection.Clockwise,
                true
            ));

            figure.Segments.Add(new LineSegment(pEndInner, true));

            figure.Segments.Add(new ArcSegment(
                pStartInner,
                new Size(innerRadius, innerRadius),
                0,
                isLargeArc,
                SweepDirection.Counterclockwise,
                true
            ));

            figure.IsClosed = true;
            geometry.Figures.Add(figure);
            return geometry;
        }
    }
}
