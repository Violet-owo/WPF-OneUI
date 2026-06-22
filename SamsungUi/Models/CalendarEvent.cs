using System.Windows.Media;

namespace SamsungUi.Models
{
    public class CalendarEvent
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Brush DotColor { get; set; } = Brushes.Gray;
    }
}
