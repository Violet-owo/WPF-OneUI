using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class IsEllipsisConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.ToString() == "...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EqualityConverter : System.Windows.Data.IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length < 2) return false;
            return object.Equals(values[0], values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SamsungPagination : Control
    {
        // Event fired when a page changes
        public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(PageChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(SamsungPagination));

        public event RoutedPropertyChangedEventHandler<int> PageChanged
        {
            add { AddHandler(PageChangedEvent, value); }
            remove { RemoveHandler(PageChangedEvent, value); }
        }

        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            nameof(CurrentPage), typeof(int), typeof(SamsungPagination), 
            new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentPageChanged, CoerceCurrentPage));

        public static readonly DependencyProperty TotalPagesProperty = DependencyProperty.Register(
            nameof(TotalPages), typeof(int), typeof(SamsungPagination), 
            new FrameworkPropertyMetadata(1, OnTotalPagesChanged, CoerceTotalPages));

        public static readonly DependencyProperty VisiblePageButtonCountProperty = DependencyProperty.Register(
            nameof(VisiblePageButtonCount), typeof(int), typeof(SamsungPagination), 
            new PropertyMetadata(5, OnVisiblePageButtonCountChanged));

        // Read-only collection of page buttons to render
        private static readonly DependencyPropertyKey PageButtonsPropertyKey = DependencyProperty.RegisterReadOnly(
            "PageButtons", typeof(IEnumerable<object>), typeof(SamsungPagination), new PropertyMetadata(null));

        public static readonly DependencyProperty PageButtonsProperty = PageButtonsPropertyKey.DependencyProperty;

        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public int TotalPages
        {
            get => (int)GetValue(TotalPagesProperty);
            set => SetValue(TotalPagesProperty, value);
        }

        public int VisiblePageButtonCount
        {
            get => (int)GetValue(VisiblePageButtonCountProperty);
            set => SetValue(VisiblePageButtonCountProperty, value);
        }

        public IEnumerable<object> PageButtons
        {
            get => (IEnumerable<object>)GetValue(PageButtonsProperty);
            private set => SetValue(PageButtonsPropertyKey, value);
        }

        static SamsungPagination()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungPagination), new FrameworkPropertyMetadata(typeof(SamsungPagination)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            if (Template.FindName("PART_PrevButton", this) is System.Windows.Controls.Primitives.ButtonBase prevBtn)
                prevBtn.Click += (s, e) => { if (CurrentPage > 1) CurrentPage--; };

            if (Template.FindName("PART_NextButton", this) is System.Windows.Controls.Primitives.ButtonBase nextBtn)
                nextBtn.Click += (s, e) => { if (CurrentPage < TotalPages) CurrentPage++; };

            // Listen to any bubbling click events from the ItemsControl buttons
            this.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(OnAnyButtonClick));

            UpdatePageButtons();
        }

        private void OnAnyButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement fe && fe.DataContext != null)
            {
                // If the user clicked a button inside the items control, its DataContext is the page number or "..."
                if (fe.DataContext is int page && fe.Name == "PART_PageButton")
                {
                    CurrentPage = page;
                    e.Handled = true;
                }
            }
        }

        private static object CoerceTotalPages(DependencyObject d, object baseValue)
        {
            return Math.Max(1, (int)baseValue);
        }

        private static void OnTotalPagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pagination = (SamsungPagination)d;
            pagination.CoerceValue(CurrentPageProperty);
            pagination.UpdatePageButtons();
        }

        private static object CoerceCurrentPage(DependencyObject d, object baseValue)
        {
            var pagination = (SamsungPagination)d;
            int current = (int)baseValue;
            if (current < 1) return 1;
            if (current > pagination.TotalPages) return pagination.TotalPages;
            return current;
        }

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pagination = (SamsungPagination)d;
            pagination.UpdatePageButtons();
            
            var args = new RoutedPropertyChangedEventArgs<int>((int)e.OldValue, (int)e.NewValue)
            {
                RoutedEvent = PageChangedEvent
            };
            pagination.RaiseEvent(args);
        }

        private static void OnVisiblePageButtonCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SamsungPagination)d).UpdatePageButtons();
        }

        private void UpdatePageButtons()
        {
            var buttons = new List<object>();

            int total = TotalPages;
            int current = CurrentPage;
            int visibleCount = VisiblePageButtonCount;

            if (total <= visibleCount + 2) // If small enough, show all
            {
                for (int i = 1; i <= total; i++) buttons.Add(i);
            }
            else
            {
                int half = visibleCount / 2;
                int start = current - half;
                int end = current + half;

                if (start <= 2)
                {
                    start = 1;
                    end = visibleCount;
                }
                else if (end >= total - 1)
                {
                    start = total - visibleCount + 1;
                    end = total;
                }

                if (start > 1)
                {
                    buttons.Add(1);
                    if (start > 2) buttons.Add("...");
                }

                for (int i = start; i <= end; i++) buttons.Add(i);

                if (end < total)
                {
                    if (end < total - 1) buttons.Add("...");
                    buttons.Add(total);
                }
            }

            PageButtons = buttons;
        }
    }
}
