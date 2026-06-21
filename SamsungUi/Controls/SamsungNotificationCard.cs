using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a highly modular and scalable notification card in Samsung One UI style.
    /// Can be used both as a static XAML control or dynamically via SamsungNotificationService.
    /// </summary>
    public class SamsungNotificationCard : Control
    {
        static SamsungNotificationCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungNotificationCard), new FrameworkPropertyMetadata(typeof(SamsungNotificationCard)));
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(SamsungNotificationCard), new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(SamsungNotificationCard), new PropertyMetadata(string.Empty));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(SamsungNotificationCard), new PropertyMetadata(null));

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty CardBackgroundProperty =
            DependencyProperty.Register(nameof(CardBackground), typeof(Brush), typeof(SamsungNotificationCard), new PropertyMetadata(null));

        public Brush CardBackground
        {
            get => (Brush)GetValue(CardBackgroundProperty);
            set => SetValue(CardBackgroundProperty, value);
        }

        // --- Events ---

        public static readonly RoutedEvent CloseClickEvent = EventManager.RegisterRoutedEvent(
            nameof(CloseClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SamsungNotificationCard));

        public event RoutedEventHandler CloseClick
        {
            add => AddHandler(CloseClickEvent, value);
            remove => RemoveHandler(CloseClickEvent, value);
        }

        public static readonly RoutedEvent CardClickEvent = EventManager.RegisterRoutedEvent(
            nameof(CardClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SamsungNotificationCard));

        public event RoutedEventHandler CardClick
        {
            add => AddHandler(CardClickEvent, value);
            remove => RemoveHandler(CardClickEvent, value);
        }

        // --- Overrides ---

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_CloseButton") is Button closeButton)
            {
                closeButton.Click += (s, e) =>
                {
                    e.Handled = true; // Prevent bubble to CardClick
                    RaiseEvent(new RoutedEventArgs(CloseClickEvent, this));
                };
            }
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (!e.Handled)
            {
                RaiseEvent(new RoutedEventArgs(CardClickEvent, this));
                e.Handled = true;
            }
        }
    }
}
