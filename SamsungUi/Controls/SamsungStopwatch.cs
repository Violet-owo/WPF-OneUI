using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SamsungUi.Models;

namespace SamsungUi.Controls
{
    [TemplatePart(Name = "PART_MainDial", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_MiniStopwatchPopup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_MiniStopwatch", Type = typeof(SamsungMiniStopwatch))]
    [TemplatePart(Name = "PART_StartButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_LapButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_TogglePipButton", Type = typeof(ButtonBase))]
    public class SamsungStopwatch : Control
    {
        // --- Fields ---
        private DispatcherTimer _timer;
        private Stopwatch _stopwatch;
        private TimeSpan _lastLapTotalTime = TimeSpan.Zero;
        private int _lapCounter = 1;

        private FrameworkElement? _mainDial;
        private Popup? _miniPopup;
        private ScrollViewer? _parentScrollViewer;
        private bool _manuallyClosed = false;

        // Custom palette for charts
        private static readonly Brush[] Palette = new Brush[]
        {
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0381FE")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#52A1FF")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D1BCE3")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E3A8B1")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4DB6AC")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD54F")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8A65"))
        };

        // --- Dependency Properties ---

        public static readonly DependencyProperty ElapsedTimeTextProperty =
            DependencyProperty.Register(nameof(ElapsedTimeText), typeof(string), typeof(SamsungStopwatch), new PropertyMetadata("00:00.00"));
        public string ElapsedTimeText
        {
            get => (string)GetValue(ElapsedTimeTextProperty);
            set => SetValue(ElapsedTimeTextProperty, value);
        }

        public static readonly DependencyProperty NeedleAngleProperty =
            DependencyProperty.Register(nameof(NeedleAngle), typeof(double), typeof(SamsungStopwatch), new PropertyMetadata(0.0));
        public double NeedleAngle
        {
            get => (double)GetValue(NeedleAngleProperty);
            set => SetValue(NeedleAngleProperty, value);
        }

        public static readonly DependencyProperty StartButtonTextProperty =
            DependencyProperty.Register(nameof(StartButtonText), typeof(string), typeof(SamsungStopwatch), new PropertyMetadata("Start"));
        public string StartButtonText
        {
            get => (string)GetValue(StartButtonTextProperty);
            set => SetValue(StartButtonTextProperty, value);
        }

        public static readonly DependencyProperty LapButtonTextProperty =
            DependencyProperty.Register(nameof(LapButtonText), typeof(string), typeof(SamsungStopwatch), new PropertyMetadata("Lap"));
        public string LapButtonText
        {
            get => (string)GetValue(LapButtonTextProperty);
            set => SetValue(LapButtonTextProperty, value);
        }

        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register(nameof(IsRunning), typeof(bool), typeof(SamsungStopwatch), new PropertyMetadata(false));
        public bool IsRunning
        {
            get => (bool)GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }

        public static readonly DependencyProperty HasLapsProperty =
            DependencyProperty.Register(nameof(HasLaps), typeof(bool), typeof(SamsungStopwatch), new PropertyMetadata(false));
        public bool HasLaps
        {
            get => (bool)GetValue(HasLapsProperty);
            set => SetValue(HasLapsProperty, value);
        }

        public static readonly DependencyProperty IsPipVisibleProperty =
            DependencyProperty.Register(nameof(IsPipVisible), typeof(bool), typeof(SamsungStopwatch), new PropertyMetadata(false, OnIsPipVisibleChanged));
        public bool IsPipVisible
        {
            get => (bool)GetValue(IsPipVisibleProperty);
            set => SetValue(IsPipVisibleProperty, value);
        }

        private static void OnIsPipVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungStopwatch sw)
            {
                if ((bool)e.NewValue)
                    sw.ShowMiniPopup();
                else
                    sw.HideMiniPopup();
            }
        }

        public ObservableCollection<LapItem> Laps { get; } = new();
        public ObservableCollection<ChartSegment> LapChartSegments { get; } = new();

        // --- Initialization ---
        static SamsungStopwatch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungStopwatch), new FrameworkPropertyMetadata(typeof(SamsungStopwatch)));
        }

        public SamsungStopwatch()
        {
            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += Timer_Tick;
        }

        // --- Template ---
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Hook up buttons
            if (GetTemplateChild("PART_StartButton") is ButtonBase startBtn) startBtn.Click += StartStop_Click;
            if (GetTemplateChild("PART_LapButton") is ButtonBase lapBtn) lapBtn.Click += LapReset_Click;

            if (GetTemplateChild("PART_TogglePipButton") is ButtonBase togglePipBtn)
            {
                togglePipBtn.Click += (s, e) => 
                { 
                    if (IsPipVisible && _miniPopup != null && !_miniPopup.IsOpen)
                    {
                        // Forza la visualizzazione se lo stato è disallineato (es. dopo scaricamento pagina)
                        ShowMiniPopup();
                    }
                    else
                    {
                        IsPipVisible = true; 
                    }
                    _manuallyClosed = false;
                };
            }

            _mainDial = GetTemplateChild("PART_MainDial") as FrameworkElement;
            _miniPopup = GetTemplateChild("PART_MiniStopwatchPopup") as Popup;

            if (GetTemplateChild("PART_MiniStopwatch") is SamsungMiniStopwatch mini)
            {
                _miniStopwatch = mini;
                mini.StartStopClick += StartStop_Click;
                mini.LapResetClick += LapReset_Click;
                mini.CloseClick += (s, e) => 
                { 
                    IsPipVisible = false; 
                    _manuallyClosed = true;
                };
                mini.DragDelta += (s, e) => 
                {
                    if (_miniPopup != null)
                    {
                        _miniPopup.HorizontalOffset += e.HorizontalChange;
                        _miniPopup.VerticalOffset += e.VerticalChange;
                    }
                };
            }

            // Find parent ScrollViewer when loaded
            this.Loaded += (s, e) =>
            {
                _parentScrollViewer = FindParent<ScrollViewer>(this);
                if (_parentScrollViewer != null)
                {
                    _parentScrollViewer.ScrollChanged += ParentScrollViewer_ScrollChanged;
                }
            };
            
            this.Unloaded += (s, e) => 
            {
                IsPipVisible = false;
                if (_miniPopup != null)
                {
                    _miniPopup.IsOpen = false;
                }

                if (_parentScrollViewer != null)
                {
                    _parentScrollViewer.ScrollChanged -= ParentScrollViewer_ScrollChanged;
                }
            };
        }

        // --- Animations ---
        
        private SamsungMiniStopwatch? _miniStopwatch;

        private void ShowMiniPopup()
        {
            if (_miniPopup == null || _miniStopwatch == null) return;
            
            _miniPopup.IsOpen = true;

            var sb = new System.Windows.Media.Animation.Storyboard();
            
            var opacityAnim = new System.Windows.Media.Animation.DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250));
            System.Windows.Media.Animation.Storyboard.SetTarget(opacityAnim, _miniStopwatch);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(opacityAnim, new PropertyPath("Opacity"));

            _miniStopwatch.RenderTransform = new ScaleTransform(0.8, 0.8);
            _miniStopwatch.RenderTransformOrigin = new Point(0.5, 0.5);

            var scaleXAnim = new System.Windows.Media.Animation.DoubleAnimation(0.8, 1.0, TimeSpan.FromMilliseconds(350))
            {
                EasingFunction = new System.Windows.Media.Animation.BackEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut, Amplitude = 0.6 }
            };
            System.Windows.Media.Animation.Storyboard.SetTarget(scaleXAnim, _miniStopwatch);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("RenderTransform.ScaleX"));

            var scaleYAnim = new System.Windows.Media.Animation.DoubleAnimation(0.8, 1.0, TimeSpan.FromMilliseconds(350))
            {
                EasingFunction = new System.Windows.Media.Animation.BackEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut, Amplitude = 0.6 }
            };
            System.Windows.Media.Animation.Storyboard.SetTarget(scaleYAnim, _miniStopwatch);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("RenderTransform.ScaleY"));

            sb.Children.Add(opacityAnim);
            sb.Children.Add(scaleXAnim);
            sb.Children.Add(scaleYAnim);
            sb.Begin();
        }

        private void HideMiniPopup()
        {
            if (_miniPopup == null || _miniStopwatch == null || !_miniPopup.IsOpen) return;

            var sb = new System.Windows.Media.Animation.Storyboard();
            
            var opacityAnim = new System.Windows.Media.Animation.DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
            System.Windows.Media.Animation.Storyboard.SetTarget(opacityAnim, _miniStopwatch);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(opacityAnim, new PropertyPath("Opacity"));

            if (_miniStopwatch.RenderTransform is not ScaleTransform)
            {
                _miniStopwatch.RenderTransform = new ScaleTransform(1, 1);
                _miniStopwatch.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            var scaleXAnim = new System.Windows.Media.Animation.DoubleAnimation(1.0, 0.8, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseIn }
            };
            System.Windows.Media.Animation.Storyboard.SetTarget(scaleXAnim, _miniStopwatch);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("RenderTransform.ScaleX"));

            var scaleYAnim = new System.Windows.Media.Animation.DoubleAnimation(1.0, 0.8, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseIn }
            };
            System.Windows.Media.Animation.Storyboard.SetTarget(scaleYAnim, _miniStopwatch);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("RenderTransform.ScaleY"));

            sb.Children.Add(opacityAnim);
            sb.Children.Add(scaleXAnim);
            sb.Children.Add(scaleYAnim);

            sb.Completed += (s, e) => 
            {
                if (!IsPipVisible) _miniPopup.IsOpen = false;
            };
            
            sb.Begin();
        }

        // --- Methods ---
        
        private T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent) return parent;
            return FindParent<T>(parentObject);
        }

        private void ParentScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_mainDial == null || _parentScrollViewer == null) return;
            if (!IsRunning && Laps.Count == 0) return; // Non mostrare il PiP se il cronometro è intonso

            try
            {
                // Check if _mainDial is visible within _parentScrollViewer
                GeneralTransform transform = _mainDial.TransformToAncestor(_parentScrollViewer);
                Rect rect = transform.TransformBounds(new Rect(0, 0, _mainDial.ActualWidth, _mainDial.ActualHeight));

                // If the bottom of the dial is above the top of the scrollviewer (scrolled down past it)
                // or the top is below the bottom of the scrollviewer
                bool isOutOfView = rect.Bottom < 0 || rect.Top > _parentScrollViewer.ViewportHeight;

                if (isOutOfView)
                {
                    if (!_miniPopup!.IsOpen && !_manuallyClosed)
                    {
                        IsPipVisible = true;
                    }
                }
                else
                {
                    if (_miniPopup!.IsOpen)
                    {
                        IsPipVisible = false;
                    }
                    // Reset _manuallyClosed so it can automatically open next time it's out of view
                    _manuallyClosed = false;
                }
            }
            catch
            {
                // In caso di problemi nel layout tree, evita crash
            }
        }


        private void Timer_Tick(object? sender, EventArgs e)
        {
            var elapsed = _stopwatch.Elapsed;
            ElapsedTimeText = $"{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds / 10:D2}";
            
            // 6 degrees per second (60 seconds = 360 degrees)
            double secondsWithFraction = elapsed.TotalSeconds;
            NeedleAngle = (secondsWithFraction * 6) % 360;
        }

        private void StartStop_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRunning)
            {
                _stopwatch.Start();
                _timer.Start();
                IsRunning = true;
                StartButtonText = "Stop";
                LapButtonText = "Lap";
            }
            else
            {
                _stopwatch.Stop();
                _timer.Stop();
                IsRunning = false;
                StartButtonText = "Start";
                LapButtonText = "Reset";
            }
        }

        private void LapReset_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                // Record Lap
                var currentTotal = _stopwatch.Elapsed;
                var lapDuration = currentTotal - _lastLapTotalTime;
                _lastLapTotalTime = currentTotal;

                var item = new LapItem
                {
                    Index = _lapCounter++,
                    LapTimeText = $"+{lapDuration.Minutes:D2}:{lapDuration.Seconds:D2}.{lapDuration.Milliseconds / 10:D2}",
                    TotalTimeText = $"{currentTotal.Minutes:D2}:{currentTotal.Seconds:D2}.{currentTotal.Milliseconds / 10:D2}",
                    DurationSeconds = lapDuration.TotalSeconds
                };

                Laps.Insert(0, item);
                HasLaps = true;
                UpdateCharts();
            }
            else
            {
                // Reset
                _stopwatch.Reset();
                _lastLapTotalTime = TimeSpan.Zero;
                _lapCounter = 1;
                ElapsedTimeText = "00:00.00";
                NeedleAngle = 0;
                Laps.Clear();
                LapChartSegments.Clear();
                HasLaps = false;
                LapButtonText = "Lap";
                IsPipVisible = false; // Nascondi PiP se resettato
            }
        }

        private void UpdateCharts()
        {
            var recentLaps = Laps.Take(7).Reverse().ToList();
            LapChartSegments.Clear();

            for (int i = 0; i < recentLaps.Count; i++)
            {
                var lap = recentLaps[i];
                LapChartSegments.Add(new ChartSegment
                {
                    Label = $"Lap {lap.Index}",
                    Value = lap.DurationSeconds,
                    Brush = Palette[i % Palette.Length]
                });
            }
        }
    }

    public class LapItem
    {
        public int Index { get; set; }
        public string LapTimeText { get; set; } = string.Empty;
        public string TotalTimeText { get; set; } = string.Empty;
        public double DurationSeconds { get; set; }
    }
}
