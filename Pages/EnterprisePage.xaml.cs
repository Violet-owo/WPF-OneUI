using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SamsungUi.Controls;

namespace SamsungUi.Demo.Pages
{
    public partial class EnterprisePage : Page
    {
        public EnterprisePage()
        {
            InitializeComponent();
        }

        private void SamsungFileDropZone_FilesDropped(object sender, string[] e)
        {
            if (e != null && e.Length > 0)
            {
                // Just display the name of the first file for demonstration
                string fileName = Path.GetFileName(e[0]);
                UploadStatusText.Text = $"Uploaded: {fileName}";
                
                if (e.Length > 1)
                {
                    UploadStatusText.Text += $" and {e.Length - 1} more";
                }

                SamsungToastService.Show("Upload Success", $"Successfully received {e.Length} files.");
            }
        }

        private void SamsungFileDropZone_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Simulate clicking to browse for file
            UploadStatusText.Text = "Browsing for files... (Not implemented in demo)";
        }

        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            UploadStatusText.Text = "Uploading file from context menu...";
        }

        private void ClearArea_Click(object sender, RoutedEventArgs e)
        {
            UploadStatusText.Text = string.Empty;
        }

        private async void LoadData_Click(object sender, RoutedEventArgs e)
        {
            // Show loading state
            EnterpriseGrid.IsLoading = true;
            GridPagination.Visibility = Visibility.Collapsed;

            // Generate 100,000 rows in background thread to avoid freezing UI
            var data = await System.Threading.Tasks.Task.Run(() =>
            {
                var list = new System.Collections.Generic.List<Employee>();
                var rand = new Random();
                var depts = new[] { "Engineering", "Marketing", "HR", "Sales", "Design" };
                var roles = new[] { "Manager", "Senior", "Junior", "Intern", "Director" };
                var statuses = new[] { "Active", "On Leave", "Remote", "Terminated" };

                // Generate 100,000 rows to prove UI virtualization works flawlessly
                for (int i = 1; i <= 100000; i++)
                {
                    string status = statuses[rand.Next(statuses.Length)];
                    System.Windows.Media.Brush colorBrush;
                    switch(status)
                    {
                        case "Active": colorBrush = (System.Windows.Media.Brush)Application.Current.Resources["OneUiSuccessBrush"]; break;
                        case "On Leave": colorBrush = (System.Windows.Media.Brush)Application.Current.Resources["OneUiWarningBrush"]; break;
                        case "Terminated": colorBrush = (System.Windows.Media.Brush)Application.Current.Resources["OneUiDangerBrush"]; break;
                        default: colorBrush = (System.Windows.Media.Brush)Application.Current.Resources["OneUiPrimaryBrush"]; break;
                    }

                    list.Add(new Employee
                    {
                        Id = $"EMP-{i:D6}",
                        Name = $"Employee {i}",
                        Department = depts[rand.Next(depts.Length)],
                        Role = roles[rand.Next(roles.Length)],
                        Status = status,
                        PerformanceScore = rand.Next(10, 101)
                    });
                }
                
                // Simulate network/db delay
                System.Threading.Thread.Sleep(2000);
                
                return list;
            });

            // Bind the data
            EnterpriseGrid.ItemsSource = data;
            
            // Turn off loading state
            EnterpriseGrid.IsLoading = false;
            
            SamsungToastService.Show("Data Loaded", "100,000 records loaded successfully.");
        }

        private bool _isGridExpanded = false;

        private void ExpandGrid_Click(object sender, RoutedEventArgs e)
        {
            _isGridExpanded = !_isGridExpanded;

            if (_isGridExpanded)
            {
                // Expand to full page
                GridCard.SetValue(Grid.RowProperty, 0);
                GridCard.SetValue(Grid.RowSpanProperty, 3);
                System.Windows.Controls.Panel.SetZIndex(GridCard, 999);
                GridCard.Margin = new Thickness(0);
                EnterpriseGrid.MaxHeight = double.PositiveInfinity;
                ExpandButton.Content = "\uE73F"; // BackToWindow Icon
                ExpandButton.ToolTip = "Restore";
            }
            else
            {
                // Restore to normal
                GridCard.SetValue(Grid.RowProperty, 2);
                GridCard.SetValue(Grid.RowSpanProperty, 1);
                System.Windows.Controls.Panel.SetZIndex(GridCard, 0);
                GridCard.Margin = new Thickness(0);
                EnterpriseGrid.MaxHeight = 400;
                ExpandButton.Content = "\uE740"; // FullScreen Icon
                ExpandButton.ToolTip = "Expand to Fullscreen";
            }
        }

        // --- PAGINATION LOGIC ---
        private List<Employee> _paginatedDataset;
        private const int PAGE_SIZE = 50;

        private async void LoadPagination_Click(object sender, RoutedEventArgs e)
        {
            EnterpriseGrid.IsLoading = true;
            GridPagination.Visibility = Visibility.Collapsed;

            // Generate 500 rows
            _paginatedDataset = await System.Threading.Tasks.Task.Run(() =>
            {
                var list = new List<Employee>();
                var rand = new Random();
                var depts = new[] { "Engineering", "Marketing", "Sales", "HR", "Finance", "Legal", "Operations" };
                var roles = new[] { "Junior", "Mid-Level", "Senior", "Lead", "Manager", "Director" };
                var statuses = new[] { "Active", "On Leave", "Terminated", "Remote" };

                for (int i = 1; i <= 500; i++)
                {
                    list.Add(new Employee
                    {
                        Id = $"EMP-{i:D6}",
                        Name = $"Employee {i}",
                        Department = depts[rand.Next(depts.Length)],
                        Role = roles[rand.Next(roles.Length)],
                        Status = statuses[rand.Next(statuses.Length)],
                        PerformanceScore = rand.Next(10, 101)
                    });
                }
                
                System.Threading.Thread.Sleep(1000); // Simulate network
                return list;
            });

            // Setup Pagination Control
            GridPagination.TotalPages = (int)Math.Ceiling(_paginatedDataset.Count / (double)PAGE_SIZE);
            GridPagination.CurrentPage = 1;
            GridPagination.Visibility = Visibility.Visible;

            LoadPageData(1);

            SamsungToastService.Show("Pagination Ready", "500 records prepared. Showing page 1.");
        }

        private async void GridPagination_PageChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (_paginatedDataset == null) return;
            
            // Show loading briefly to simulate fetching next page from DB
            EnterpriseGrid.IsLoading = true;
            
            await System.Threading.Tasks.Task.Delay(300); // Simulate small network delay
            
            LoadPageData(e.NewValue);
        }

        private void LoadPageData(int page)
        {
            int skip = (page - 1) * PAGE_SIZE;
            var pageData = _paginatedDataset.Skip(skip).Take(PAGE_SIZE).ToList();
            
            EnterpriseGrid.ItemsSource = pageData;
            EnterpriseGrid.IsLoading = false;
        }

        // --- CONTEXT MENU & MODALS LOGIC ---
        private Employee _selectedEmployeeForAction;

        private void EditRow_Click(object sender, RoutedEventArgs e)
        {
            if (EnterpriseGrid.SelectedItem is Employee emp)
            {
                _selectedEmployeeForAction = emp;
                
                // Populate modal
                EditNameBox.Text = emp.Name;
                EditRoleBox.Text = emp.Role;
                EditPerformanceSlider.Value = emp.PerformanceScore;
                
                // Select combobox items
                foreach(ComboBoxItem item in EditDeptBox.Items)
                {
                    if (item.Content?.ToString() == emp.Department)
                    {
                        EditDeptBox.SelectedItem = item;
                        break;
                    }
                }

                foreach(ComboBoxItem item in EditStatusBox.Items)
                {
                    if (item.Content?.ToString() == emp.Status)
                    {
                        EditStatusBox.SelectedItem = item;
                        break;
                    }
                }

                EditEmployeeModal.IsOpen = true;
            }
        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            EditEmployeeModal.IsOpen = false;
        }

        private void SaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployeeForAction != null)
            {
                _selectedEmployeeForAction.Name = EditNameBox.Text;
                _selectedEmployeeForAction.Role = EditRoleBox.Text;
                _selectedEmployeeForAction.PerformanceScore = (int)EditPerformanceSlider.Value;

                if (EditDeptBox.SelectedItem is ComboBoxItem deptItem)
                    _selectedEmployeeForAction.Department = deptItem.Content?.ToString();

                if (EditStatusBox.SelectedItem is ComboBoxItem statusItem)
                    _selectedEmployeeForAction.Status = statusItem.Content?.ToString();

                // Refresh Grid
                EnterpriseGrid.Items.Refresh();
                
                EditEmployeeModal.IsOpen = false;

                // Show Notification - exactly like ModulesPage
                EditSuccessNotification.Description = $"{_selectedEmployeeForAction.Name}'s profile has been updated.";
                EditSuccessNotification.Show();
            }
        }

        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (EnterpriseGrid.SelectedItem is Employee emp)
            {
                _selectedEmployeeForAction = emp;
                DeleteConfirmModal.IsOpen = true;
            }
        }

        private void CancelDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteConfirmModal.IsOpen = false;
        }

        private void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployeeForAction != null)
            {
                // Remove from current view source
                var source = EnterpriseGrid.ItemsSource as System.Collections.IList;
                if (source != null && source.Contains(_selectedEmployeeForAction))
                {
                    source.Remove(_selectedEmployeeForAction);
                    
                    // If paginated, remove from the main list too
                    if (_paginatedDataset != null && _paginatedDataset.Contains(_selectedEmployeeForAction))
                    {
                        _paginatedDataset.Remove(_selectedEmployeeForAction);
                    }

                    EnterpriseGrid.Items.Refresh();
                    
                    DeleteConfirmModal.IsOpen = false;

                    // Show Notification - exactly like ModulesPage
                    DeleteSuccessNotification.Description = $"{_selectedEmployeeForAction.Name} has been removed.";
                    DeleteSuccessNotification.Show();
                }
            }
        }
    }

    // Dummy model for demonstration
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public int PerformanceScore { get; set; }
    }
}
