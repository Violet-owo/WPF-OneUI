using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SamsungUi.Models;

namespace SamsungUi.Controls
{
    [TemplatePart(Name = "PART_PrevMonthButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_NextMonthButton", Type = typeof(ButtonBase))]
    public class SamsungCalendar : Control
    {
        static SamsungCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungCalendar), new FrameworkPropertyMetadata(typeof(SamsungCalendar)));
        }

        public SamsungCalendar()
        {
            var df = CultureInfo.CurrentCulture.DateTimeFormat;
            int firstDay = (int)df.FirstDayOfWeek;
            for (int i = 0; i < 7; i++)
            {
                int dayIndex = (firstDay + i) % 7;
                string dayName = df.ShortestDayNames[dayIndex];
                if (dayName.Length > 2) dayName = dayName.Substring(0, 2);
                WeekDays.Add(dayName.ToUpper());
            }

            Events.CollectionChanged += Events_CollectionChanged;
            GenerateCalendarDays();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_PrevMonthButton") is ButtonBase prevBtn)
            {
                prevBtn.Click += (s, e) => CurrentDisplayMonth = CurrentDisplayMonth.AddMonths(-1);
            }

            if (GetTemplateChild("PART_NextMonthButton") is ButtonBase nextBtn)
            {
                nextBtn.Click += (s, e) => CurrentDisplayMonth = CurrentDisplayMonth.AddMonths(1);
            }
        }

        public ObservableCollection<string> WeekDays { get; } = new();
        public ObservableCollection<CalendarDay> CalendarDays { get; } = new();
        public ObservableCollection<CalendarEvent> Events { get; } = new();

        private void Events_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GenerateCalendarDays();
        }

        public static readonly DependencyProperty CurrentDisplayMonthProperty =
            DependencyProperty.Register(nameof(CurrentDisplayMonth), typeof(DateTime), typeof(SamsungCalendar), new PropertyMetadata(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), OnCurrentDisplayMonthChanged));

        public DateTime CurrentDisplayMonth
        {
            get => (DateTime)GetValue(CurrentDisplayMonthProperty);
            set => SetValue(CurrentDisplayMonthProperty, value);
        }

        private static void OnCurrentDisplayMonthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungCalendar calendar)
            {
                calendar.HeaderText = calendar.CurrentDisplayMonth.ToString("MMMM yyyy");
                calendar.GenerateCalendarDays();
            }
        }

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(SamsungCalendar), new PropertyMetadata(DateTime.Today.ToString("MMMM yyyy")));

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            private set => SetValue(HeaderTextProperty, value);
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
                var day = new CalendarDay
                {
                    Date = date,
                    DayNumber = date.Day.ToString(),
                    IsCurrentMonth = date.Month == CurrentDisplayMonth.Month,
                    IsToday = date.Date == DateTime.Today
                };

                // Add events that match this date
                var matchingEvents = Events.Where(ev => ev.Date.Date == date.Date);
                foreach(var ev in matchingEvents)
                {
                    day.Events.Add(ev);
                }

                CalendarDays.Add(day);
            }
        }
    }
}
