using System;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace InterportCargoWPF.Views;

public partial class QuotationDetailPage : Page
{
    private readonly int _quotationId;

    // Define the QuotationUpdated event
    public event Action QuotationUpdated;

    public QuotationDetailPage(int quotationId)
    {
        InitializeComponent();
        _quotationId = quotationId;
        LoadQuotationDetails();
    }

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

                // Calculate and display amounts
                CalculateAndDisplayAmounts(quotation);
            }
            else
            {
                MessageBox.Show("Quotation not found.");
                NavigationService.GoBack();
            }
        }
    }

    private void CalculateAndDisplayAmounts(Quotation quotation)
    {
        // Retrieve the base rates (hard-coded for demonstration)
        var baseRatePerContainer = quotation.CargoType == "20 Feet" ? 60.0m : 70.0m;
        var initialAmount = baseRatePerContainer * quotation.ContainerQuantity;

        // Apply discount based on criteria
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

        // Update the UI fields with calculated amounts
        InitialAmountTextBlock.Text = $"Initial Amount: {initialAmount:C}";
        FinalAmountTextBlock.Text = $"Final Amount: {finalAmount:C}";

        // Store the calculated values in the quotation object
        quotation.TotalAmount = initialAmount;
        quotation.Discount = discount;
        quotation.FinalAmount = finalAmount;

        // Save changes back to the database
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

    private void AcceptQuotation_Click(object sender, RoutedEventArgs e)
    {
        UpdateQuotationStatus("Accepted");
    }

    private void RejectQuotation_Click(object sender, RoutedEventArgs e)
    {
        var rejectionReason = Microsoft.VisualBasic.Interaction.InputBox("Enter the reason for rejection:",
            "Reject Quotation", "Incomplete information");
        UpdateQuotationStatus("Rejected", rejectionReason);
    }

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

                // Raise the QuotationUpdated event
                QuotationUpdated?.Invoke();
            }
        }

        MessageBox.Show($"Quotation {status} successfully.");
        NavigationService.GoBack();
    }

    private void ViewRateSchedule_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new RateScheduleViewPage());
    }
}