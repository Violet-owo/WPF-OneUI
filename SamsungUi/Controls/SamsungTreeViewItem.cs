using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class SamsungTreeViewItem : TreeViewItem
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SamsungTreeViewItem), new PropertyMetadata(new CornerRadius(8)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        static SamsungTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTreeViewItem), new FrameworkPropertyMetadata(typeof(SamsungTreeViewItem)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SamsungTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SamsungTreeViewItem;
        }
    }
}
