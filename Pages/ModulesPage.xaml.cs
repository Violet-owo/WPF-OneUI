using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using SamsungUi.Models;

namespace SamsungUi.Demo.Pages
{
    public partial class ModulesPage : SamsungUi.Controls.SamsungExpandablePage
    {
        // --- Properties ---
        public List<ChartDataPoint> WeeklyUsageData { get; set; }
        public List<CalendarDay> CalendarDays { get; set; }

        public List<double> HistorySeries1 { get; set; }
        public List<double> HistorySeries2 { get; set; }
        public List<string> HistoryXLabels { get; set; }

        // --- Initialization ---
        public ModulesPage()
        {
            InitializeComponent();

            HistorySeries1 = new List<double> { 90, 140, 160, 162, 150, 130, 115, 110, 120, 132, 154, 185, 230 };
            HistorySeries2 = new List<double> { 250, 210, 190, 180, 170, 160, 172, 190, 204, 210, 202, 188, 150 };
            HistoryXLabels = new List<string> { "12", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            WeeklyUsageData = new List<ChartDataPoint>
            {
                new ChartDataPoint { Label = "S", Value = 110, IsHighlight = false },
                new ChartDataPoint { Label = "M", Value = 112, IsHighlight = false },
                new ChartDataPoint { Label = "T", Value = 95,  IsHighlight = false },
                new ChartDataPoint { Label = "W", Value = 125, IsHighlight = false },
                new ChartDataPoint { Label = "T", Value = 138, IsHighlight = false },
                new ChartDataPoint { Label = "F", Value = 115, IsHighlight = false },
                new ChartDataPoint { Label = "S", Value = 110, IsHighlight = true }
            };

            CalendarDays = new List<CalendarDay>();
            CalendarDays.Add(new CalendarDay { DayNumber = "29", IsCurrentMonth = false });
            CalendarDays.Add(new CalendarDay { DayNumber = "30", IsCurrentMonth = false });

            for (int i = 1; i <= 31; i++)
            {
                var day = new CalendarDay
                {
                    DayNumber = i.ToString(),
                    IsCurrentMonth = true,
                    IsToday = (i == 19)
                };

                if (i == 5)
                {
                    day.Events.Add(new CalendarEvent { Title = "Project Meeting", Description = "Architecture discussion.", DotColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0381FE")) });
                }
                if (i == 14)
                {
                    day.Events.Add(new CalendarEvent { Title = "Business Lunch", Description = "Lunch with clients.", DotColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E53935")) });
                }
                if (i == 22)
                {
                    day.Events.Add(new CalendarEvent { Title = "Birthday", Description = "Office birthday party.", DotColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50")) });
                }

                CalendarDays.Add(day);
            }

            CalendarDays.Add(new CalendarDay { DayNumber = "1", IsCurrentMonth = false });
            CalendarDays.Add(new CalendarDay { DayNumber = "2", IsCurrentMonth = false });

            this.DataContext = this;
        }

        // --- Event Handlers ---
        private int _testCount = 1;

        private void OnTestNotificationTopRight_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var brush = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString("#FFB045"),
                (Color)ColorConverter.ConvertFromString("#FF763A"),
                new System.Windows.Point(0, 0),
                new System.Windows.Point(1, 1));
            
            SamsungUi.Controls.SamsungNotificationService.Show(
                title: $"Before bed ({_testCount++})",
                description: "Wind down before bed by turning on the Blue light filter, adjusting the screen brightness, and more.",
                background: brush,
                icon: null,
                position: SamsungUi.Controls.NotificationPosition.TopRight,
                onClick: () => SamsungUi.Controls.SamsungToastService.Show("Hai cliccato la notifica Top Right!")
            );
        }

        private void OnTestNotificationTopLeft_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var brush = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString("#FF6B6B"),
                (Color)ColorConverter.ConvertFromString("#EE3A3A"),
                new System.Windows.Point(0, 0),
                new System.Windows.Point(1, 1));

            SamsungUi.Controls.SamsungNotificationService.Show(
                title: $"Save battery at night ({_testCount++})",
                description: "This routine will run if you go to bed without plugging in your phone.",
                background: brush,
                icon: null,
                position: SamsungUi.Controls.NotificationPosition.TopLeft,
                onClick: () => SamsungUi.Controls.SamsungToastService.Show("Hai cliccato la notifica Top Left!")
            );
        }

        private void OnTestNotificationBottomRight_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var brush = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString("#6B9CFF"),
                (Color)ColorConverter.ConvertFromString("#3A76EE"),
                new System.Windows.Point(0, 0),
                new System.Windows.Point(1, 1));

            SamsungUi.Controls.SamsungNotificationService.Show(
                title: $"Driving ({_testCount++})",
                description: "Get behind the wheel with ease by automatically setting up your phone for driving.",
                background: brush,
                icon: null,
                position: SamsungUi.Controls.NotificationPosition.BottomRight,
                onClick: () => SamsungUi.Controls.SamsungToastService.Show("Hai cliccato la notifica Bottom Right!")
            );
        }

        private void OnTestNotificationBottomCenter_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8E24AA"));

            SamsungUi.Controls.SamsungNotificationService.Show(
                title: $"Custom Update ({_testCount++})",
                description: "System has been updated successfully.",
                background: brush,
                icon: null,
                position: SamsungUi.Controls.NotificationPosition.BottomCenter,
                onClick: () => SamsungUi.Controls.SamsungToastService.Show("Hai cliccato la notifica Bottom Center!"),
                isSoundOn: true
            );
        }

        private void OnTestXamlNotification_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // The notification is declared in XAML, we just call Show() on it.
            // Alternatively, we could do XamlNotification.IsOpen = true;
            XamlNotification.Show();
        }

        private void XamlNotification_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SamsungUi.Controls.SamsungToastService.Show("Hai cliccato la notifica dichiarata in XAML!");
        }

        // --- Nested Classes ---
        public class ChartDataPoint
        {
            public string Label { get; set; }
            public double Value { get; set; }
            public bool IsHighlight { get; set; }
        }
    }
}
