using System;
using System.Windows;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A declarative XAML component that acts as a wrapper for the SamsungNotificationService.
    /// It allows defining notifications in XAML with binding support.
    /// </summary>
    public class SamsungNotification : FrameworkElement
    {
        // --- Dependency Properties ---

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SamsungNotification), new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(SamsungNotification), new PropertyMetadata(string.Empty));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty CardBackgroundProperty =
            DependencyProperty.Register("CardBackground", typeof(Brush), typeof(SamsungNotification), new PropertyMetadata(null));

        public Brush CardBackground
        {
            get => (Brush)GetValue(CardBackgroundProperty);
            set => SetValue(CardBackgroundProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(SamsungNotification), new PropertyMetadata(null));

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty NotificationPositionProperty =
            DependencyProperty.Register("NotificationPosition", typeof(NotificationPosition), typeof(SamsungNotification), new PropertyMetadata(NotificationPosition.TopRight));

        public NotificationPosition NotificationPosition
        {
            get => (NotificationPosition)GetValue(NotificationPositionProperty);
            set => SetValue(NotificationPositionProperty, value);
        }

        public static readonly DependencyProperty DurationMsProperty =
            DependencyProperty.Register("DurationMs", typeof(int), typeof(SamsungNotification), new PropertyMetadata(4000));

        public int DurationMs
        {
            get => (int)GetValue(DurationMsProperty);
            set => SetValue(DurationMsProperty, value);
        }

        public static readonly DependencyProperty IsSoundOnProperty =
            DependencyProperty.Register("IsSoundOn", typeof(bool), typeof(SamsungNotification), new PropertyMetadata(false));

        public bool IsSoundOn
        {
            get => (bool)GetValue(IsSoundOnProperty);
            set => SetValue(IsSoundOnProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(SamsungNotification), new PropertyMetadata(false, OnIsOpenChanged));

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        // --- Event ---

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SamsungNotification));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        // --- Methods ---

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungNotification notif && (bool)e.NewValue == true)
            {
                notif.Show();
                
                // Reset IsOpen to allow re-triggering via bindings
                notif.Dispatcher.InvokeAsync(() => notif.SetCurrentValue(IsOpenProperty, false));
            }
        }

        /// <summary>
        /// Manually triggers the notification to appear.
        /// </summary>
        public void Show()
        {
            SamsungNotificationService.Show(
                title: Title,
                description: Description,
                background: CardBackground,
                icon: Icon,
                position: NotificationPosition,
                durationMs: DurationMs,
                onClick: () => RaiseEvent(new RoutedEventArgs(ClickEvent, this)),
                isSoundOn: IsSoundOn
            );
        }
    }
}
