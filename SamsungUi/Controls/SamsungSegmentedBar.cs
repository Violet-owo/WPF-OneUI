using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using SamsungUi.Models;

namespace SamsungUi.Controls
{
    [TemplatePart(Name = "PART_SegmentsContainer", Type = typeof(Panel))]
    public class SamsungSegmentedBar : Control
    {
        // --- Initialization ---

        static SamsungSegmentedBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungSegmentedBar), new FrameworkPropertyMetadata(typeof(SamsungSegmentedBar)));
        }

        // --- Fields ---

        private Panel? _segmentsContainer;
        private Popup? _popup;

        // --- Dependency Properties ---

        public static readonly DependencyProperty SegmentsProperty =
            DependencyProperty.Register(nameof(Segments), typeof(IEnumerable<ChartSegment>), typeof(SamsungSegmentedBar),
                new PropertyMetadata(null, OnSegmentsChanged));

        public IEnumerable<ChartSegment>? Segments
        {
            get => (IEnumerable<ChartSegment>?)GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }

        public static readonly DependencyProperty ValueUnitProperty =
            DependencyProperty.Register(nameof(ValueUnit), typeof(string), typeof(SamsungSegmentedBar),
                new PropertyMetadata("unit"));

        public string ValueUnit
        {
            get => (string)GetValue(ValueUnitProperty);
            set => SetValue(ValueUnitProperty, value);
        }

        public static readonly DependencyProperty TotalValueProperty =
            DependencyProperty.Register(nameof(TotalValue), typeof(double), typeof(SamsungSegmentedBar),
                new PropertyMetadata(0.0));

        public double TotalValue
        {
            get => (double)GetValue(TotalValueProperty);
            set => SetValue(TotalValueProperty, value);
        }

        public static readonly DependencyProperty TitleTextProperty =
            DependencyProperty.Register(nameof(TitleText), typeof(string), typeof(SamsungSegmentedBar),
                new PropertyMetadata(string.Empty));

        public string TitleText
        {
            get => (string)GetValue(TitleTextProperty);
            set => SetValue(TitleTextProperty, value);
        }

        public static readonly DependencyProperty DescriptionTextProperty =
            DependencyProperty.Register(nameof(DescriptionText), typeof(string), typeof(SamsungSegmentedBar),
                new PropertyMetadata(string.Empty));

        public string DescriptionText
        {
            get => (string)GetValue(DescriptionTextProperty);
            set => SetValue(DescriptionTextProperty, value);
        }

        /// <summary>
        /// The segment that was last clicked by the user. Drives the popup content.
        /// </summary>
        public static readonly DependencyProperty SelectedSegmentProperty =
            DependencyProperty.Register(nameof(SelectedSegment), typeof(ChartSegment), typeof(SamsungSegmentedBar),
                new PropertyMetadata(null));

        public ChartSegment? SelectedSegment
        {
            get => (ChartSegment?)GetValue(SelectedSegmentProperty);
            set => SetValue(SelectedSegmentProperty, value);
        }

        /// <summary>
        /// Controls whether the segment detail popup is currently visible.
        /// </summary>
        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register(nameof(IsPopupOpen), typeof(bool), typeof(SamsungSegmentedBar),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsPopupOpen
        {
            get => (bool)GetValue(IsPopupOpenProperty);
            set => SetValue(IsPopupOpenProperty, value);
        }

        // --- Event Handlers & Callbacks ---

        private static void OnSegmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungSegmentedBar bar)
            {
                if (e.OldValue is INotifyCollectionChanged oldColl)
                {
                    oldColl.CollectionChanged -= bar.OnSegmentsCollectionChanged;
                }
                if (e.NewValue is INotifyCollectionChanged newColl)
                {
                    newColl.CollectionChanged += bar.OnSegmentsCollectionChanged;
                }
                bar.UpdateSegments();
            }
        }

        private void OnSegmentsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateSegments();
        }

        // --- Methods ---

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _segmentsContainer = GetTemplateChild("PART_SegmentsContainer") as Panel;
            _popup = GetTemplateChild("PART_SegmentPopup") as Popup;
            UpdateSegments();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateSegmentSizes();
        }

        private void UpdateSegments()
        {
            var list = Segments?.ToList();
            if (list == null || list.Count == 0)
            {
                TotalValue = 0;
                if (_segmentsContainer != null) _segmentsContainer.Children.Clear();
                return;
            }

            double total = list.Sum(s => s.Value);
            TotalValue = total;

            foreach (var seg in list)
            {
                seg.Percentage = total > 0 ? (seg.Value / total) * 100 : 0;
            }

            if (_segmentsContainer == null) return;

            _segmentsContainer.Children.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                var seg = list[i];
                bool isFirst = (i == 0);
                bool isLast = (i == list.Count - 1);

                // Pill shape: first/last get outer rounded end; middle get small gap only
                var cornerRadius = new CornerRadius(
                    topLeft:     isFirst ? 9 : 0,
                    topRight:    isLast  ? 9 : 0,
                    bottomRight: isLast  ? 9 : 0,
                    bottomLeft:  isFirst ? 9 : 0
                );

                // Add a small gap between pills
                var margin = new Thickness(
                    left:   i == 0 ? 0 : 2,
                    top:    0,
                    right:  0,
                    bottom: 0
                );

                var capturedSeg = seg;
                var border = new Border
                {
                    Background   = seg.Brush,
                    CornerRadius = cornerRadius,
                    Height       = 18,
                    Margin       = margin,
                    Cursor       = Cursors.Hand
                };

                border.MouseLeftButtonUp += (s, e) =>
                {
                    SelectedSegment = capturedSeg;
                    IsPopupOpen = false; // force re-open if already open
                    IsPopupOpen = true;
                    e.Handled = true;
                };

                _segmentsContainer.Children.Add(border);
            }

            UpdateSegmentSizes();
        }

        private void UpdateSegmentSizes()
        {
            if (_segmentsContainer == null || Segments == null) return;

            var list = Segments.ToList();
            if (list.Count == 0) return;

            double containerWidth = _segmentsContainer.ActualWidth;
            if (containerWidth <= 0) return;

            double total = list.Sum(s => s.Value);
            if (total <= 0) return;

            // Subtract total gap widths (2px per gap, gaps = count - 1)
            double gapTotal = (list.Count - 1) * 2.0;
            double availableWidth = Math.Max(1, containerWidth - gapTotal);

            for (int i = 0; i < list.Count; i++)
            {
                if (i < _segmentsContainer.Children.Count && _segmentsContainer.Children[i] is Border border)
                {
                    double w = (list[i].Value / total) * availableWidth;
                    border.Width = Math.Max(0.5, w);
                }
            }
        }
    }
}
