using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Represents a page with the scrolling behavior typical of Samsung's One UI.
    /// The large title fades out when scrolling down, while a smaller title appears pinned to the top.
    /// </summary>
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_SmallHeader", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_BigHeaderTitle", Type = typeof(UIElement))]
    public class SamsungExpandablePage : Page
    {
        // --- Fields ---
        
        private ScrollViewer? _scrollViewer;
        private UIElement? _smallHeader;
        private UIElement? _bigHeaderTitle;

        // --- Dependency Properties ---

        /// <summary>
        /// Identifies the PageTitle dependency property.
        /// </summary>
        public static readonly DependencyProperty PageTitleProperty =
            DependencyProperty.Register("PageTitle", typeof(string), typeof(SamsungExpandablePage), new PropertyMetadata("Page Title"));

        // --- Properties ---

        /// <summary>
        /// Gets or sets the title that is displayed in both the expanded and compact headers.
        /// </summary>
        public string PageTitle
        {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }

        // --- Initialization ---

        static SamsungExpandablePage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungExpandablePage), new FrameworkPropertyMetadata(typeof(SamsungExpandablePage)));
        }

        // --- Methods ---

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }

            _scrollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            _smallHeader = GetTemplateChild("PART_SmallHeader") as UIElement;
            _bigHeaderTitle = GetTemplateChild("PART_BigHeaderTitle") as UIElement;

            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
        }

        // --- Event Handlers & Callbacks ---

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_scrollViewer == null) return;

            // One UI transition logic:
            // Y scroll varies from 0 to about 100 for the full effect.
            double offset = _scrollViewer.VerticalOffset;
            double threshold = 60.0; // Scroll value at which the fixed transition starts

            // Large Title Opacity calculation (fades out scrolling down)
            if (_bigHeaderTitle != null)
            {
                double bigOpacity = 1.0 - (offset / threshold);
                _bigHeaderTitle.Opacity = Math.Max(0, Math.Min(1, bigOpacity));
                
                // Optional parallax/scale effect
                double scale = 1.0 - (offset * 0.002);
                scale = Math.Max(0.8, scale);
                _bigHeaderTitle.RenderTransform = new ScaleTransform(scale, scale) { CenterY = 20 };
            }

            // Small Title Opacity calculation (appears when threshold is crossed)
            if (_smallHeader != null)
            {
                double smallOpacity = (offset - (threshold * 0.5)) / (threshold * 0.5);
                _smallHeader.Opacity = Math.Max(0, Math.Min(1, smallOpacity));
            }
        }
    }
}
