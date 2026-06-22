using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a smooth curve line chart control. Supports up to two series.
    /// Features interactive tooltips and X-axis labels that react to hover.
    /// </summary>
    public class SamsungLineChart : Control
    {
        // --- Initialization ---
        static SamsungLineChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungLineChart), new FrameworkPropertyMetadata(typeof(SamsungLineChart)));
        }

        // --- Dependency Properties ---

        public static readonly DependencyProperty Series1Property =
            DependencyProperty.Register(nameof(Series1), typeof(IEnumerable<double>), typeof(SamsungLineChart),
                new PropertyMetadata(null, OnDataChanged));

        public IEnumerable<double>? Series1
        {
            get => (IEnumerable<double>?)GetValue(Series1Property);
            set => SetValue(Series1Property, value);
        }

        public static readonly DependencyProperty Series2Property =
            DependencyProperty.Register(nameof(Series2), typeof(IEnumerable<double>), typeof(SamsungLineChart),
                new PropertyMetadata(null, OnDataChanged));

        public IEnumerable<double>? Series2
        {
            get => (IEnumerable<double>?)GetValue(Series2Property);
            set => SetValue(Series2Property, value);
        }

        public static readonly DependencyProperty XAxisLabelsProperty =
            DependencyProperty.Register(nameof(XAxisLabels), typeof(IEnumerable<string>), typeof(SamsungLineChart),
                new PropertyMetadata(null, OnDataChanged));

        public IEnumerable<string>? XAxisLabels
        {
            get => (IEnumerable<string>?)GetValue(XAxisLabelsProperty);
            set => SetValue(XAxisLabelsProperty, value);
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(SamsungLineChart),
                new PropertyMetadata(9, OnSelectedIndexChanged));

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public static readonly DependencyProperty MaxYValueProperty =
            DependencyProperty.Register(nameof(MaxYValue), typeof(double), typeof(SamsungLineChart),
                new PropertyMetadata(300.0, OnDataChanged));

        public double MaxYValue
        {
            get => (double)GetValue(MaxYValueProperty);
            set => SetValue(MaxYValueProperty, value);
        }

        public static readonly DependencyProperty MinYValueProperty =
            DependencyProperty.Register(nameof(MinYValue), typeof(double), typeof(SamsungLineChart),
                new PropertyMetadata(0.0, OnDataChanged));

        public double MinYValue
        {
            get => (double)GetValue(MinYValueProperty);
            set => SetValue(MinYValueProperty, value);
        }

        public static readonly DependencyProperty Series1NameProperty =
            DependencyProperty.Register(nameof(Series1Name), typeof(string), typeof(SamsungLineChart),
                new PropertyMetadata("Series 1"));

        public string Series1Name
        {
            get => (string)GetValue(Series1NameProperty);
            set => SetValue(Series1NameProperty, value);
        }

        public static readonly DependencyProperty Series2NameProperty =
            DependencyProperty.Register(nameof(Series2Name), typeof(string), typeof(SamsungLineChart),
                new PropertyMetadata("Series 2"));

        public string Series2Name
        {
            get => (string)GetValue(Series2NameProperty);
            set => SetValue(Series2NameProperty, value);
        }

        public static readonly DependencyProperty Series1UnitProperty =
            DependencyProperty.Register(nameof(Series1Unit), typeof(string), typeof(SamsungLineChart),
                new PropertyMetadata(""));

        public string Series1Unit
        {
            get => (string)GetValue(Series1UnitProperty);
            set => SetValue(Series1UnitProperty, value);
        }

        public static readonly DependencyProperty Series2UnitProperty =
            DependencyProperty.Register(nameof(Series2Unit), typeof(string), typeof(SamsungLineChart),
                new PropertyMetadata(""));

        public string Series2Unit
        {
            get => (string)GetValue(Series2UnitProperty);
            set => SetValue(Series2UnitProperty, value);
        }

        // Output read-only geometries and properties for Template Binding

        public static readonly DependencyProperty Series1GeometryProperty =
            DependencyProperty.Register(nameof(Series1Geometry), typeof(Geometry), typeof(SamsungLineChart),
                new PropertyMetadata(Geometry.Empty));

        public Geometry Series1Geometry
        {
            get => (Geometry)GetValue(Series1GeometryProperty);
            set => SetValue(Series1GeometryProperty, value);
        }

        public static readonly DependencyProperty Series2GeometryProperty =
            DependencyProperty.Register(nameof(Series2Geometry), typeof(Geometry), typeof(SamsungLineChart),
                new PropertyMetadata(Geometry.Empty));

        public Geometry Series2Geometry
        {
            get => (Geometry)GetValue(Series2GeometryProperty);
            set => SetValue(Series2GeometryProperty, value);
        }

        public static readonly DependencyProperty Series1AreaGeometryProperty =
            DependencyProperty.Register(nameof(Series1AreaGeometry), typeof(Geometry), typeof(SamsungLineChart),
                new PropertyMetadata(Geometry.Empty));

        public Geometry Series1AreaGeometry
        {
            get => (Geometry)GetValue(Series1AreaGeometryProperty);
            set => SetValue(Series1AreaGeometryProperty, value);
        }

        public static readonly DependencyProperty Series2AreaGeometryProperty =
            DependencyProperty.Register(nameof(Series2AreaGeometry), typeof(Geometry), typeof(SamsungLineChart),
                new PropertyMetadata(Geometry.Empty));

        public Geometry Series2AreaGeometry
        {
            get => (Geometry)GetValue(Series2AreaGeometryProperty);
            set => SetValue(Series2AreaGeometryProperty, value);
        }

        public static readonly DependencyProperty SelectedPoint1Property =
            DependencyProperty.Register(nameof(SelectedPoint1), typeof(Point), typeof(SamsungLineChart),
                new PropertyMetadata(new Point(0, 0)));

        public Point SelectedPoint1
        {
            get => (Point)GetValue(SelectedPoint1Property);
            set => SetValue(SelectedPoint1Property, value);
        }

        public static readonly DependencyProperty SelectedPoint2Property =
            DependencyProperty.Register(nameof(SelectedPoint2), typeof(Point), typeof(SamsungLineChart),
                new PropertyMetadata(new Point(0, 0)));

        public Point SelectedPoint2
        {
            get => (Point)GetValue(SelectedPoint2Property);
            set => SetValue(SelectedPoint2Property, value);
        }

        public static readonly DependencyProperty SelectedPointVisibilityProperty =
            DependencyProperty.Register(nameof(SelectedPointVisibility), typeof(Visibility), typeof(SamsungLineChart),
                new PropertyMetadata(Visibility.Collapsed));

        public Visibility SelectedPointVisibility
        {
            get => (Visibility)GetValue(SelectedPointVisibilityProperty);
            set => SetValue(SelectedPointVisibilityProperty, value);
        }

        public static readonly DependencyProperty TooltipXProperty =
            DependencyProperty.Register(nameof(TooltipX), typeof(double), typeof(SamsungLineChart),
                new PropertyMetadata(0.0));

        public double TooltipX
        {
            get => (double)GetValue(TooltipXProperty);
            set => SetValue(TooltipXProperty, value);
        }

        public static readonly DependencyProperty TooltipYProperty =
            DependencyProperty.Register(nameof(TooltipY), typeof(double), typeof(SamsungLineChart),
                new PropertyMetadata(0.0));

        public double TooltipY
        {
            get => (double)GetValue(TooltipYProperty);
            set => SetValue(TooltipYProperty, value);
        }

        public static readonly DependencyProperty SelectedSeries1TextProperty =
            DependencyProperty.Register(nameof(SelectedSeries1Text), typeof(string), typeof(SamsungLineChart),
                new PropertyMetadata(string.Empty));

        public string SelectedSeries1Text
        {
            get => (string)GetValue(SelectedSeries1TextProperty);
            set => SetValue(SelectedSeries1TextProperty, value);
        }

        public static readonly DependencyProperty SelectedSeries2TextProperty =
            DependencyProperty.Register(nameof(SelectedSeries2Text), typeof(string), typeof(SamsungLineChart),
                new PropertyMetadata(string.Empty));

        public string SelectedSeries2Text
        {
            get => (string)GetValue(SelectedSeries2TextProperty);
            set => SetValue(SelectedSeries2TextProperty, value);
        }

        public static readonly DependencyProperty XAxisPillsProperty =
            DependencyProperty.Register(nameof(XAxisPills), typeof(IEnumerable<ChartXAxisLabel>), typeof(SamsungLineChart),
                new PropertyMetadata(null));

        public IEnumerable<ChartXAxisLabel>? XAxisPills
        {
            get => (IEnumerable<ChartXAxisLabel>?)GetValue(XAxisPillsProperty);
            set => SetValue(XAxisPillsProperty, value);
        }

        // --- Event Handlers & Callbacks ---

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungLineChart chart)
            {
                chart.UpdateGeometries();
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungLineChart chart)
            {
                chart.UpdateSelectedIndex();
            }
        }

        // --- Methods ---

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateGeometries();
        }

        protected override void OnPreviewMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            SelectClosestPoint(e.GetPosition(this));
        }

        private void SelectClosestPoint(Point mousePos)
        {
            if (_s1Points == null || _s1Points.Count == 0) return;

            double minDistance = double.MaxValue;
            int closestIdx = -1;

            for (int i = 0; i < _s1Points.Count; i++)
            {
                double dist = Math.Abs(_s1Points[i].X - mousePos.X);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestIdx = i;
                }
            }

            if (closestIdx >= 0 && closestIdx < _s1Points.Count)
            {
                SelectedIndex = closestIdx;
            }
        }

        // --- Fields ---
        private List<Point> _s1Points = new();
        private List<Point> _s2Points = new();

        private void UpdateGeometries()
        {
            if (ActualWidth <= 0 || ActualHeight <= 0)
                return;

            var s1 = Series1?.ToList();
            var s2 = Series2?.ToList();

            if (s1 == null || s1.Count == 0)
            {
                Series1Geometry = Geometry.Empty;
                Series1AreaGeometry = Geometry.Empty;
                Series2Geometry = Geometry.Empty;
                Series2AreaGeometry = Geometry.Empty;
                return;
            }

            // Define Layout Margins
            double leftMargin = 20;
            double rightMargin = 50; // space for Y axis
            double topMargin = 50;   // space for tooltip
            double bottomMargin = 40; // space for X axis

            double drawWidth = ActualWidth - leftMargin - rightMargin;
            double drawHeight = ActualHeight - topMargin - bottomMargin;

            if (drawWidth <= 0 || drawHeight <= 0)
                return;

            int count = s1.Count;
            double xStep = drawWidth / Math.Max(1, count - 1);

            double maxY = MaxYValue;
            double minY = MinYValue;
            double yRange = maxY - minY;
            if (yRange <= 0) yRange = 1.0;

            _s1Points.Clear();
            for (int i = 0; i < count; i++)
            {
                double x = leftMargin + i * xStep;
                double val = s1[i];
                double y = topMargin + drawHeight - ((val - minY) / yRange) * drawHeight;
                _s1Points.Add(new Point(x, y));
            }

            _s2Points.Clear();
            if (s2 != null && s2.Count > 0)
            {
                int s2Count = s2.Count;
                double s2xStep = drawWidth / Math.Max(1, s2Count - 1);
                for (int i = 0; i < s2Count; i++)
                {
                    double x = leftMargin + i * s2xStep;
                    double val = s2[i];
                    double y = topMargin + drawHeight - ((val - minY) / yRange) * drawHeight;
                    _s2Points.Add(new Point(x, y));
                }
            }

            // Calculate geometries
            double areaHeight = topMargin + drawHeight;
            Series1Geometry = CalculateSmoothPath(_s1Points, areaHeight, false);
            Series1AreaGeometry = CalculateSmoothPath(_s1Points, areaHeight, true);

            if (_s2Points.Count > 0)
            {
                Series2Geometry = CalculateSmoothPath(_s2Points, areaHeight, false);
                Series2AreaGeometry = CalculateSmoothPath(_s2Points, areaHeight, true);
            }
            else
            {
                Series2Geometry = Geometry.Empty;
                Series2AreaGeometry = Geometry.Empty;
            }

            // Generate X Axis Pills/Labels
            var xLabels = XAxisLabels?.ToList();
            if (xLabels != null)
            {
                var pills = new List<ChartXAxisLabel>();
                for (int i = 0; i < xLabels.Count; i++)
                {
                    double xPos = leftMargin + i * (drawWidth / Math.Max(1, xLabels.Count - 1));
                    pills.Add(new ChartXAxisLabel
                    {
                        Text = xLabels[i],
                        XPosition = xPos,
                        IsSelected = i == SelectedIndex,
                        Index = i
                    });
                }
                XAxisPills = pills;
            }

            UpdateSelectedIndex();
        }

        private void UpdateSelectedIndex()
        {
            int idx = SelectedIndex;
            var s1 = Series1?.ToList();
            var s2 = Series2?.ToList();

            if (idx >= 0 && s1 != null && idx < s1.Count && _s1Points.Count > idx)
            {
                SelectedPoint1 = _s1Points[idx];
                SelectedSeries1Text = $"{s1[idx]} {Series1Unit}";

                if (s2 != null && idx < s2.Count && _s2Points.Count > idx)
                {
                    SelectedPoint2 = _s2Points[idx];
                    SelectedSeries2Text = $"{s2[idx]} {Series2Unit}";
                }
                else
                {
                    SelectedPoint2 = new Point(0, 0);
                    SelectedSeries2Text = string.Empty;
                }

                TooltipX = SelectedPoint1.X;
                // Align tooltip above the highest point of the two
                double highestY = SelectedPoint1.Y;
                if (s2 != null && idx < s2.Count && _s2Points.Count > idx)
                {
                    highestY = Math.Min(SelectedPoint1.Y, SelectedPoint2.Y);
                }
                TooltipY = highestY;

                SelectedPointVisibility = Visibility.Visible;
            }
            else
            {
                SelectedPointVisibility = Visibility.Collapsed;
            }

            // Sync X axis selection state
            if (XAxisPills != null)
            {
                foreach (var pill in XAxisPills)
                {
                    pill.IsSelected = pill.Index == idx;
                }
            }
        }

        private static Geometry CalculateSmoothPath(IList<Point> points, double height, bool isArea)
        {
            if (points == null || points.Count == 0)
                return Geometry.Empty;

            var geometry = new PathGeometry();
            var figure = new PathFigure { StartPoint = points[0] };

            if (points.Count == 1)
            {
                if (isArea)
                {
                    figure.Segments.Add(new LineSegment(new Point(points[0].X, height), true));
                    figure.Segments.Add(new LineSegment(new Point(points[0].X, height), false));
                    figure.IsClosed = true;
                }
                geometry.Figures.Add(figure);
                return geometry;
            }

            double tension = 0.15; // smooth factor

            for (int i = 0; i < points.Count - 1; i++)
            {
                Point p0 = i == 0 ? points[i] : points[i - 1];
                Point p1 = points[i];
                Point p2 = points[i + 1];
                Point p3 = i + 2 >= points.Count ? points[i + 1] : points[i + 2];

                double cp1X = p1.X + (p2.X - p0.X) * tension;
                double cp1Y = p1.Y + (p2.Y - p0.Y) * tension;

                double cp2X = p2.X - (p3.X - p1.X) * tension;
                double cp2Y = p2.Y - (p3.Y - p1.Y) * tension;

                figure.Segments.Add(new BezierSegment(new Point(cp1X, cp1Y), new Point(cp2X, cp2Y), p2, true));
            }

            if (isArea)
            {
                figure.Segments.Add(new LineSegment(new Point(points[points.Count - 1].X, height), true));
                figure.Segments.Add(new LineSegment(new Point(points[0].X, height), true));
                figure.IsClosed = true;
            }
            else
            {
                figure.IsClosed = false;
            }

            geometry.Figures.Add(figure);
            return geometry;
        }
    }

    public class ChartXAxisLabel : DependencyObject
    {
        public string Text { get; set; } = string.Empty;
        public double XPosition { get; set; }
        public int Index { get; set; }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(ChartXAxisLabel),
                new PropertyMetadata(false));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
    }
}
