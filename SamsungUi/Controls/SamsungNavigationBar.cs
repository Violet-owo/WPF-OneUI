using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a bottom navigation bar in Samsung One UI style,
    /// featuring an animated indicator that slides to the selected item.
    /// </summary>
    [TemplatePart(Name = "PART_SelectionIndicator", Type = typeof(Border))]
    [TemplatePart(Name = "PART_IndicatorTransform", Type = typeof(TranslateTransform))]
    public class SamsungNavigationBar : ListBox
    {
        // --- Fields ---
        private Border? _selectionIndicator;
        private TranslateTransform? _indicatorTransform;
        private bool _isLoaded;

        // --- Initialization ---

        static SamsungNavigationBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungNavigationBar), new FrameworkPropertyMetadata(typeof(SamsungNavigationBar)));
        }

        public SamsungNavigationBar()
        {
            Loaded += (s, e) => 
            {
                _isLoaded = true;
                UpdateIndicator(false);
            };
        }

        // --- Methods ---

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _selectionIndicator = GetTemplateChild("PART_SelectionIndicator") as Border;
            _indicatorTransform = GetTemplateChild("PART_IndicatorTransform") as TranslateTransform;
            
            if (_isLoaded)
            {
                UpdateIndicator(false);
            }
        }

        // --- Event Handlers & Callbacks ---

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            UpdateIndicator(true);
        }

        private void UpdateIndicator(bool animate)
        {
            if (_selectionIndicator == null || _indicatorTransform == null) return;

            if (SelectedItem != null)
            {
                var container = ItemContainerGenerator.ContainerFromItem(SelectedItem) as UIElement;
                if (container != null)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            var transform = container.TransformToAncestor(this);
                            var point = transform.Transform(new Point(0, 0));
                            var targetWidth = ((FrameworkElement)container).ActualWidth;

                            if (animate && _isLoaded)
                            {
                                var moveAnim = new DoubleAnimation
                                {
                                    To = point.X,
                                    Duration = TimeSpan.FromMilliseconds(300),
                                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                                };

                                var widthAnim = new DoubleAnimation
                                {
                                    To = targetWidth,
                                    Duration = TimeSpan.FromMilliseconds(300),
                                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                                };

                                _indicatorTransform.BeginAnimation(TranslateTransform.XProperty, moveAnim);
                                _selectionIndicator.BeginAnimation(FrameworkElement.WidthProperty, widthAnim);
                            }
                            else
                            {
                                _indicatorTransform.BeginAnimation(TranslateTransform.XProperty, null);
                                _selectionIndicator.BeginAnimation(FrameworkElement.WidthProperty, null);
                                _indicatorTransform.X = point.X;
                                _selectionIndicator.Width = targetWidth;
                            }
                        }
                        catch { }
                    }), System.Windows.Threading.DispatcherPriority.Loaded);
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SamsungNavigationBarItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SamsungNavigationBarItem;
        }
    }

    /// <summary>
    /// Represents a single item inside a <see cref="SamsungNavigationBar"/>.
    /// Supports vector icons (via character) and text.
    /// </summary>
    public class SamsungNavigationBarItem : ListBoxItem
    {
        // --- Initialization ---
        static SamsungNavigationBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungNavigationBarItem), new FrameworkPropertyMetadata(typeof(SamsungNavigationBarItem)));
        }

        // --- Dependency Properties ---

        /// <summary>
        /// Identifies the Icon dependency property.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(string), typeof(SamsungNavigationBarItem), new PropertyMetadata(string.Empty));

        // --- Properties ---

        /// <summary>
        /// Gets or sets the character (or string) representing the item's icon.
        /// Generally, a character from a special font like Segoe MDL2 Assets is used.
        /// </summary>
        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// Identifies the Text dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(SamsungNavigationBarItem), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the text displayed below the item's icon.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}
