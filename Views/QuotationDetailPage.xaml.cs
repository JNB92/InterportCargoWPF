using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace InterportCargoWPF.Views
{
    public partial class QuotationDetailPage : Page
    {
        private readonly int _quotationId;

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
                    QuotationIdTextBlock.Text = quotation.Id.ToString();
                    CustomerNameTextBlock.Text = quotation.Customer.FullName;
                    OriginTextBlock.Text = quotation.Origin;
                    DestinationTextBlock.Text = quotation.Destination;
                    CargoTypeTextBlock.Text = quotation.CargoType;
                    ContainerQuantityTextBlock.Text = quotation.ContainerQuantity.ToString();
                    TransportationDateTextBlock.Text = quotation.TransportationDate.ToShortDateString();
                    NatureOfJobTextBlock.Text = quotation.NatureOfJob;
                    AdditionalRequirementsTextBlock.Text = quotation.AdditionalRequirements;
                }
                else
                {
                    MessageBox.Show("Quotation not found.");
                    NavigationService.GoBack();
                }
            }
        }

        private void ViewRateSchedule_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RateScheduleViewPage());
        }

        private void AcceptQuotation_Click(object sender, RoutedEventArgs e)
        {
            UpdateQuotationStatus("Accepted");
        }

        private void RejectQuotation_Click(object sender, RoutedEventArgs e)
        {
            string rejectionReason = Microsoft.VisualBasic.Interaction.InputBox("Enter the reason for rejection:",
                "Reject Quotation", "Incomplete information");
            UpdateQuotationStatus("Rejected", rejectionReason);
        }

        public event Action QuotationUpdated;

        private void UpdateQuotationStatus(string status, string rejectionReason = null)
        {
            using (var context = new AppDbContext())
            {
                var quotation = context.Quotations.FirstOrDefault(q => q.Id == _quotationId);
                if (quotation != null)
                {
                    quotation.Status = status;

                    if (status == "Accepted" && ApplyDiscountCheckBox.IsChecked == true)
                    {
                        quotation.Discount = DetermineDiscount(quotation);
                        quotation.FinalAmount = quotation.TotalAmount * (1 - quotation.Discount);
                    }

                    context.SaveChanges();

                    // Notify customer of status change
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
                }
            }

            MessageBox.Show($"Quotation {status} successfully.");

            // Raise the event to signal that the quotation has been updated
            QuotationUpdated?.Invoke();

            NavigationService.GoBack();
        }
        
        private decimal DetermineDiscount(Quotation quotation)
        {
            // Implement discount logic based on criteria.
            // For example, apply a 10% discount if the nature of the job contains the word "special"
            return quotation.NatureOfJob.Contains("special") ? 0.1m : 0;
        }

    }
    
    
}
