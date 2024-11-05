using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace InterportCargoWPF.Views;

/// <summary>
///     A page that displays and allows customers to respond to their quotations.
/// </summary>
public partial class CustomerQuotationsPage : Page
{
    private readonly int _customerId;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CustomerQuotationsPage" /> class with the specified customer ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer whose quotations are to be displayed.</param>
    public CustomerQuotationsPage(int customerId)
    {
        InitializeComponent();
        _customerId = customerId;
        LoadQuotations();
    }

    /// <summary>
    ///     Gets or sets the collection of quotations associated with the current customer.
    /// </summary>
    public ObservableCollection<Quotation> Quotations { get; set; }

    /// <summary>
    ///     Loads quotations associated with the current customer from the database.
    /// </summary>
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

    /// <summary>
    ///     Handles the Respond button click event to allow the customer to accept or reject a quotation.
    /// </summary>
    /// <param name="sender">The button that was clicked.</param>
    /// <param name="e">The event data.</param>
    private void RespondButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int quotationId)
        {
            var result = MessageBox.Show("Do you want to accept this quotation?",
                "Respond to Quotation", MessageBoxButton.YesNo);

            var status = result == MessageBoxResult.Yes ? "Accepted by Customer" : "Rejected by Customer";
            UpdateCustomerQuotationStatus(quotationId, status);
        }
    }

    /// <summary>
    ///     Updates the status of a customer's quotation based on their response.
    /// </summary>
    /// <param name="quotationId">The ID of the quotation to update.</param>
    /// <param name="status">The new status for the quotation.</param>
    private void UpdateCustomerQuotationStatus(int quotationId, string status)
    {
        using (var context = new AppDbContext())
        {
            var quotation = context.Quotations.FirstOrDefault(q => q.Id == quotationId);
            if (quotation != null)
            {
                quotation.Status = status;
                context.SaveChanges();
                NotifyQuotationOfficer(quotationId, status);
                LoadQuotations(); // Refresh the quotations grid to reflect the new status
            }
        }
    }

    /// <summary>
    ///     Notifies the quotation officer of the customer's response to the quotation.
    /// </summary>
    /// <param name="quotationId">The ID of the quotation responded to.</param>
    /// <param name="status">The status of the quotation after the customer's response.</param>
    private void NotifyQuotationOfficer(int quotationId, string status)
    {
        using (var context = new AppDbContext())
        {
            var quotation = context.Quotations.Include(q => q.Customer).FirstOrDefault(q => q.Id == quotationId);
            if (quotation != null)
            {
                var notification = new Notification
                {
                    CustomerId = quotation.CustomerId,
                    Message =
                        $"Customer {quotation.Customer.FullName} has {status.ToLower()} the quotation with ID {quotationId}.",
                    DateCreated = DateTime.Now,
                    IsRead = false
                };
                context.Notifications.Add(notification);
                context.SaveChanges();
            }
        }
    }
}