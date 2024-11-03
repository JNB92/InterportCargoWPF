using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;

namespace InterportCargoWPF.Views
{
    public partial class CustomerQuotationsPage : Page
    {
        public ObservableCollection<Quotation> Quotations { get; set; }
        private int _customerId;

        public CustomerQuotationsPage(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            LoadQuotations();
        }

        private void LoadQuotations()
        {
            using (var context = new AppDbContext())
            {
                // Load only quotations for the logged-in customer
                var quotations = context.Quotations
                    .Where(q => q.CustomerId == _customerId)
                    .ToList();

                QuotationsDataGrid.ItemsSource = quotations;
            }
        }

        private void RespondButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int quotationId)
            {
                // Show confirmation dialog
                MessageBoxResult result = MessageBox.Show("Do you want to accept this quotation?", "Respond to Quotation", MessageBoxButton.YesNo);
                string status = result == MessageBoxResult.Yes ? "Accepted by Customer" : "Rejected by Customer";
        
                // Update the quotation status based on customer response
                UpdateCustomerQuotationStatus(quotationId, status);
            }
        }

        private void UpdateCustomerQuotationStatus(int quotationId, string status)
        {
            // Update the status in the data source (in-memory collection or database)
            var quotation = Quotations.FirstOrDefault(q => q.Id == quotationId);
            if (quotation != null)
            {
                quotation.Status = status;
                RefreshQuotationGrid(); // Refresh the DataGrid to show the updated status
        
                // Notify the Quotation Officer
                NotifyQuotationOfficer(quotationId, status);
            }
        }

        private void NotifyQuotationOfficer(int quotationId, string status)
        {
            using (var context = new AppDbContext())
            {
                var quotation = context.Quotations.FirstOrDefault(q => q.Id == quotationId);
                if (quotation != null)
                {
                    quotation.Status = status;
                    context.SaveChanges();
            
                    // Optionally, add a notification record or update Quotation Officer's dashboard
                    // This is a placeholder; implementation will depend on notification logic
                }
            }
        }

        private void LoadNotifications()
        {
            using (var context = new AppDbContext())
            {
                var notifications = context.Notifications
                    .Where(n => n.CustomerId == _customerId && !n.IsRead)
                    .OrderByDescending(n => n.DateCreated)
                    .ToList();

                if (notifications.Any())
                {
                    string notificationMessage = string.Join("\n", notifications.Select(n => n.Message));
                    MessageBox.Show(notificationMessage, "Notifications");

                    // Mark notifications as read
                    foreach (var notification in notifications)
                    {
                        notification.IsRead = true;
                    }

                    context.SaveChanges();
                }
            }
        }

        private void RefreshQuotationGrid()
        {
            LoadQuotations(); // Reload from the database to get the latest data
        }

    }
}