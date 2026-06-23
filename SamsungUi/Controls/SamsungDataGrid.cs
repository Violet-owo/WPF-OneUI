using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class SamsungDataGrid : DataGrid
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SamsungDataGrid), new PropertyMetadata(new CornerRadius(16)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(SamsungDataGrid), new PropertyMetadata(false));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        static SamsungDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungDataGrid), new FrameworkPropertyMetadata(typeof(SamsungDataGrid)));
        }

        public SamsungDataGrid()
        {
            // Default optimizations and settings for One UI style
            AutoGenerateColumns = false;
            CanUserAddRows = false;
            CanUserDeleteRows = false;
            GridLinesVisibility = DataGridGridLinesVisibility.All;
            HeadersVisibility = DataGridHeadersVisibility.Column;
            RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
            SelectionMode = DataGridSelectionMode.Extended;
            SelectionUnit = DataGridSelectionUnit.FullRow;
            
            // Enable UI Virtualization for massive datasets
            VirtualizingPanel.SetIsVirtualizing(this, true);
            VirtualizingPanel.SetVirtualizationMode(this, VirtualizationMode.Recycling);
            EnableRowVirtualization = true;
            EnableColumnVirtualization = true;
        }

        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            // Intercept sorting to show a busy indicator, since sorting 100k rows locks the UI thread.
            eventArgs.Handled = true;
            
            var column = eventArgs.Column;
            // Determine new sort direction
            var direction = (column.SortDirection != System.ComponentModel.ListSortDirection.Ascending) ?
                            System.ComponentModel.ListSortDirection.Ascending : System.ComponentModel.ListSortDirection.Descending;
            
            // Show the loading overlay
            IsLoading = true;

            // Defer the heavy sorting operation to a background dispatcher priority.
            // This gives the UI thread a moment to actually render the IsLoading overlay before it gets locked by the sort.
            Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    // Apply the visual sort direction to the column header
                    column.SortDirection = direction;

                    var view = System.Windows.Data.CollectionViewSource.GetDefaultView(ItemsSource);
                    if (view != null)
                    {
                        using (view.DeferRefresh())
                        {
                            view.SortDescriptions.Clear();
                            view.SortDescriptions.Add(new System.ComponentModel.SortDescription(column.SortMemberPath, direction));
                        }
                    }
                }
                finally
                {
                    IsLoading = false;
                }
            }, System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}
