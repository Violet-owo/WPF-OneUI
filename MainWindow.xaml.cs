using System.Windows;
using System.Windows.Controls;
using SamsungUi.Demo.Pages;
using SamsungUi.Controls;

namespace SamsungUi.Demo
{
    public partial class MainWindow : SamsungWindow
    {
        // --- Fields ---
        private ControlsPage _controlsPage;
        private ModulesPage _modulesPage;
        private StopwatchPage _stopwatchPage;
        private GalleryPage _galleryPage;
        private SettingsPage _settingsPage;

        // --- Initialization ---
        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize pages
            _controlsPage = new ControlsPage();
            _modulesPage = new ModulesPage();
            _stopwatchPage = new StopwatchPage();
            _galleryPage = new GalleryPage();
            _settingsPage = new SettingsPage();

            // Load initial page
            MainFrame.Navigate(_controlsPage);
        }

        // --- Event Handlers & Callbacks ---

        private void NavBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavBar == null || MainFrame == null) return;

            var selectedItem = NavBar.SelectedItem as SamsungUi.Controls.SamsungNavigationBarItem;
            if (selectedItem == null) return;

            switch (selectedItem.Text)
            {
                case "Controls":
                    MainFrame.Navigate(_controlsPage);
                    break;
                case "Modules":
                    MainFrame.Navigate(_modulesPage);
                    break;
                case "Stopwatch":
                    MainFrame.Navigate(_stopwatchPage);
                    break;
                case "Gallery":
                    MainFrame.Navigate(_galleryPage);
                    break;
                case "Settings":
                    MainFrame.Navigate(_settingsPage);
                    break;
            }
        }

        // --- Methods ---

        public void FilterAllPages(string query)
        {
            query = query?.ToLower().Trim() ?? "";
            
            FilterCards(_controlsPage, query);
            FilterCards(_modulesPage, query);
            FilterCards(_stopwatchPage, query);
            FilterCards(_galleryPage, query);
        }

        private void FilterCards(DependencyObject parent, string query)
        {
            if (parent == null) return;
            foreach (var childObj in LogicalTreeHelper.GetChildren(parent))
            {
                if (childObj is DependencyObject child)
                {
                    if (child is SamsungCard card)
                    {
                        if (string.IsNullOrEmpty(query))
                        {
                            card.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            bool match = SearchTextBlocks(card, query);
                            card.Visibility = match ? Visibility.Visible : Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        FilterCards(child, query);
                    }
                }
            }
        }

        private bool SearchTextBlocks(DependencyObject parent, string query)
        {
            if (parent == null) return false;
            if (parent is TextBlock textBlock && textBlock.Text != null && textBlock.Text.ToLower().Contains(query))
                return true;
            
            foreach (var childObj in LogicalTreeHelper.GetChildren(parent))
            {
                if (childObj is DependencyObject child)
                {
                    if (SearchTextBlocks(child, query))
                        return true;
                }
            }
            return false;
        }
    }
}