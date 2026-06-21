using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using SamsungUi.Models;

namespace SamsungUi.Demo.Pages
{
    public partial class StopwatchPage : SamsungUi.Controls.SamsungExpandablePage, INotifyPropertyChanged
    {
        // --- Fields ---
        private DispatcherTimer _timer;
        private Stopwatch _stopwatch;
        private TimeSpan _lastLapTotalTime = TimeSpan.Zero;
        private int _lapCounter = 1;

        // --- Events ---
        public event PropertyChangedEventHandler? PropertyChanged;

        // --- Properties ---
        private string _elapsedTimeText = "00:00.00";
        public string ElapsedTimeText
        {
            get => _elapsedTimeText;
            set { _elapsedTimeText = value; OnPropertyChanged(nameof(ElapsedTimeText)); }
        }

        private double _needleAngle = 0;
        public double NeedleAngle
        {
            get => _needleAngle;
            set { _needleAngle = value; OnPropertyChanged(nameof(NeedleAngle)); }
        }

        private string _startButtonText = "Start";
        public string StartButtonText
        {
            get => _startButtonText;
            set { _startButtonText = value; OnPropertyChanged(nameof(StartButtonText)); }
        }

        private string _lapButtonText = "Lap";
        public string LapButtonText
        {
            get => _lapButtonText;
            set { _lapButtonText = value; OnPropertyChanged(nameof(LapButtonText)); }
        }

        private bool _isRunning = false;
        public bool IsRunning
        {
            get => _isRunning;
            set { _isRunning = value; OnPropertyChanged(nameof(IsRunning)); }
        }

        private bool _hasLaps = false;
        public bool HasLaps
        {
            get => _hasLaps;
            set { _hasLaps = value; OnPropertyChanged(nameof(HasLaps)); }
        }

        public ObservableCollection<LapItem> Laps { get; } = new();
        public ObservableCollection<ChartSegment> LapChartSegments { get; } = new();

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

        // --- Initialization ---
        public StopwatchPage()
        {
            InitializeComponent();
            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += Timer_Tick;

            DataContext = this;
        }

        // --- Event Handlers & Callbacks ---
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
            if (!_isRunning)
            {
                // Start
                _stopwatch.Start();
                _timer.Start();
                IsRunning = true;
                StartButtonText = "Stop";
                LapButtonText = "Lap";
            }
            else
            {
                // Stop
                _stopwatch.Stop();
                _timer.Stop();
                IsRunning = false;
                StartButtonText = "Start";
                LapButtonText = "Reset";
            }
        }

        private void LapReset_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
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
            }
        }

        // --- Methods ---
        private void UpdateCharts()
        {
            // Limit to last 7 laps for visual clarity in charts
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

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // --- Nested Classes ---
    public class LapItem
    {
        public int Index { get; set; }
        public string LapTimeText { get; set; } = string.Empty;
        public string TotalTimeText { get; set; } = string.Empty;
        public double DurationSeconds { get; set; }
    }
}
