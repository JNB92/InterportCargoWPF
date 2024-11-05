using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace InterportCargoWPF.Views;

/// <summary>
///     Represents the details page for a specific quotation, providing functionalities to view, accept, and reject
///     quotations.
/// </summary>
public partial class QuotationDetailPage : Page
{
    private readonly int _quotationId;

    /// <summary>
    ///     Initializes a new instance of the <see cref="QuotationDetailPage" /> class.
    /// </summary>
    /// <param name="quotationId">The ID of the quotation to display details for.</param>
    public QuotationDetailPage(int quotationId)
    {
        InitializeComponent();
        _quotationId = quotationId;
        LoadQuotationDetails();
    }

    /// <summary>
    ///     Event that is raised when a quotation is updated (e.g., accepted or rejected).
    /// </summary>
    public event Action QuotationUpdated;

    /// <summary>
    ///     Loads the details of the specified quotation from the database and populates the UI.
    /// </summary>
    private void LoadQuotationDetails()
    {
        using (var context = new AppDbContext())
        {
            var quotation = context.Quotations
                .Include(q => q.Customer)
                .FirstOrDefault(q => q.Id == _quotationId);

            if (quotation != null)
            {
                // Populate UI with quotation details
                QuotationIdTextBlock.Text = quotation.Id.ToString();
                CustomerNameTextBlock.Text = quotation.Customer?.FullName ?? "N/A";
                OriginTextBlock.Text = quotation.Origin;
                DestinationTextBlock.Text = quotation.Destination;
                CargoTypeTextBlock.Text = quotation.CargoType;
                ContainerQuantityTextBlock.Text = quotation.ContainerQuantity.ToString();
                TransportationDateTextBlock.Text = quotation.TransportationDate.ToShortDateString();
                NatureOfJobTextBlock.Text = quotation.NatureOfJob;
                AdditionalRequirementsTextBlock.Text = quotation.AdditionalRequirements ?? string.Empty;

                // Calculate and display quotation amounts
                CalculateAndDisplayAmounts(quotation);
            }
            else
            {
                MessageBox.Show("Quotation not found.");
                NavigationService.GoBack();
            }
        }
    }

    /// <summary>
    ///     Calculates the initial and final amounts for the quotation based on its details.
    /// </summary>
    /// <param name="quotation">The quotation for which to calculate amounts.</param>
    private void CalculateAndDisplayAmounts(Quotation quotation)
    {
        // Define base rates (hard-coded for demonstration)
        var baseRatePerContainer = quotation.CargoType == "20 Feet" ? 60.0m : 70.0m;
        var initialAmount = baseRatePerContainer * quotation.ContainerQuantity;

        // Apply discount based on conditions
        decimal discount = 0;
        if ((quotation.ContainerQuantity > 5 && quotation.NatureOfJob.Contains("Quarantine")) ||
            quotation.NatureOfJob.Contains("Fumigation"))
        {
            discount = 0.025m; // 2.5% discount
            if (quotation.ContainerQuantity > 10) discount = 0.10m; // 10% discount for over 10 containers
        }
        else if (quotation.ContainerQuantity > 5)
        {
            discount = 0.05m; // 5% discount
        }

        var finalAmount = initialAmount * (1 - discount);

        // Update the UI with calculated amounts
        InitialAmountTextBlock.Text = $"Initial Amount: {initialAmount:C}";
        FinalAmountTextBlock.Text = $"Final Amount: {finalAmount:C}";

        // Save calculated values to the database
        using (var context = new AppDbContext())
        {
            var quotationToUpdate = context.Quotations.FirstOrDefault(q => q.Id == _quotationId);
            if (quotationToUpdate != null)
            {
                quotationToUpdate.TotalAmount = initialAmount;
                quotationToUpdate.Discount = discount;
                quotationToUpdate.FinalAmount = finalAmount;
                context.SaveChanges();
            }
        }
    }

    /// <summary>
    ///     Event handler for accepting the quotation.
    /// </summary>
    private void AcceptQuotation_Click(object sender, RoutedEventArgs e)
    {
        UpdateQuotationStatus("Accepted");
    }

    /// <summary>
    ///     Event handler for rejecting the quotation, prompting for a rejection reason.
    /// </summary>
    private void RejectQuotation_Click(object sender, RoutedEventArgs e)
    {
        var rejectionReason = Interaction.InputBox("Enter the reason for rejection:", "Reject Quotation",
            "Incomplete information");
        UpdateQuotationStatus("Rejected", rejectionReason);
    }

    /// <summary>
    ///     Updates the status of the quotation and notifies the customer of the change.
    /// </summary>
    /// <param name="status">The new status for the quotation (e.g., "Accepted" or "Rejected").</param>
    /// <param name="rejectionReason">The reason for rejection, if applicable.</param>
    private void UpdateQuotationStatus(string status, string rejectionReason = null)
    {
        using (var context = new AppDbContext())
        {
            var quotation = context.Quotations.FirstOrDefault(q => q.Id == _quotationId);
            if (quotation != null)
            {
                quotation.Status = status;
                context.SaveChanges();

                // Notify customer of the status change
                var notificationMessage = status == "Rejected"
                    ? $"Your quotation was rejected. Reason: {rejectionReason}"
                    : "Your quotation has been accepted.";

                var notification = new Notification
                {
                    CustomerId = quotation.CustomerId,
                    Message = notificationMessage,
                    DateCreated = DateTime.Now,
                    IsRead = false
                };
                context.Notifications.Add(notification);
                context.SaveChanges();

                // Trigger the QuotationUpdated event
                QuotationUpdated?.Invoke();
            }
        }

        MessageBox.Show($"Quotation {status} successfully.");
        NavigationService.GoBack();
    }

    /// <summary>
    ///     Navigates to the rate schedule page, allowing the user to view rate information.
    /// </summary>
    private void ViewRateSchedule_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new RateScheduleViewPage());
    }
}