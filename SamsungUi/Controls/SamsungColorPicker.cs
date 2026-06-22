using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_SpectrumArea", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_SpectrumThumb", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_BrightnessSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_OpacitySlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_DoneButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_CancelButton", Type = typeof(ButtonBase))]
    public class SamsungColorPicker : Control
    {
        private Popup _popup;
        private FrameworkElement _spectrumArea;
        private FrameworkElement _spectrumThumb;
        private Slider _brightnessSlider;
        private Slider _opacitySlider;

        private bool _isDraggingThumb = false;
        private bool _isUpdatingFromInternal = false;

        private Color _previousColor;

        static SamsungColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungColorPicker), new FrameworkPropertyMetadata(typeof(SamsungColorPicker)));
        }

        public SamsungColorPicker()
        {
            RecentColors = new ObservableCollection<Color>();
        }

        #region Dependency Properties

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(SamsungColorPicker), 
                new FrameworkPropertyMetadata(Colors.LightBlue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(SamsungColorPicker), 
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(double), typeof(SamsungColorPicker), new PropertyMetadata(0.0, OnHsvChanged));

        public double Hue
        {
            get => (double)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register("Saturation", typeof(double), typeof(SamsungColorPicker), new PropertyMetadata(1.0, OnHsvChanged));

        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(SamsungColorPicker), new PropertyMetadata(1.0, OnHsvChanged));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register("Alpha", typeof(double), typeof(SamsungColorPicker), new PropertyMetadata(1.0, OnHsvChanged));

        public double Alpha
        {
            get => (double)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }

        public static readonly DependencyProperty RecentColorsProperty =
            DependencyProperty.Register("RecentColors", typeof(ObservableCollection<Color>), typeof(SamsungColorPicker), new PropertyMetadata(null));

        public ObservableCollection<Color> RecentColors
        {
            get => (ObservableCollection<Color>)GetValue(RecentColorsProperty);
            set => SetValue(RecentColorsProperty, value);
        }
        
        // This brush represents the pure Hue color for the background of the Brightness slider
        public static readonly DependencyProperty PureHueBrushProperty =
            DependencyProperty.Register("PureHueBrush", typeof(SolidColorBrush), typeof(SamsungColorPicker), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public SolidColorBrush PureHueBrush
        {
            get => (SolidColorBrush)GetValue(PureHueBrushProperty);
            set => SetValue(PureHueBrushProperty, value);
        }

        public static readonly DependencyProperty UsePlusIconProperty =
            DependencyProperty.Register("UsePlusIcon", typeof(bool), typeof(SamsungColorPicker), new PropertyMetadata(false));

        public bool UsePlusIcon
        {
            get => (bool)GetValue(UsePlusIconProperty);
            set => SetValue(UsePlusIconProperty, value);
        }

        public static readonly RoutedEvent ColorAppliedEvent = EventManager.RegisterRoutedEvent(
            "ColorApplied", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SamsungColorPicker));

        public event RoutedEventHandler ColorApplied
        {
            add { AddHandler(ColorAppliedEvent, value); }
            remove { RemoveHandler(ColorAppliedEvent, value); }
        }

        #endregion

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungColorPicker picker && !picker._isUpdatingFromInternal)
            {
                picker.UpdateHsvFromColor((Color)e.NewValue);
            }
        }

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungColorPicker picker)
            {
                if ((bool)e.NewValue)
                {
                    // Popup opened
                    picker._previousColor = picker.SelectedColor;
                    picker.AttachWindowEvents();
                }
                else
                {
                    // Popup closed
                    picker.DetachWindowEvents();
                    
                    // Add to recent colors if not already there and color has changed
                    if (picker._previousColor != picker.SelectedColor && !picker.RecentColors.Contains(picker.SelectedColor))
                    {
                        picker.RecentColors.Insert(0, picker.SelectedColor);
                        if (picker.RecentColors.Count > 5)
                            picker.RecentColors.RemoveAt(picker.RecentColors.Count - 1);
                    }
                }
            }
        }

        private static void OnHsvChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungColorPicker picker && !picker._isUpdatingFromInternal)
            {
                picker.UpdateColorFromHsv();
                picker.UpdateThumbPosition();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = GetTemplateChild("PART_Popup") as Popup;
            
            _spectrumArea = GetTemplateChild("PART_SpectrumArea") as FrameworkElement;
            if (_spectrumArea != null)
            {
                _spectrumArea.MouseLeftButtonDown += SpectrumArea_MouseLeftButtonDown;
                _spectrumArea.MouseLeftButtonUp += SpectrumArea_MouseLeftButtonUp;
                _spectrumArea.MouseMove += SpectrumArea_MouseMove;
            }

            _spectrumThumb = GetTemplateChild("PART_SpectrumThumb") as FrameworkElement;

            _brightnessSlider = GetTemplateChild("PART_BrightnessSlider") as Slider;
            _opacitySlider = GetTemplateChild("PART_OpacitySlider") as Slider;

            if (GetTemplateChild("PART_DoneButton") is ButtonBase doneBtn)
                doneBtn.Click += (s, e) => {
                    IsDropDownOpen = false;
                    RaiseEvent(new RoutedEventArgs(ColorAppliedEvent));
                };

            if (GetTemplateChild("PART_CancelButton") is ButtonBase cancelBtn)
                cancelBtn.Click += (s, e) => { SelectedColor = _previousColor; IsDropDownOpen = false; };

            if (GetTemplateChild("PART_ToggleButton") is ButtonBase toggleBtn)
                toggleBtn.Click += (s, e) => IsDropDownOpen = !IsDropDownOpen;

            UpdateHsvFromColor(SelectedColor);
            UpdateThumbPosition();
        }

        private void SpectrumArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDraggingThumb = true;
            _spectrumArea?.CaptureMouse();
            UpdateHsvFromMouse(e.GetPosition(_spectrumArea));
        }

        private void SpectrumArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDraggingThumb)
            {
                _isDraggingThumb = false;
                _spectrumArea?.ReleaseMouseCapture();
            }
        }

        private void SpectrumArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDraggingThumb)
            {
                UpdateHsvFromMouse(e.GetPosition(_spectrumArea));
            }
        }

        private void UpdateHsvFromMouse(Point pos)
        {
            if (_spectrumArea == null) return;

            double width = _spectrumArea.ActualWidth;
            double height = _spectrumArea.ActualHeight;

            if (width <= 0 || height <= 0) return;

            double x = Math.Max(0, Math.Min(pos.X, width));
            double y = Math.Max(0, Math.Min(pos.Y, height));

            _isUpdatingFromInternal = true;

            Hue = (x / width) * 360.0;
            if (Hue >= 360) Hue = 359.99;
            Saturation = 1.0 - (y / height);

            UpdateColorFromHsv();
            UpdateThumbPosition();

            _isUpdatingFromInternal = false;
        }

        private void UpdateThumbPosition()
        {
            if (_spectrumArea == null || _spectrumThumb == null) return;

            double width = _spectrumArea.ActualWidth;
            double height = _spectrumArea.ActualHeight;

            if (width > 0 && height > 0)
            {
                double x = (Hue / 360.0) * width;
                double y = (1.0 - Saturation) * height;

                Canvas.SetLeft(_spectrumThumb, x - (_spectrumThumb.ActualWidth / 2));
                Canvas.SetTop(_spectrumThumb, y - (_spectrumThumb.ActualHeight / 2));
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateThumbPosition();
        }

        private void UpdateColorFromHsv()
        {
            _isUpdatingFromInternal = true;
            
            Color c = HsvToRgb(Hue, Saturation, Value, Alpha);
            SelectedColor = c;
            
            Color pureHue = HsvToRgb(Hue, Saturation, 1.0, 1.0);
            PureHueBrush = new SolidColorBrush(pureHue);

            _isUpdatingFromInternal = false;
        }

        private void UpdateHsvFromColor(Color color)
        {
            _isUpdatingFromInternal = true;

            RgbToHsv(color, out double h, out double s, out double v);
            
            // If saturation or value is 0, hue gets lost in conversion, preserve it if possible
            if (s > 0 && v > 0) Hue = h;
            Saturation = s;
            Value = v;
            Alpha = color.A / 255.0;

            Color pureHue = HsvToRgb(Hue, Saturation, 1.0, 1.0);
            PureHueBrush = new SolidColorBrush(pureHue);

            UpdateThumbPosition();

            _isUpdatingFromInternal = false;
        }

        // --- Math Helpers ---

        public static Color HsvToRgb(double h, double s, double v, double a)
        {
            int hi = Convert.ToInt32(Math.Floor(h / 60)) % 6;
            double f = h / 60 - Math.Floor(h / 60);

            double vVal = v * 255;
            byte vByte = (byte)vVal;
            byte p = (byte)(vVal * (1 - s));
            byte q = (byte)(vVal * (1 - f * s));
            byte t = (byte)(vVal * (1 - (1 - f) * s));
            byte alpha = (byte)(a * 255);

            switch (hi)
            {
                case 0: return Color.FromArgb(alpha, vByte, t, p);
                case 1: return Color.FromArgb(alpha, q, vByte, p);
                case 2: return Color.FromArgb(alpha, p, vByte, t);
                case 3: return Color.FromArgb(alpha, p, q, vByte);
                case 4: return Color.FromArgb(alpha, t, p, vByte);
                default: return Color.FromArgb(alpha, vByte, p, q);
            }
        }

        public static void RgbToHsv(Color color, out double h, out double s, out double v)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            v = max / 255.0;
            s = max == 0 ? 0 : 1d - (1d * min / max);

            if (max == min)
            {
                h = 0;
            }
            else if (max == color.R)
            {
                h = 60 * (color.G - color.B) / (double)(max - min);
                if (color.G < color.B) h += 360;
            }
            else if (max == color.G)
            {
                h = 60 * (color.B - color.R) / (double)(max - min) + 120;
            }
            else // max == color.B
            {
                h = 60 * (color.R - color.G) / (double)(max - min) + 240;
            }
        }
        
        // --- Input Blocking Logic for the Window ---
        
        private Window _parentWindow;

        private void AttachWindowEvents()
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                _parentWindow = window;
                window.PreviewMouseWheel += Window_PreviewMouseWheel;
                window.PreviewKeyDown += Window_PreviewKeyDown;
            }
        }

        private void DetachWindowEvents()
        {
            if (_parentWindow != null)
            {
                _parentWindow.PreviewMouseWheel -= Window_PreviewMouseWheel;
                _parentWindow.PreviewKeyDown -= Window_PreviewKeyDown;
                _parentWindow = null;
            }
        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_popup != null && _popup.Child != null && _popup.Child.IsMouseOver) return;
            e.Handled = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Only block navigation keys that might scroll or change focus
            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.PageUp || e.Key == Key.PageDown)
            {
                e.Handled = true;
            }
        }
    }
}
