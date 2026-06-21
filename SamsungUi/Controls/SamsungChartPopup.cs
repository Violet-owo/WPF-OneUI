using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SamsungUi.Models;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A reusable floating popup card that shows ChartSegment detail data.
    /// Drop one inside any chart control template and bind <see cref="SelectedSegment"/>
    /// and <see cref="IsOpen"/>; the card will appear at the mouse cursor position
    /// with Samsung One UI styling (glass card, coloured accent strip, scale-in animation).
    ///
    /// Usage in any ControlTemplate:
    ///   &lt;sui:SamsungChartPopup
    ///       SelectedSegment="{Binding SelectedSegment, RelativeSource={RelativeSource TemplatedParent}}"
    ///       IsOpen="{Binding IsPopupOpen, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}"
    ///       ValueUnit="{TemplateBinding ValueUnit}"/&gt;
    /// </summary>
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class SamsungChartPopup : Control
    {
        static SamsungChartPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SamsungChartPopup),
                new FrameworkPropertyMetadata(typeof(SamsungChartPopup)));
        }

        private Popup? _popup;

        // --- Dependency Properties ---

        /// <summary>The segment whose data is displayed inside the popup card.</summary>
        public static readonly DependencyProperty SelectedSegmentProperty =
            DependencyProperty.Register(
                nameof(SelectedSegment),
                typeof(ChartSegment),
                typeof(SamsungChartPopup),
                new PropertyMetadata(null));

        public ChartSegment? SelectedSegment
        {
            get => (ChartSegment?)GetValue(SelectedSegmentProperty);
            set => SetValue(SelectedSegmentProperty, value);
        }

        /// <summary>
        /// Controls whether the popup is visible.
        /// Set to <c>true</c> to open, <c>false</c> to close.
        /// Automatically resets to <c>false</c> when the user clicks outside (StaysOpen=False).
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(
                nameof(IsOpen),
                typeof(bool),
                typeof(SamsungChartPopup),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnIsOpenChanged));

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungChartPopup ctrl && ctrl._popup != null)
                ctrl._popup.IsOpen = (bool)e.NewValue;
        }

        /// <summary>Unit label shown next to the segment value (e.g. "s", "min", "km").</summary>
        public static readonly DependencyProperty ValueUnitProperty =
            DependencyProperty.Register(
                nameof(ValueUnit),
                typeof(string),
                typeof(SamsungChartPopup),
                new PropertyMetadata(string.Empty));

        public string ValueUnit
        {
            get => (string)GetValue(ValueUnitProperty);
            set => SetValue(ValueUnitProperty, value);
        }

        // --- Methods ---

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Detach old popup
            if (_popup != null)
                _popup.Closed -= OnPopupClosed;

            _popup = GetTemplateChild("PART_Popup") as Popup;

            if (_popup != null)
            {
                // Make our own DPs (SelectedSegment, ValueUnit, IsOpen) available
                // to bindings inside the Popup's isolated visual tree.
                _popup.DataContext = this;

                _popup.Closed += OnPopupClosed;

                // Sync initial state
                _popup.IsOpen = IsOpen;
            }
        }

        // --- Event Handlers & Callbacks ---
        private void OnPopupClosed(object? sender, EventArgs e)
        {
            // When the user clicks outside (StaysOpen=False) we reset IsOpen
            // so that the next click opens the popup again.
            SetCurrentValue(IsOpenProperty, false);
        }
    }
}
