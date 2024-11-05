using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace InterportCargoWPF.Views;

public partial class EmployeeDashboardPage : Page
{
    public ObservableCollection<Quotation> Quotations { get; set; }

    public EmployeeDashboardPage()
    {
        InitializeComponent();
        LoadQuotations();
        LoadUnreadNotifications();
    }

    private void LoadQuotations()
    {
        try
        {
            using (var context = new AppDbContext())
            {
                var quotationsFromDb = context.Quotations
                    .Include(q => q.Customer)
                    .ToList();

                // Update quotations with missing status and save changes if any
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

    private void OpenButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int quotationId)
        {
            var detailPage = new QuotationDetailPage(quotationId);
            detailPage.QuotationUpdated += LoadQuotations;
            NavigationService?.Navigate(detailPage);
        }
    }

    private void ActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            if (selectedItem.Tag is int quotationId)
                HandleQuotationAction(selectedItem.Content.ToString(), quotationId);
    }

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
        var rejectionMessage = Microsoft.VisualBasic.Interaction.InputBox(
            "Enter the reason for rejecting this quotation:", "Rejection Reason", "Incomplete information");
        UpdateQuotationStatus(quotationId, "Rejected");
        NotifyCustomer(quotationId, $"Your quotation (ID: {quotationId}) was rejected. Reason: {rejectionMessage}");
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

                        NotifyCustomer(quotationId,
                            $"Your quotation (ID: {quotationId}) status has been updated to: {status}.");
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

    private void RefreshQuotationGrid()
    {
        LoadQuotations();
    }

    private void ShowErrorMessage(string message, Exception ex)
    {
        MessageBox.Show($"{message} Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void ViewQuotations_Click(object sender, RoutedEventArgs e)
    {
        // Add code to navigate or display quotations as needed
        MessageBox.Show("View Quotations button clicked.");
    }

    // Event handler for View Notifications button
    private void ViewNotifications_Click(object sender, RoutedEventArgs e)
    {
        // Add code to load or display notifications as needed
        MessageBox.Show("View Notifications button clicked.");
    }

    // Event handler for Outturn button
    private void OutturnButton_Click(object sender, RoutedEventArgs e)
    {
        // Add code to handle outturn functionality as needed
        MessageBox.Show("Outturn button clicked.");
    }

    // Event handler for Booking button
    private void BookingButton_Click(object sender, RoutedEventArgs e)
    {
        // Add code to handle booking functionality as needed
        MessageBox.Show("Booking button clicked.");
    }
}