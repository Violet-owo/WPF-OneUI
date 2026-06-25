#pragma warning disable CS8600, CS8601, CS8602, CS8603, CS8604, CS8618, CS8622, CS8625
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;


namespace SamsungUi.Controls
{
    public class SamsungWindow : Window
    {
        public static readonly RoutedCommand OpenDocsCommand = new RoutedCommand();

        public SamsungWindow()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
            CommandBindings.Add(new CommandBinding(OpenDocsCommand, OnOpenDocs));
        }

        private static ScrollViewer _activeScrollViewer;
        private static System.DateTime _lastScrollTime;

        static SamsungWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungWindow), new FrameworkPropertyMetadata(typeof(SamsungWindow)));
            
            // Global scroll inertia management (anti scroll-hijacking)
            EventManager.RegisterClassHandler(typeof(ScrollViewer), UIElement.PreviewMouseWheelEvent, new MouseWheelEventHandler(OnPreviewMouseWheel_Enforce));
        }

        private static void OnPreviewMouseWheel_Enforce(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled) return;
            var sv = sender as ScrollViewer;
            if (sv == null) return;

            var now = System.DateTime.Now;

            // If we have an active lock and it has not expired (400ms)
            if (_activeScrollViewer != null && (now - _lastScrollTime).TotalMilliseconds < 400)
            {
                if (sv == _activeScrollViewer) 
                {
                    _lastScrollTime = now;
                    return; // Let the event pass for the locked ScrollViewer
                }

                // Block the tunneling event for other ScrollViewers (including the Parent)
                e.Handled = true;
                
                // Re-send the bubbling event directly to the locked ScrollViewer (so the scroll is fluid/native)
                var args = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = _activeScrollViewer
                };
                _activeScrollViewer.RaiseEvent(args);
                
                // Check if the locked scroll viewer successfully scrolled natively
                if (args.Handled)
                {
                    _lastScrollTime = now;
                }
                else
                {
                    // Reached the end of scroll, unlock to allow the Parent to scroll
                    _activeScrollViewer = null;
                }
                return;
            }

            // No active lock. We search for the appropriate target (the innermost that can scroll)
            var innermostTarget = GetScrollableParent(e.OriginalSource as DependencyObject, e.Delta);
            if (innermostTarget != null && sv == innermostTarget)
            {
                _activeScrollViewer = innermostTarget;
                _lastScrollTime = now;
            }
        }

        private static ScrollViewer GetScrollableParent(DependencyObject child, int delta)
        {
            var parent = child;
            while (parent != null)
            {
                if (parent is ScrollViewer sv && sv.VerticalScrollBarVisibility != ScrollBarVisibility.Disabled)
                {
                    if (delta > 0 && sv.VerticalOffset > 0) return sv;
                    if (delta < 0 && sv.VerticalOffset < sv.ScrollableHeight) return sv;
                }
                if (parent is Visual || parent is System.Windows.Media.Media3D.Visual3D)
                    parent = VisualTreeHelper.GetParent(parent);
                else
                    parent = LogicalTreeHelper.GetParent(parent);
            }
            return null;
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        private void OnOpenDocs(object target, ExecutedRoutedEventArgs e)
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/Violet-owo/WPF-OneUI/blob/main/docs/Home.md",
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
    }
}
