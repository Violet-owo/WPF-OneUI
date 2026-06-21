using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SamsungUi.Demo.Pages
{
    public class ColorPaletteItem : System.ComponentModel.INotifyPropertyChanged
    {
        // --- Properties ---
        public SolidColorBrush Brush { get; set; }
        
        private bool _isSelected;
        public bool IsSelected 
        { 
            get => _isSelected; 
            set 
            { 
                if (_isSelected != value) 
                {
                    _isSelected = value; 
                    PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(IsSelected))); 
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }

    public partial class SettingsPage : SamsungUi.Controls.SamsungExpandablePage
    {
        // --- Fields ---
        private bool _isUpdatingSliders = false;
        private System.Collections.ObjectModel.ObservableCollection<ColorPaletteItem> _baseColors;
        private System.Collections.ObjectModel.ObservableCollection<ColorPaletteItem> _customColors;

        // --- Initialization ---
        public SettingsPage()
        {
            InitializeComponent();
            
            _baseColors = new System.Collections.ObjectModel.ObservableCollection<ColorPaletteItem>
            {
                new ColorPaletteItem { Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0381FE")), IsSelected = true },
                new ColorPaletteItem { Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#34C759")) },
                new ColorPaletteItem { Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9500")) },
                new ColorPaletteItem { Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AF52DE")) },
                new ColorPaletteItem { Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2D55")) }
            };
            
            _customColors = new System.Collections.ObjectModel.ObservableCollection<ColorPaletteItem>();
            
            ColorPaletteItems.ItemsSource = _baseColors;
            CustomColorPaletteItems.ItemsSource = _customColors;
        }

        // --- Event Handlers & Callbacks ---
        private void AccentColor_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.DataContext is ColorPaletteItem item)
            {
                if (item.IsSelected)
                {
                    ApplyAccentColor(item.Brush.Color);
                    
                    // Deselect all other colors to avoid visual glitches (double rings)
                    if (_baseColors != null)
                    {
                        foreach (var c in _baseColors)
                        {
                            if (c != item) c.IsSelected = false;
                        }
                    }
                    if (_customColors != null)
                    {
                        foreach (var c in _customColors)
                        {
                            if (c != item) c.IsSelected = false;
                        }
                    }
                }
            }
        }

        // --- Methods ---
        private void ApplyAccentColor(Color color)
        {
            var brush = new SolidColorBrush(color);
            Application.Current.Resources["OneUiPrimaryBrush"] = brush;
            
            // Calculate a slightly darker color for hover effect
            var hoverColor = Color.FromArgb(color.A, (byte)Math.Max(0, color.R - 30), (byte)Math.Max(0, color.G - 30), (byte)Math.Max(0, color.B - 30));
            Application.Current.Resources["OneUiPrimaryHoverBrush"] = new SolidColorBrush(hoverColor);
            
            Application.Current.Resources["OneUiSliderActiveBrush"] = brush;
            Application.Current.Resources["OneUiChartBarBrush"] = brush;
        }

        private void ThemeModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            var mergedDicts = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary oldDict = null;
            foreach (var dict in mergedDicts)
            {
                if (dict.Source != null && dict.Source.ToString().Contains("ColorsLight.xaml"))
                {
                    oldDict = dict;
                    break;
                }
            }
            if (oldDict != null) mergedDicts.Remove(oldDict);
            
            mergedDicts.Insert(0, new ResourceDictionary { Source = new Uri("pack://application:,,,/SamsungUi;component/Themes/ColorsDark.xaml") });
        }

        private void ThemeModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            var mergedDicts = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary oldDict = null;
            foreach (var dict in mergedDicts)
            {
                if (dict.Source != null && dict.Source.ToString().Contains("ColorsDark.xaml"))
                {
                    oldDict = dict;
                    break;
                }
            }
            if (oldDict != null) mergedDicts.Remove(oldDict);
            
            mergedDicts.Insert(0, new ResourceDictionary { Source = new Uri("pack://application:,,,/SamsungUi;component/Themes/ColorsLight.xaml") });
        }

        private void OpenColorPicker_Click(object sender, RoutedEventArgs e)
        {
            var currentBrush = Application.Current.Resources["OneUiPrimaryBrush"] as SolidColorBrush;
            if (currentBrush != null)
            {
                _isUpdatingSliders = true;
                RedSlider.Value = currentBrush.Color.R;
                GreenSlider.Value = currentBrush.Color.G;
                BlueSlider.Value = currentBrush.Color.B;
                UpdatePreviewColor();
                _isUpdatingSliders = false;
            }
            ColorPickerModal.IsOpen = true;
        }

        private void CloseColorPicker_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerModal.IsOpen = false;
        }

        private void ApplyCustomColor_Click(object sender, RoutedEventArgs e)
        {
            var color = Color.FromRgb((byte)RedSlider.Value, (byte)GreenSlider.Value, (byte)BlueSlider.Value);
            var brush = new SolidColorBrush(color);
            
            // Deselect all other colors before adding the new one
            if (_baseColors != null) { foreach (var c in _baseColors) c.IsSelected = false; }
            if (_customColors != null) { foreach (var c in _customColors) c.IsSelected = false; }
            
            var newItem = new ColorPaletteItem { Brush = brush, IsSelected = true };
            _customColors.Add(newItem);
            CustomColorSeparator.Visibility = Visibility.Visible;
            
            ApplyAccentColor(color);
            ColorPickerModal.IsOpen = false;
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isUpdatingSliders && ColorPreviewBorder != null)
            {
                UpdatePreviewColor();
            }
        }

        private void UpdatePreviewColor()
        {
            if (RedSlider == null || GreenSlider == null || BlueSlider == null || ColorPreviewBorder == null)
                return;

            var color = Color.FromRgb((byte)RedSlider.Value, (byte)GreenSlider.Value, (byte)BlueSlider.Value);
            ColorPreviewBorder.Background = new SolidColorBrush(color);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox == null) return;
            string query = SearchBox.Text.ToLower().Trim();

            bool isSearchEmpty = string.IsNullOrEmpty(query);

            // Filter the elements in the Settings page
            WiFiRow.Visibility = (isSearchEmpty || "wi-fi".Contains(query) || "network".Contains(query)) ? Visibility.Visible : Visibility.Collapsed;
            BluetoothRow.Visibility = (isSearchEmpty || "bluetooth".Contains(query)) ? Visibility.Visible : Visibility.Collapsed;
            WiFiSep.Visibility = (WiFiRow.Visibility == Visibility.Visible && BluetoothRow.Visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;

            RingtoneRow.Visibility = (isSearchEmpty || "ringtone".Contains(query) || "volume".Contains(query) || "sounds".Contains(query)) ? Visibility.Visible : Visibility.Collapsed;
            RingtoneSlider.Visibility = RingtoneRow.Visibility;

            MediaRow.Visibility = (isSearchEmpty || "media".Contains(query) || "volume".Contains(query) || "sounds".Contains(query)) ? Visibility.Visible : Visibility.Collapsed;
            MediaSlider.Visibility = MediaRow.Visibility;

            DarkModeRow.Visibility = (isSearchEmpty || "dark mode".Contains(query) || "theme".Contains(query) || "display".Contains(query)) ? Visibility.Visible : Visibility.Collapsed;
            
            AccentColorRow.Visibility = (isSearchEmpty || "accent color".Contains(query) || "colors".Contains(query) || "display".Contains(query)) ? Visibility.Visible : Visibility.Collapsed;
            AccentColorPanel.Visibility = AccentColorRow.Visibility;

            // Auto-navigate to the correct tab
            if (!isSearchEmpty)
            {
                if (WiFiRow.Visibility == Visibility.Visible || BluetoothRow.Visibility == Visibility.Visible)
                {
                    SelectTabItem("Connections");
                }
                else if (RingtoneRow.Visibility == Visibility.Visible || MediaRow.Visibility == Visibility.Visible)
                {
                    SelectTabItem("Sounds");
                }
                else if (DarkModeRow.Visibility == Visibility.Visible || AccentColorRow.Visibility == Visibility.Visible)
                {
                    SelectTabItem("Display");
                }
            }

            // Filter cards in other pages via MainWindow
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.FilterAllPages(query);
            }
        }

        private void SelectTabItem(string header)
        {
            if (SettingsTabControl == null) return;
            foreach (TabItem item in SettingsTabControl.Items)
            {
                if (item.Header?.ToString() == header)
                {
                    SettingsTabControl.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
