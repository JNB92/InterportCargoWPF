using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace InterportCargoWPF.Views
{
    public partial class EmployeeDashboardPage : Page
    {
        public ObservableCollection<Quotation> Quotations { get; set; }

        public EmployeeDashboardPage()
        {
            InitializeComponent();
            LoadQuotations();
            LoadNotifications();
        }

        private void LoadQuotations()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    // Fetch quotations from the database and include related Customer data
                    var quotationsFromDb = context.Quotations
                        .Include(q => q.Customer)
                        .ToList();

                    // Update any quotations with an empty or null Status to "Pending"
                    bool statusUpdated = false;
                    foreach (var quotation in quotationsFromDb)
                    {
                        if (string.IsNullOrEmpty(quotation.Status))
                        {
                            quotation.Status = "Pending";
                            statusUpdated = true;
                        }
                    }

                    // Save changes only if any status was updated
                    if (statusUpdated)
                    {
                        context.SaveChanges();
                    }

                    // Set the quotations to the ObservableCollection for the DataGrid
                    Quotations = new ObservableCollection<Quotation>(quotationsFromDb);
                    QuotationsDataGrid.ItemsSource = Quotations;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading quotations: {ex.Message}", "Error");
            }
        }

        
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int quotationId)
            {
                var detailPage = new QuotationDetailPage(quotationId);

                // Subscribe to the QuotationUpdated event to refresh the dashboard when returning
                detailPage.QuotationUpdated += () => LoadQuotations();

                NavigationService?.Navigate(detailPage);
            }
        }

    

        private void ActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                MessageBox.Show("ComboBox Selection Changed");

                if (selectedItem.Tag is int quotationId)
                {
                    string action = selectedItem.Content.ToString();

                    switch (action)
                    {
                        case "Accept":
                            AcceptQuotation(quotationId);
                            break;
                        case "Reject":
                            RejectQuotation(quotationId);
                            break;
                        case "Pending":
                            SetQuotationPending(quotationId);
                            break;
                    }
                }
            }
        }


        private void AcceptQuotation(int quotationId)
        {
            var quotation = Quotations.FirstOrDefault(q => q.Id == quotationId);
            if (quotation != null)
            {
                decimal discount = DetermineDiscount(quotation);
                quotation.Discount = discount;
                quotation.FinalAmount = quotation.TotalAmount * (1 - discount);
                UpdateQuotationStatus(quotationId, "Accepted");
            }
        }

        private void SetQuotationPending(int quotationId)
        {
            UpdateQuotationStatus(quotationId, "Pending");
        }

        private decimal DetermineDiscount(Quotation quotation)
        {
            return quotation.NatureOfJob.Contains("special") ? 0.1m : 0;
        }

        private void RejectQuotation(int quotationId)
        {
            string rejectionMessage = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the reason for rejecting this quotation:", "Rejection Reason", "Incomplete information");
            UpdateQuotationStatus(quotationId, "Rejected");
            NotifyCustomerOfRejection(quotationId, rejectionMessage);
        }

        private void UpdateQuotationStatus(int quotationId, string status)
        {
            var quotation = Quotations.FirstOrDefault(q => q.Id == quotationId);
            if (quotation != null)
            {
                quotation.Status = status;

                try
                {
                    using (var context = new AppDbContext())
                    {
                        var quotationToUpdate = context.Quotations.Find(quotationId);
                        if (quotationToUpdate != null)
                        {
                            quotationToUpdate.Status = status;
                            context.SaveChanges();

                            // Add a notification for the customer about the status change
                            var notification = new Notification
                            {
                                CustomerId = quotationToUpdate.CustomerId,
                                Message = $"Your quotation (ID: {quotationId}) status has been updated to: {status}.",
                                DateCreated = DateTime.Now,
                                IsRead = false
                            };
                            context.Notifications.Add(notification);
                            context.SaveChanges();
                        }
                    }
                    RefreshQuotationGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the quotation: {ex.Message}", "Error");
                }
            }
        }

        private void NotifyCustomerOfRejection(int quotationId, string message)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var quotation = context.Quotations.Include(q => q.Customer).FirstOrDefault(q => q.Id == quotationId);
                    if (quotation != null)
                    {
                        var notification = new Notification
                        {
                            CustomerId = quotation.CustomerId,
                            Message = $"Your quotation (ID: {quotationId}) was rejected. Reason: {message}",
                            DateCreated = DateTime.Now,
                            IsRead = false
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while notifying the customer: {ex.Message}", "Error");
            }
        }

        private void RefreshQuotationGrid()
        {
            LoadQuotations();
        }
        
        private void LoadNotifications()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var notifications = context.Notifications
                        .Where(n => !n.IsRead) // Filter for unread notifications if needed
                        .ToList();

                    // Clear the ListBox before loading
                    NotificationsListBox.Items.Clear();

                    // Display notifications in the ListBox
                    foreach (var notification in notifications)
                    {
                        NotificationsListBox.Items.Add(notification.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading notifications: {ex.Message}", "Error");
            }
        }

        private void MarkNotificationAsRead(int notificationId)
        {
            using (var context = new AppDbContext())
            {
                var notification = context.Notifications.FirstOrDefault(n => n.Id == notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                    context.SaveChanges();
                }
            }
        }


    }
}
