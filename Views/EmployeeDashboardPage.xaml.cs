using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace InterportCargoWPF.Views
{
    /// <summary>
    /// Represents the dashboard page for employees, allowing them to view and manage quotations and notifications.
    /// </summary>
    public partial class EmployeeDashboardPage : Page
    {
        /// <summary>
        /// Gets or sets the collection of quotations displayed on the dashboard.
        /// </summary>
        public ObservableCollection<Quotation> Quotations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeDashboardPage"/> class.
        /// </summary>
        public EmployeeDashboardPage()
        {
            InitializeComponent();
            LoadQuotations();
            LoadUnreadNotifications();
        }

        /// <summary>
        /// Loads all quotations from the database and populates the QuotationsDataGrid.
        /// </summary>
        private void LoadQuotations()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var quotationsFromDb = context.Quotations
                        .Include(q => q.Customer)
                        .ToList();

                    UpdateQuotationStatusIfNeeded(quotationsFromDb, context);

                    Quotations = new ObservableCollection<Quotation>(quotationsFromDb);
                    QuotationsDataGrid.ItemsSource = Quotations;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("An error occurred while loading quotations.", ex);
            }
        }

        /// <summary>
        /// Sets a default "Pending" status for quotations without a status, if necessary, and saves changes.
        /// </summary>
        /// <param name="quotations">The list of quotations to check and update.</param>
        /// <param name="context">The database context to use for saving changes.</param>
        private void UpdateQuotationStatusIfNeeded(IList<Quotation> quotations, AppDbContext context)
        {
            var statusUpdated = false;
            foreach (var quotation in quotations.Where(q => string.IsNullOrEmpty(q.Status)))
            {
                quotation.Status = "Pending";
                statusUpdated = true;
            }

            if (statusUpdated) context.SaveChanges();
        }

        /// <summary>
        /// Loads unread notifications from the database and marks them as read.
        /// </summary>
        private void LoadUnreadNotifications()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var notifications = context.Notifications
                        .Where(n => !n.IsRead)
                        .OrderByDescending(n => n.DateCreated)
                        .ToList();

                    NotificationsListBox.Items.Clear();
                    foreach (var notification in notifications)
                    {
                        NotificationsListBox.Items.Add(notification.Message);
                        MarkNotificationAsRead(notification.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("An error occurred while loading notifications.", ex);
            }
        }

        /// <summary>
        /// Marks a notification as read in the database.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to mark as read.</param>
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

        /// <summary>
        /// Opens the details page for the selected quotation.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Event data.</param>
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int quotationId)
            {
                var detailPage = new QuotationDetailPage(quotationId);
                detailPage.QuotationUpdated += LoadQuotations;
                NavigationService?.Navigate(detailPage);
            }
        }

        /// <summary>
        /// Handles selection changes in the ActionComboBox to perform actions on a quotation.
        /// </summary>
        private void ActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
                if (selectedItem.Tag is int quotationId)
                    HandleQuotationAction(selectedItem.Content.ToString(), quotationId);
        }

        /// <summary>
        /// Executes the specified action on a quotation.
        /// </summary>
        /// <param name="action">The action to perform (e.g., "Accept", "Reject", "Pending").</param>
        /// <param name="quotationId">The ID of the quotation to update.</param>
        private void HandleQuotationAction(string action, int quotationId)
        {
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

        /// <summary>
        /// Accepts a quotation, applies a discount if applicable, and updates the status.
        /// </summary>
        private void AcceptQuotation(int quotationId)
        {
            var quotation = Quotations.FirstOrDefault(q => q.Id == quotationId);
            if (quotation != null)
            {
                var discount = DetermineDiscount(quotation);
                quotation.Discount = discount;
                quotation.FinalAmount = quotation.TotalAmount * (1 - discount);
                UpdateQuotationStatus(quotationId, "Accepted");
            }
        }

        /// <summary>
        /// Sets a quotation's status to "Pending".
        /// </summary>
        private void SetQuotationPending(int quotationId)
        {
            UpdateQuotationStatus(quotationId, "Pending");
        }

        /// <summary>
        /// Determines an applicable discount for the quotation based on its details.
        /// </summary>
        /// <param name="quotation">The quotation for which to determine a discount.</param>
        /// <returns>The discount rate as a decimal.</returns>
        private decimal DetermineDiscount(Quotation quotation)
        {
            return quotation.NatureOfJob.Contains("special") ? 0.1m : 0;
        }

        /// <summary>
        /// Rejects a quotation, captures a rejection reason, and notifies the customer.
        /// </summary>
        private void RejectQuotation(int quotationId)
        {
            var rejectionMessage = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the reason for rejecting this quotation:", "Rejection Reason", "Incomplete information");
            UpdateQuotationStatus(quotationId, "Rejected");
            NotifyCustomer(quotationId, $"Your quotation (ID: {quotationId}) was rejected. Reason: {rejectionMessage}");
        }

        /// <summary>
        /// Updates the status of a quotation in the database and notifies the customer.
        /// </summary>
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

                            NotifyCustomer(quotationId, $"Your quotation (ID: {quotationId}) status has been updated to: {status}.");
                        }
                    }

                    RefreshQuotationGrid();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("An error occurred while updating the quotation.", ex);
                }
            }
        }

        /// <summary>
        /// Sends a notification to the customer about the quotation status update.
        /// </summary>
        private void NotifyCustomer(int quotationId, string message)
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
                            Message = message,
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
                ShowErrorMessage("An error occurred while notifying the customer.", ex);
            }
        }

        /// <summary>
        /// Refreshes the QuotationsDataGrid to reflect the latest data.
        /// </summary>
        private void RefreshQuotationGrid()
        {
            LoadQuotations();
        }

        /// <summary>
        /// Displays an error message to the user with specific details.
        /// </summary>
        private void ShowErrorMessage(string message, Exception ex)
        {
            MessageBox.Show($"{message} Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OutturnButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Outturn button clicked.");
        }

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Booking button clicked.");
        }
    }
}
