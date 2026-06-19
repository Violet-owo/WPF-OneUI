using System;
using System.Collections.Generic;
using System.Text;

namespace SamsungUi.Demo
{
    public class CalendarDay
    {
        public string DayNumber { get; set; }  // Testo da mostrare (es: "1", "2" o "" se vuoto)
        public bool IsCurrentMonth { get; set; } = true; // False per i giorni del mese precedente/successivo
        public bool IsToday { get; set; } = false; // True per colorare il giorno corrente
        public DateTime Date { get; set; }
    }
}
