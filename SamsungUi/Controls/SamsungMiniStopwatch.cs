using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    [TemplatePart(Name = "PART_MiniThumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_MiniStartButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_MiniLapButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_MiniCloseButton", Type = typeof(ButtonBase))]
    public class SamsungMiniStopwatch : Control
    {
        // --- Dependency Properties ---

        public static readonly DependencyProperty ElapsedTimeTextProperty =
            DependencyProperty.Register(nameof(ElapsedTimeText), typeof(string), typeof(SamsungMiniStopwatch), new PropertyMetadata("00:00.00"));
        public string ElapsedTimeText
        {
            get => (string)GetValue(ElapsedTimeTextProperty);
            set => SetValue(ElapsedTimeTextProperty, value);
        }

        public static readonly DependencyProperty StartButtonTextProperty =
            DependencyProperty.Register(nameof(StartButtonText), typeof(string), typeof(SamsungMiniStopwatch), new PropertyMetadata("Start"));
        public string StartButtonText
        {
            get => (string)GetValue(StartButtonTextProperty);
            set => SetValue(StartButtonTextProperty, value);
        }

        public static readonly DependencyProperty LapButtonTextProperty =
            DependencyProperty.Register(nameof(LapButtonText), typeof(string), typeof(SamsungMiniStopwatch), new PropertyMetadata("Lap"));
        public string LapButtonText
        {
            get => (string)GetValue(LapButtonTextProperty);
            set => SetValue(LapButtonTextProperty, value);
        }

        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register(nameof(IsRunning), typeof(bool), typeof(SamsungMiniStopwatch), new PropertyMetadata(false));
        public bool IsRunning
        {
            get => (bool)GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }

        // --- Routed Events ---

        public static readonly RoutedEvent StartStopClickEvent = EventManager.RegisterRoutedEvent(
            nameof(StartStopClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SamsungMiniStopwatch));
        public event RoutedEventHandler StartStopClick
        {
            add { AddHandler(StartStopClickEvent, value); }
            remove { RemoveHandler(StartStopClickEvent, value); }
        }

        public static readonly RoutedEvent LapResetClickEvent = EventManager.RegisterRoutedEvent(
            nameof(LapResetClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SamsungMiniStopwatch));
        public event RoutedEventHandler LapResetClick
        {
            add { AddHandler(LapResetClickEvent, value); }
            remove { RemoveHandler(LapResetClickEvent, value); }
        }

        public static readonly RoutedEvent CloseClickEvent = EventManager.RegisterRoutedEvent(
            nameof(CloseClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SamsungMiniStopwatch));
        public event RoutedEventHandler CloseClick
        {
            add { AddHandler(CloseClickEvent, value); }
            remove { RemoveHandler(CloseClickEvent, value); }
        }

        public static readonly RoutedEvent DragDeltaEvent = EventManager.RegisterRoutedEvent(
            nameof(DragDelta), RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(SamsungMiniStopwatch));
        public event DragDeltaEventHandler DragDelta
        {
            add { AddHandler(DragDeltaEvent, value); }
            remove { RemoveHandler(DragDeltaEvent, value); }
        }

        // --- Initialization ---

        static SamsungMiniStopwatch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungMiniStopwatch), new FrameworkPropertyMetadata(typeof(SamsungMiniStopwatch)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_MiniThumb") is Thumb thumb)
            {
                thumb.DragDelta += (s, e) => RaiseEvent(new DragDeltaEventArgs(e.HorizontalChange, e.VerticalChange) { RoutedEvent = DragDeltaEvent });
            }

            if (GetTemplateChild("PART_MiniStartButton") is ButtonBase startBtn)
            {
                startBtn.Click += (s, e) => RaiseEvent(new RoutedEventArgs(StartStopClickEvent));
            }

            if (GetTemplateChild("PART_MiniLapButton") is ButtonBase lapBtn)
            {
                lapBtn.Click += (s, e) => RaiseEvent(new RoutedEventArgs(LapResetClickEvent));
            }

            if (GetTemplateChild("PART_MiniCloseButton") is ButtonBase closeBtn)
            {
                closeBtn.Click += (s, e) => RaiseEvent(new RoutedEventArgs(CloseClickEvent));
            }
        }
    }
}
