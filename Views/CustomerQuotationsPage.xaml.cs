using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                using (var context = new AppDbContext())
                {
                    // Load quotations associated with the current customer
                    var quotations = context.Quotations
                        .Where(q => q.CustomerId == _customerId)
                        .ToList();
                    QuotationsDataGrid.ItemsSource = quotations;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading quotations: {ex.Message}", "Error");
            }
        }

        private void RespondButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int quotationId)
            {
                // Ask the customer if they want to accept or reject the quotation
                MessageBoxResult result = MessageBox.Show("Do you want to accept this quotation?", "Respond to Quotation", MessageBoxButton.YesNo);
                string status = result == MessageBoxResult.Yes ? "Accepted by Customer" : "Rejected by Customer";
                UpdateCustomerQuotationStatus(quotationId, status);
            }
        }

        private void UpdateCustomerQuotationStatus(int quotationId, string status)
        {
            using (var context = new AppDbContext())
            {
                var quotation = context.Quotations.FirstOrDefault(q => q.Id == quotationId);
                if (quotation != null)
                {
                    // Update the status of the quotation based on customer response
                    quotation.Status = status;
                    context.SaveChanges();
                    NotifyQuotationOfficer(quotationId, status);
                    LoadQuotations(); // Refresh the quotations grid to reflect the new status
                }
            }
        }

        private void NotifyQuotationOfficer(int quotationId, string status)
        {
            using (var context = new AppDbContext())
            {
                var quotation = context.Quotations.Include(q => q.Customer).FirstOrDefault(q => q.Id == quotationId);
                if (quotation != null)
                {
                    // Send a notification to the Quotation officer about the customer's response
                    var notification = new Notification
                    {
                        CustomerId = quotation.CustomerId,
                        Message = $"Customer {quotation.Customer.FullName} has {status.ToLower()} the quotation with ID {quotationId}.",
                        DateCreated = DateTime.Now,
                        IsRead = false
                    };
                    context.Notifications.Add(notification);
                    context.SaveChanges();
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
            LoadQuotations();
        }
    }
}
