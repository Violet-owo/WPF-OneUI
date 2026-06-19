using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SamsungUi.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<ChartDataPoint> WeeklyUsageData { get; set; }
        public List<CalendarDay> CalendarDays { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            WeeklyUsageData = new List<ChartDataPoint>
            {
                new ChartDataPoint { Label = "S", Value = 110, IsHighlight = false },
                new ChartDataPoint { Label = "D", Value = 112, IsHighlight = false },
                new ChartDataPoint { Label = "L", Value = 95,  IsHighlight = false },
                new ChartDataPoint { Label = "M", Value = 125, IsHighlight = false },
                new ChartDataPoint { Label = "M", Value = 138, IsHighlight = false },
                new ChartDataPoint { Label = "G", Value = 115, IsHighlight = false },
                new ChartDataPoint { Label = "V", Value = 110, IsHighlight = true } // Giorno corrente evidenziato col cerchio
            };

            CalendarDays = new List<CalendarDay>();

            CalendarDays.Add(new CalendarDay { DayNumber = "29", IsCurrentMonth = false });
            CalendarDays.Add(new CalendarDay { DayNumber = "30", IsCurrentMonth = false });

            // Giorni del mese corrente
            for (int i = 1; i <= 31; i++)
            {
                CalendarDays.Add(new CalendarDay
                {
                    DayNumber = i.ToString(),
                    IsCurrentMonth = true,
                    IsToday = (i == 19) // Evidenziamo il giorno 19 come oggi
                });
            }

            // Spazi per completare la griglia finale
            CalendarDays.Add(new CalendarDay { DayNumber = "1", IsCurrentMonth = false });
            CalendarDays.Add(new CalendarDay { DayNumber = "2", IsCurrentMonth = false });

            // Diamo il DataContext a noi stessi per far funzionare i Binding in XAML
            this.DataContext = this;
            this.InvalidateVisual();
        }

        private void ThemeModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            ChangeTheme(new Uri("Themes/ColorsDark.xaml", UriKind.Relative));
        }

        private void ThemeModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ChangeTheme(new Uri("Themes/ColorsLight.xaml", UriKind.Relative));
        }

        private void ChangeTheme(Uri newThemeUrl)
        {
            var currentTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null &&
                                (d.Source.OriginalString.Contains("ColorsLight.xaml") ||
                                d.Source.OriginalString.Contains("ColorsDark.xaml")));
            if (currentTheme != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(currentTheme);
            }

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = newThemeUrl });
        }

        // Semplice classe POCO per strutturare i dati del grafico
        public class ChartDataPoint
        {
            public string Label { get; set; }
            public double Value { get; set; }
            public bool IsHighlight { get; set; }
        }
    }
}