using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Rappresenta una pagina con il comportamento di scorrimento tipico della One UI di Samsung.
    /// Il titolo grande svanisce scorrendo verso il basso, mentre un titolo più piccolo appare appiccicato in alto.
    /// </summary>
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_SmallHeader", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_BigHeaderTitle", Type = typeof(UIElement))]
    public class SamsungExpandablePage : Page
    {
        private ScrollViewer _scrollViewer;
        private UIElement _smallHeader;
        private UIElement _bigHeaderTitle;

        /// <summary>
        /// Identifica la dependency property per il titolo della pagina.
        /// </summary>
        public static readonly DependencyProperty PageTitleProperty =
            DependencyProperty.Register("PageTitle", typeof(string), typeof(SamsungExpandablePage), new PropertyMetadata("Page Title"));

        /// <summary>
        /// Ottiene o imposta il titolo che viene visualizzato sia nell'intestazione espansa che in quella compatta.
        /// </summary>
        public string PageTitle
        {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }

        static SamsungExpandablePage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungExpandablePage), new FrameworkPropertyMetadata(typeof(SamsungExpandablePage)));
        }

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

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_scrollViewer == null) return;

            // Logica di transizione One UI:
            // Lo scroll Y varia da 0 a circa 100 per l'effetto completo.
            double offset = _scrollViewer.VerticalOffset;
            double threshold = 60.0; // Valore di scroll a cui inizia la transizione fissa

            // Calcolo Opacità Titolo Grande (svanisce scorrendo in giù)
            if (_bigHeaderTitle != null)
            {
                double bigOpacity = 1.0 - (offset / threshold);
                _bigHeaderTitle.Opacity = Math.Max(0, Math.Min(1, bigOpacity));
                
                // Effetto parallasse/scala opzionale
                double scale = 1.0 - (offset * 0.002);
                scale = Math.Max(0.8, scale);
                _bigHeaderTitle.RenderTransform = new ScaleTransform(scale, scale) { CenterY = 20 };
            }

            // Calcolo Opacità Titolo Piccolo (appare quando si supera la soglia)
            if (_smallHeader != null)
            {
                double smallOpacity = (offset - (threshold * 0.5)) / (threshold * 0.5);
                _smallHeader.Opacity = Math.Max(0, Math.Min(1, smallOpacity));
            }
        }
    }
}
