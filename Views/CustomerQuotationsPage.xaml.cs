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
            try
            {
                using (var context = new AppDbContext())
                {
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
                MessageBoxResult result = MessageBox.Show("Do you want to accept this quotation?", "Respond to Quotation", MessageBoxButton.YesNo);
                string status = result == MessageBoxResult.Yes ? "Accepted by Customer" : "Rejected by Customer";
                UpdateCustomerQuotationStatus(quotationId, status);
            }
        }

        private void UpdateCustomerQuotationStatus(int quotationId, string status)
        {
            var quotation = Quotations.FirstOrDefault(q => q.Id == quotationId);
            if (quotation != null)
            {
                quotation.Status = status;
                RefreshQuotationGrid();
                NotifyQuotationOfficer(quotationId, status);
            }
        }

        private void NotifyQuotationOfficer(int quotationId, string status)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var quotation = context.Quotations.FirstOrDefault(q => q.Id == quotationId);
                    if (quotation != null)
                    {
                        quotation.Status = status;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while notifying the officer: {ex.Message}", "Error");
            }
        }

        private void LoadNotifications()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading notifications: {ex.Message}", "Error");
            }
        }

        private void RefreshQuotationGrid()
        {
            LoadQuotations();
        }
    }
}
