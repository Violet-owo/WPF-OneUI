using System;
using System.Collections.Generic;
using System.Linq;

namespace SamsungUi.Models
{
    public class CalendarDay
    {
        public string DayNumber { get; set; } = string.Empty;
        public bool IsCurrentMonth { get; set; } = true;
        public bool IsToday { get; set; } = false;
        public DateTime Date { get; set; }

        public List<CalendarEvent> Events { get; set; } = new List<CalendarEvent>();

        // Helpers per il binding XAML
        public bool HasEvents => Events != null && Events.Any();
        public CalendarEvent? FirstEvent => Events?.FirstOrDefault();
    }
}
