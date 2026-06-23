using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class SamsungTreeView : TreeView
    {
        static SamsungTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungTreeView), new FrameworkPropertyMetadata(typeof(SamsungTreeView)));
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
