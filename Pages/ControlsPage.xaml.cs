using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Demo.Pages
{
    public partial class ControlsPage : SamsungUi.Controls.SamsungExpandablePage
    {
        // --- Initialization ---
        public ControlsPage()
        {
            InitializeComponent();
        }

        // --- Event Handlers & Callbacks ---
        private void OpenModal_Click(object sender, RoutedEventArgs e)
        {
            DemoModal.IsOpen = true;
        }

        private void CloseModal_Click(object sender, RoutedEventArgs e)
        {
            DemoModal.IsOpen = false;
        }

        private void ShowLabelToast_Click(object sender, RoutedEventArgs e)
        {
            SamsungUi.Controls.SamsungToastService.Show("Link copied to clipboard");
        }

        private void ShowActionToast_Click(object sender, RoutedEventArgs e)
        {
            SamsungUi.Controls.SamsungToastService.Show("File downloaded successfully", "Open", () => 
            {
                SamsungUi.Controls.SamsungToastService.Show("Opening file...");
            });
        }

        private void ShowPersistentActionToast_Click(object sender, RoutedEventArgs e)
        {
            SamsungUi.Controls.SamsungToastService.Show("Update required to continue", "UPDATE", () => 
            {
                SamsungUi.Controls.SamsungToastService.Show("Updating...");
            }, autoClose: false);
        }

        private void OnSaveAccountClick(object sender, RoutedEventArgs e)
        {
            AccountExpander.IsExpanded = false;
            SamsungUi.Controls.SamsungToastService.Show("Accout Updated.");
        }

        private void OnCancelAccountClick(object sender, RoutedEventArgs e)
        {
            AccountExpander.IsExpanded = false;
        }

        private async void OnShowBusyIndicatorClick(object sender, RoutedEventArgs e)
        {
            SamsungUi.Controls.SamsungBusyIndicator.Show("Loading...");
            await System.Threading.Tasks.Task.Delay(2000);
            SamsungUi.Controls.SamsungBusyIndicator.Hide();
        }
    }
}
