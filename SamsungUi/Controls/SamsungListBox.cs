using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style ListBox.
    /// </summary>
    public class SamsungListBox : ListBox
    {
        // --- Initialization ---

        static SamsungListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungListBox), new FrameworkPropertyMetadata(typeof(SamsungListBox)));
        }
        
        // --- Methods ---

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SamsungListBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SamsungListBoxItem;
        }
    }
}
