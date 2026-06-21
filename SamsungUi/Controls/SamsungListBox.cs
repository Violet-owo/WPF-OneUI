using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// ListBox in stile Samsung One UI.
    /// </summary>
    public class SamsungListBox : ListBox
    {
        static SamsungListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungListBox), new FrameworkPropertyMetadata(typeof(SamsungListBox)));
        }
        
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
