using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style ListView (inherits from ListBox) that automatically 
    /// styles its items with separators and handles scrolling.
    /// </summary>
    public class SamsungListView : ListBox
    {
        static SamsungListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungListView), new FrameworkPropertyMetadata(typeof(SamsungListView)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SamsungListViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SamsungListViewItem;
        }
    }
}
