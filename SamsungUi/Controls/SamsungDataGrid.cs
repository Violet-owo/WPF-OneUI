using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A Samsung One UI style DataGrid with rounded corners, custom scrollbars, and an integrated loading overlay.
    /// </summary>
    public class SamsungDataGrid : DataGrid
    {
        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SamsungDataGrid), new PropertyMetadata(new CornerRadius(16)));

        /// <summary>
        /// Gets or sets the corner radius of the data grid's outer border.
        /// </summary>
        /// <value>A <see cref="System.Windows.CornerRadius"/> value. Default is 16.</value>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsLoading"/> dependency property.
        /// </summary>

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(SamsungDataGrid), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether the data grid is currently loading or processing data.
        /// When true, an animated loading overlay is displayed over the content.
        /// </summary>
        /// <value><c>true</c> to show the loading overlay; otherwise, <c>false</c>.</value>
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
