using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Specifies the corner or edge where the notification stack should appear.
    /// </summary>
    public enum NotificationPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    /// <summary>
    /// Represents the parameters for a notification to be displayed.
    /// </summary>
    public class NotificationRequest
    {
        // --- Properties ---
        public string Title { get; set; }
        public string Description { get; set; }
        public Brush Background { get; set; }
        public ImageSource Icon { get; set; }
        public int DurationMs { get; set; }
        public Action OnClick { get; set; }
        public bool IsSoundOn { get; set; }
    }

    /// <summary>
    /// Service for showing advanced notifications that stack elegantly on screen.
    /// Supports multiple positions, custom icons, actions, and automatic dismissal.
    /// </summary>
    public static class SamsungNotificationService
    {
        // --- Fields ---
        private static readonly Dictionary<NotificationPosition, NotificationStackContainer> _containers = new();
        private static readonly List<MediaPlayer> _activePlayers = new();

        // --- Methods ---
        public static void Show(
            string title, 
            string description, 
            Brush background, 
            ImageSource icon = null, 
            NotificationPosition position = NotificationPosition.TopRight, 
            int durationMs = 4000,
            Action onClick = null,
            bool isSoundOn = false)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow == null || mainWindow.Content as UIElement == null) return;
                
                var rootElement = mainWindow.Content as UIElement;
                var adornerLayer = AdornerLayer.GetAdornerLayer(rootElement);
                if (adornerLayer == null) return;

                if (!_containers.TryGetValue(position, out var container))
                {
                    container = new NotificationStackContainer(position, adornerLayer, rootElement);
                    _containers[position] = container;
                }

                container.AddNotification(new NotificationRequest
                {
                    Title = title,
                    Description = description,
                    Background = background,
                    Icon = icon,
                    DurationMs = durationMs,
                    OnClick = onClick,
                    IsSoundOn = isSoundOn
                });
            });
        }

        internal static void AddPlayer(MediaPlayer player)
        {
            _activePlayers.Add(player);
            player.MediaEnded += (s, e) =>
            {
                _activePlayers.Remove(player);
            };
        }
    }

    internal class NotificationStackContainer
    {
        private readonly NotificationPosition _position;
        private readonly AdornerLayer _adornerLayer;
        private readonly UIElement _rootElement;
        private readonly ToastAdorner _adorner;
        private readonly StackPanel _stackPanel;
        private SamsungNotificationCard _summaryCard;

        private readonly List<NotificationState> _activeNotifications = new();
        private readonly DispatcherTimer _timer;

        public NotificationStackContainer(NotificationPosition position, AdornerLayer adornerLayer, UIElement rootElement)
        {
            _position = position;
            _adornerLayer = adornerLayer;
            _rootElement = rootElement;

            _stackPanel = new StackPanel
            {
                Background = Brushes.Transparent,
                IsHitTestVisible = true,
                Margin = new Thickness(24)
            };

            switch (position)
            {
                case NotificationPosition.TopLeft:
                    _stackPanel.HorizontalAlignment = HorizontalAlignment.Left; _stackPanel.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case NotificationPosition.TopCenter:
                    _stackPanel.HorizontalAlignment = HorizontalAlignment.Center; _stackPanel.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case NotificationPosition.TopRight:
                    _stackPanel.HorizontalAlignment = HorizontalAlignment.Right; _stackPanel.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case NotificationPosition.BottomLeft:
                    _stackPanel.HorizontalAlignment = HorizontalAlignment.Left; _stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case NotificationPosition.BottomCenter:
                    _stackPanel.HorizontalAlignment = HorizontalAlignment.Center; _stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case NotificationPosition.BottomRight:
                    _stackPanel.HorizontalAlignment = HorizontalAlignment.Right; _stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
            }

            var overlayGrid = new Grid { IsHitTestVisible = false };
            overlayGrid.Children.Add(_stackPanel);

            _adorner = new ToastAdorner(_rootElement, overlayGrid);
            _adornerLayer.Add(_adorner);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            foreach (var state in _activeNotifications.ToList())
            {
                if (!state.IsDismissing && !state.IsHovered && now > state.ExpirationTime && state.HasAppeared)
                {
                    DismissCard(state);
                }
            }
        }

        public void AddNotification(NotificationRequest req)
        {
            var card = new SamsungNotificationCard
            {
                Title = req.Title,
                Description = req.Description,
                CardBackground = req.Background,
                Icon = req.Icon,
                Margin = new Thickness(0, 8, 0, 8)
            };

            var state = new NotificationState 
            { 
                Request = req, 
                Card = card,
                ExpirationTime = DateTime.Now.AddMilliseconds(req.DurationMs)
            };

            card.MouseEnter += (s, e) => state.IsHovered = true;
            card.MouseLeave += (s, e) => { 
                state.IsHovered = false; 
                state.ExpirationTime = DateTime.Now.AddMilliseconds(state.Request.DurationMs); 
            };
            card.CloseClick += (s, e) => DismissCard(state);
            card.CardClick += (s, e) => {
                state.Request.OnClick?.Invoke();
                DismissCard(state);
            };

            _activeNotifications.Add(state);
            UpdateVisuals();
        }

        private void DismissCard(NotificationState state)
        {
            if (state.IsDismissing) return;
            state.IsDismissing = true;

            // Exit animation
            double startX = 0, startY = 0;
            if (_position.ToString().Contains("Left")) startX = -300;
            if (_position.ToString().Contains("Right")) startX = 300;
            if (_position == NotificationPosition.TopCenter) startY = -100;
            if (_position == NotificationPosition.BottomCenter) startY = 100;

            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(250)) { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn } };
            state.Card.BeginAnimation(UIElement.OpacityProperty, fadeOut);

            if (state.Card.RenderTransform is TranslateTransform transform)
            {
                if (startX != 0) transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation(0, startX, TimeSpan.FromMilliseconds(250)) { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn } });
                if (startY != 0) transform.BeginAnimation(TranslateTransform.YProperty, new DoubleAnimation(0, startY, TimeSpan.FromMilliseconds(250)) { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn } });
            }

            Task.Delay(250).ContinueWith(t =>
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    _activeNotifications.Remove(state);
                    if (_stackPanel.Children.Contains(state.Card))
                        _stackPanel.Children.Remove(state.Card);
                    UpdateVisuals();
                });
            });
        }

        private void UpdateVisuals()
        {
            var visibleStates = _activeNotifications.Where(n => !n.IsDismissing).ToList();
            int count = visibleStates.Count;
            bool hasMore = count > 3;

            // Aggiungiamo i primi elementi allo stackpanel
            for (int i = 0; i < Math.Min(count, 3); i++)
            {
                var state = visibleStates[i];
                if (i == 2 && hasMore)
                {
                    if (_stackPanel.Children.Contains(state.Card)) _stackPanel.Children.Remove(state.Card);
                    
                    if (_summaryCard == null)
                    {
                        _summaryCard = new SamsungNotificationCard
                        {
                            CardBackground = new SolidColorBrush(Color.FromArgb(200, 50, 50, 50)),
                            Margin = new Thickness(0, 8, 0, 8)
                        };
                    }
                    _summaryCard.Title = $"+{count - 2} nuove notifiche";
                    _summaryCard.Description = "Hai altre notifiche non visualizzate.";
                    
                    if (!_stackPanel.Children.Contains(_summaryCard))
                    {
                        _stackPanel.Children.Add(_summaryCard);
                        AnimateCardEntrance(_summaryCard, false);
                    }
                }
                else
                {
                    if (!_stackPanel.Children.Contains(state.Card))
                    {
                        _stackPanel.Children.Add(state.Card);
                        if (!state.HasAppeared)
                        {
                            state.HasAppeared = true;
                            // Reset timer al momento in cui entra
                            state.ExpirationTime = DateTime.Now.AddMilliseconds(state.Request.DurationMs);
                            AnimateCardEntrance(state.Card, state.Request.IsSoundOn);
                        }
                    }
                }
            }

            if (!hasMore && _summaryCard != null && _stackPanel.Children.Contains(_summaryCard))
            {
                _stackPanel.Children.Remove(_summaryCard);
            }
        }

        private void AnimateCardEntrance(UIElement card, bool playSound)
        {
            if (playSound)
            {
                try
                {
                    var player = new MediaPlayer();
                    var uri = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "buble_noty.mp3"));
                    player.Open(uri);
                    
                    // Prevent GC during playback
                    SamsungNotificationService.AddPlayer(player);
                    
                    player.Play();
                }
                catch { /* Ignore audio errors */ }
            }

            double startX = 0, startY = 0;
            if (_position.ToString().Contains("Left")) startX = -300;
            if (_position.ToString().Contains("Right")) startX = 300;
            if (_position == NotificationPosition.TopCenter) startY = -100;
            if (_position == NotificationPosition.BottomCenter) startY = 100;

            card.Opacity = 0;
            var transform = new TranslateTransform(startX, startY);
            card.RenderTransform = transform;

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300)) 
                { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
            card.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            if (startX != 0)
            {
                var slideX = new DoubleAnimation(startX, 0, TimeSpan.FromMilliseconds(400)) 
                    { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
                transform.BeginAnimation(TranslateTransform.XProperty, slideX);
            }
            if (startY != 0)
            {
                var slideY = new DoubleAnimation(startY, 0, TimeSpan.FromMilliseconds(400)) 
                    { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
                transform.BeginAnimation(TranslateTransform.YProperty, slideY);
            }
        }
    }

    internal class NotificationState
    {
        public NotificationRequest Request { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool HasAppeared { get; set; }
        public SamsungNotificationCard Card { get; set; }
        public bool IsDismissing { get; set; }
        public bool IsHovered { get; set; }
    }
}
