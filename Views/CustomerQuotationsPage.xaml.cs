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
            // Implementation for notifying the Quotation Officer of the customer's decision
            // This could involve updating the officer's dashboard or triggering an in-app notification
        }

        private void RefreshQuotationGrid()
        {
            QuotationsDataGrid.ItemsSource = null;
            QuotationsDataGrid.ItemsSource = Quotations; // Reassign the data source to refresh
        }

    }
}