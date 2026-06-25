#pragma warning disable CS8600, CS8601, CS8602, CS8603, CS8604, CS8618, CS8622, CS8625
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SamsungUi.Controls
{
    public enum SamsungDateTimePickerMode
    {
        Date,
        Time,
        DateTime
    }

    public class CalendarDayItem
    {
        public DateTime Date { get; set; }
        public string DayNumber { get; set; } = string.Empty;
        public bool IsCurrentMonth { get; set; }
        public bool IsToday { get; set; }
        public bool IsSelected { get; set; }
    }

    public enum CalendarViewMode
    {
        Month,
        Year
    }

    public class CalendarYearItem
    {
        public int Year { get; set; }
        public string DisplayText { get; set; } = string.Empty;
        public bool IsCurrentYear { get; set; }
        public bool IsSelected { get; set; }
    }

    public class TimeSelectorItem
    {
        public int Value { get; set; }
        public string DisplayValue { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }

    /// <summary>
    /// Represents a flexible date and time picker inspired by Samsung One UI, featuring scrolling lists and a modern calendar view.
    /// </summary>
    [TemplatePart(Name = "PART_ToggleButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_HeaderButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_PrevMonthButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_NextMonthButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_CalendarList", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_YearsList", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_HoursList", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_MinutesList", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_SecondsList", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_ConfirmButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_CancelButton", Type = typeof(ButtonBase))]
    public class SamsungDateTimePicker : Control
    {
        // --- Dependency Properties ---

        /// <summary>
        /// Identifies the <see cref="SelectedDate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime?), typeof(SamsungDateTimePicker), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChanged));

        /// <summary>
        /// Gets or sets the selected date and time.
        /// </summary>
        /// <value>A <see cref="DateTime"/> object representing the selected date/time, or null if none is selected.</value>
        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungDateTimePicker picker)
            {
                picker.UpdateDisplayText();
                picker.SyncInternalStateToSelectedDate();
            }
        }

        public static readonly DependencyProperty PickerModeProperty =
            DependencyProperty.Register(nameof(PickerMode), typeof(SamsungDateTimePickerMode), typeof(SamsungDateTimePicker), 
                new PropertyMetadata(SamsungDateTimePickerMode.DateTime, OnPickerModeChanged));

        public SamsungDateTimePickerMode PickerMode
        {
            get => (SamsungDateTimePickerMode)GetValue(PickerModeProperty);
            set => SetValue(PickerModeProperty, value);
        }

        private static void OnPickerModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungDateTimePicker picker) picker.UpdateDisplayText();
        }

        public static readonly DependencyProperty ShowSecondsProperty =
            DependencyProperty.Register(nameof(ShowSeconds), typeof(bool), typeof(SamsungDateTimePicker), new PropertyMetadata(false));

        public bool ShowSeconds
        {
            get => (bool)GetValue(ShowSecondsProperty);
            set => SetValue(ShowSecondsProperty, value);
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(SamsungDateTimePicker), new PropertyMetadata("Select date..."));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(SamsungDateTimePicker), new PropertyMetadata(new CornerRadius(20)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register(nameof(DisplayText), typeof(string), typeof(SamsungDateTimePicker), new PropertyMetadata(string.Empty));

        public string DisplayText
        {
            get => (string)GetValue(DisplayTextProperty);
            private set => SetValue(DisplayTextProperty, value);
        }

        public static readonly DependencyProperty ConfirmTextProperty =
            DependencyProperty.Register(nameof(ConfirmText), typeof(string), typeof(SamsungDateTimePicker), new PropertyMetadata("Done"));

        public string ConfirmText
        {
            get => (string)GetValue(ConfirmTextProperty);
            set => SetValue(ConfirmTextProperty, value);
        }

        public static readonly DependencyProperty CancelTextProperty =
            DependencyProperty.Register(nameof(CancelText), typeof(string), typeof(SamsungDateTimePicker), new PropertyMetadata("Cancel"));

        public string CancelText
        {
            get => (string)GetValue(CancelTextProperty);
            set => SetValue(CancelTextProperty, value);
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register(nameof(IsDropDownOpen), typeof(bool), typeof(SamsungDateTimePicker), new PropertyMetadata(false, OnIsDropDownOpenChanged));

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungDateTimePicker picker && (bool)e.NewValue)
            {
                picker.InitializePickerState();
            }
        }

        public static readonly DependencyProperty CurrentDisplayMonthProperty =
            DependencyProperty.Register(nameof(CurrentDisplayMonth), typeof(DateTime), typeof(SamsungDateTimePicker), new PropertyMetadata(DateTime.Today, OnCurrentDisplayMonthChanged));

        public DateTime CurrentDisplayMonth
        {
            get => (DateTime)GetValue(CurrentDisplayMonthProperty);
            set => SetValue(CurrentDisplayMonthProperty, value);
        }

        private static void OnCurrentDisplayMonthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungDateTimePicker picker) 
            {
                picker.GenerateCalendarDays();
                picker.UpdateHeaderDisplay();
            }
        }

        public static readonly DependencyProperty ViewModeProperty =
            DependencyProperty.Register(nameof(ViewMode), typeof(CalendarViewMode), typeof(SamsungDateTimePicker), new PropertyMetadata(CalendarViewMode.Month, OnViewModeChanged));

        public CalendarViewMode ViewMode
        {
            get => (CalendarViewMode)GetValue(ViewModeProperty);
            set => SetValue(ViewModeProperty, value);
        }

        private static void OnViewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungDateTimePicker picker) 
            {
                if (picker.ViewMode == CalendarViewMode.Year)
                {
                    picker._currentDecadeStartYear = picker.CurrentDisplayMonth.Year - (picker.CurrentDisplayMonth.Year % 10);
                    picker.GenerateCalendarYears();
                }
                picker.UpdateHeaderDisplay();
            }
        }

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(SamsungDateTimePicker), new PropertyMetadata(string.Empty));

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            private set => SetValue(HeaderTextProperty, value);
        }

        // Collection properties for the UI
        public ObservableCollection<string> WeekDays { get; } = new();
        public ObservableCollection<CalendarDayItem> CalendarDays { get; } = new();
        public ObservableCollection<CalendarYearItem> CalendarYears { get; } = new();
        public ObservableCollection<TimeSelectorItem> HoursList { get; } = new();
        public ObservableCollection<TimeSelectorItem> MinutesList { get; } = new();
        public ObservableCollection<TimeSelectorItem> SecondsList { get; } = new();

        // Internal working state
        private DateTime _workingDate = DateTime.Today;
        private Popup? _popup;
        private int _currentDecadeStartYear;

        static SamsungDateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungDateTimePicker), new FrameworkPropertyMetadata(typeof(SamsungDateTimePicker)));
        }

        public SamsungDateTimePicker()
        {
            UpdateDisplayText();
            PopulateTimeLists();
            
            var df = CultureInfo.CurrentCulture.DateTimeFormat;
            // First day of week based on culture
            int firstDay = (int)df.FirstDayOfWeek;
            for (int i = 0; i < 7; i++)
            {
                int dayIndex = (firstDay + i) % 7;
                string dayName = df.ShortestDayNames[dayIndex];
                // Limit to 2 characters max to keep UI clean, e.g. "Mo" or "Lu"
                if (dayName.Length > 2) dayName = dayName.Substring(0, 2);
                WeekDays.Add(dayName.ToUpper());
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_ToggleButton") is ButtonBase toggleBtn)
            {
                toggleBtn.Click += (s, e) => IsDropDownOpen = !IsDropDownOpen;
            }

            _popup = GetTemplateChild("PART_Popup") as Popup;
            if (_popup != null)
            {
                _popup.Opened += OnPopupOpened;
                _popup.Closed += OnPopupClosed;
            }

            if (GetTemplateChild("PART_HeaderButton") is ButtonBase headerBtn)
            {
                headerBtn.Click += (s, e) => ViewMode = ViewMode == CalendarViewMode.Month ? CalendarViewMode.Year : CalendarViewMode.Month;
            }

            if (GetTemplateChild("PART_PrevMonthButton") is ButtonBase prevBtn)
            {
                prevBtn.Click += (s, e) => {
                    if (ViewMode == CalendarViewMode.Month)
                        CurrentDisplayMonth = CurrentDisplayMonth.AddMonths(-1);
                    else
                    {
                        _currentDecadeStartYear -= 12;
                        GenerateCalendarYears();
                        UpdateHeaderDisplay();
                    }
                };
            }

            if (GetTemplateChild("PART_NextMonthButton") is ButtonBase nextBtn)
            {
                nextBtn.Click += (s, e) => {
                    if (ViewMode == CalendarViewMode.Month)
                        CurrentDisplayMonth = CurrentDisplayMonth.AddMonths(1);
                    else
                    {
                        _currentDecadeStartYear += 12;
                        GenerateCalendarYears();
                        UpdateHeaderDisplay();
                    }
                };
            }

            if (GetTemplateChild("PART_YearsList") is ListBox yearsList)
            {
                yearsList.SelectionChanged += YearsList_SelectionChanged;
            }

            if (GetTemplateChild("PART_CalendarList") is ListBox calendarList)
            {
                calendarList.SelectionChanged += CalendarList_SelectionChanged;
            }

            if (GetTemplateChild("PART_HoursList") is ListBox hList) hList.SelectionChanged += TimeList_SelectionChanged;
            if (GetTemplateChild("PART_MinutesList") is ListBox mList) mList.SelectionChanged += TimeList_SelectionChanged;
            if (GetTemplateChild("PART_SecondsList") is ListBox sList) sList.SelectionChanged += TimeList_SelectionChanged;

            if (GetTemplateChild("PART_ConfirmButton") is ButtonBase confirmBtn)
            {
                confirmBtn.Click += ConfirmBtn_Click;
            }

            if (GetTemplateChild("PART_CancelButton") is ButtonBase cancelBtn)
            {
                cancelBtn.Click += CancelBtn_Click;
            }
        }

        private void UpdateDisplayText()
        {
            if (SelectedDate == null)
            {
                DisplayText = Placeholder;
            }
            else
            {
                switch (PickerMode)
                {
                    case SamsungDateTimePickerMode.Date:
                        DisplayText = SelectedDate.Value.ToString("d");
                        break;
                    case SamsungDateTimePickerMode.Time:
                        DisplayText = SelectedDate.Value.ToString(ShowSeconds ? "T" : "t");
                        break;
                    case SamsungDateTimePickerMode.DateTime:
                    default:
                        DisplayText = SelectedDate.Value.ToString(ShowSeconds ? "G" : "g");
                        break;
                }
            }
        }

        private void UpdateHeaderDisplay()
        {
            if (ViewMode == CalendarViewMode.Month)
            {
                HeaderText = CurrentDisplayMonth.ToString("MMMM yyyy");
            }
            else
            {
                HeaderText = $"{_currentDecadeStartYear} - {_currentDecadeStartYear + 11}";
            }
        }

        private void GenerateCalendarYears()
        {
            CalendarYears.Clear();
            for (int i = 0; i < 12; i++)
            {
                int y = _currentDecadeStartYear + i;
                CalendarYears.Add(new CalendarYearItem
                {
                    Year = y,
                    DisplayText = y.ToString(),
                    IsCurrentYear = y == DateTime.Today.Year,
                    IsSelected = y == CurrentDisplayMonth.Year
                });
            }
        }

        private void YearsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is CalendarYearItem item)
            {
                CurrentDisplayMonth = new DateTime(item.Year, CurrentDisplayMonth.Month, 1);
                ViewMode = CalendarViewMode.Month;
                if (GetTemplateChild("PART_YearsList") is ListBox yearsList)
                {
                    yearsList.SelectedItem = null;
                }
            }
        }

        private void SyncInternalStateToSelectedDate()
        {
            if (SelectedDate.HasValue)
            {
                _workingDate = SelectedDate.Value;
                CurrentDisplayMonth = new DateTime(_workingDate.Year, _workingDate.Month, 1);
            }
            else
            {
                _workingDate = DateTime.Now;
                CurrentDisplayMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            RefreshCalendarSelection();
            RefreshTimeSelection();
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            InitializePickerState();
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.PreviewMouseWheel += Window_PreviewMouseWheel;
                window.PreviewKeyDown += Window_PreviewKeyDown;
            }
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            IsDropDownOpen = false;
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.PreviewMouseWheel -= Window_PreviewMouseWheel;
                window.PreviewKeyDown -= Window_PreviewKeyDown;
            }
        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_popup != null && _popup.Child != null && _popup.Child.IsMouseOver) return;
            e.Handled = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.PageDown || e.Key == Key.PageUp || e.Key == Key.Home || e.Key == Key.End)
            {
                e.Handled = true;
            }
        }

        private void InitializePickerState()
        {
            SyncInternalStateToSelectedDate();
        }

        private void PopulateTimeLists()
        {
            for (int i = 0; i < 24; i++) HoursList.Add(new TimeSelectorItem { Value = i, DisplayValue = i.ToString("D2") });
            for (int i = 0; i < 60; i++) MinutesList.Add(new TimeSelectorItem { Value = i, DisplayValue = i.ToString("D2") });
            for (int i = 0; i < 60; i++) SecondsList.Add(new TimeSelectorItem { Value = i, DisplayValue = i.ToString("D2") });
        }

        private void GenerateCalendarDays()
        {
            CalendarDays.Clear();
            var firstDayOfMonth = new DateTime(CurrentDisplayMonth.Year, CurrentDisplayMonth.Month, 1);
            int offset = (int)firstDayOfMonth.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (offset < 0) offset += 7;

            var startDate = firstDayOfMonth.AddDays(-offset);

            for (int i = 0; i < 42; i++) // 6 weeks
            {
                var date = startDate.AddDays(i);
                CalendarDays.Add(new CalendarDayItem
                {
                    Date = date,
                    DayNumber = date.Day.ToString(),
                    IsCurrentMonth = date.Month == CurrentDisplayMonth.Month,
                    IsToday = date.Date == DateTime.Today,
                    IsSelected = SelectedDate.HasValue && date.Date == SelectedDate.Value.Date
                });
            }
            RefreshCalendarSelection();
        }

        private void RefreshCalendarSelection()
        {
            foreach (var day in CalendarDays)
            {
                day.IsSelected = day.Date.Date == _workingDate.Date;
            }

            if (GetTemplateChild("PART_CalendarList") is ListBox calendarList)
            {
                // To trigger UI update
                var selectedItem = CalendarDays.FirstOrDefault(d => d.IsSelected);
                if (selectedItem != null)
                {
                    calendarList.SelectedItem = selectedItem;
                }
            }
        }

        private void RefreshTimeSelection()
        {
            var hList = GetTemplateChild("PART_HoursList") as ListBox;
            var mList = GetTemplateChild("PART_MinutesList") as ListBox;
            var sList = GetTemplateChild("PART_SecondsList") as ListBox;

            if (hList != null) hList.SelectedItem = HoursList.FirstOrDefault(x => x.Value == _workingDate.Hour);
            if (mList != null) mList.SelectedItem = MinutesList.FirstOrDefault(x => x.Value == _workingDate.Minute);
            if (sList != null) sList.SelectedItem = SecondsList.FirstOrDefault(x => x.Value == _workingDate.Second);
            
            // Scroll into view
            if (hList?.SelectedItem != null) hList.ScrollIntoView(hList.SelectedItem);
            if (mList?.SelectedItem != null) mList.ScrollIntoView(mList.SelectedItem);
            if (sList?.SelectedItem != null) sList.ScrollIntoView(sList.SelectedItem);
        }

        private void CalendarList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is CalendarDayItem item)
            {
                _workingDate = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, _workingDate.Hour, _workingDate.Minute, _workingDate.Second);
            }
        }

        private void TimeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GetTemplateChild("PART_HoursList") is ListBox hList && hList.SelectedItem is TimeSelectorItem hItem)
                _workingDate = new DateTime(_workingDate.Year, _workingDate.Month, _workingDate.Day, hItem.Value, _workingDate.Minute, _workingDate.Second);

            if (GetTemplateChild("PART_MinutesList") is ListBox mList && mList.SelectedItem is TimeSelectorItem mItem)
                _workingDate = new DateTime(_workingDate.Year, _workingDate.Month, _workingDate.Day, _workingDate.Hour, mItem.Value, _workingDate.Second);

            if (GetTemplateChild("PART_SecondsList") is ListBox sList && sList.SelectedItem is TimeSelectorItem sItem)
                _workingDate = new DateTime(_workingDate.Year, _workingDate.Month, _workingDate.Day, _workingDate.Hour, _workingDate.Minute, sItem.Value);
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate = _workingDate;
            IsDropDownOpen = false;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = false;
        }
    }
}
