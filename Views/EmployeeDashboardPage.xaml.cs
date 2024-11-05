using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace InterportCargoWPF.Views
{
    public partial class EmployeeDashboardPage : Page
    {
        public ObservableCollection<Quotation> Quotations { get; set; }

        public EmployeeDashboardPage()
        {
            InitializeComponent();
            LoadQuotations();
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

                    // Set status to pending if it hasn't been assigned
                    foreach (var quotation in quotationsFromDb)
                    {
                        if (string.IsNullOrEmpty(quotation.Status))
                        {
                            quotation.Status = "Pending";
                            context.SaveChanges(); // Update database with the initial status
                        }
                    }

                    Quotations = new ObservableCollection<Quotation>(quotationsFromDb);
                    QuotationsDataGrid.ItemsSource = Quotations;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading quotations: {ex.Message}", "Error");
            }
        }


        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int quotationId)
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
        }
        
        private void PendingButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int quotationId)
            {
                // Set the status of the quotation to "Pending"
                UpdateQuotationStatus(quotationId, "Pending");
            }
        }

        private decimal DetermineDiscount(Quotation quotation)
        {
            // Implement your discount logic here. For example:
            if (quotation.NatureOfJob.Contains("special"))
            {
                return 0.1m; // 10% discount for specific job types
            }
            return 0;
        }
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int quotationId)
            {
                string rejectionMessage = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter the reason for rejecting this quotation:", "Rejection Reason", "Incomplete information");

                UpdateQuotationStatus(quotationId, "Rejected");
                NotifyCustomerOfRejection(quotationId, rejectionMessage);
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

                            var notification = new Notification
                            {
                                CustomerId = quotationToUpdate.CustomerId,
                                Message = $"Your quotation with ID {quotationId} has been {status.ToLower()} by the Quotation Officer.",
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

        private void GenerateQuotationForCustomer(int quotationId)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var quotation = context.Quotations.Include(q => q.Customer).FirstOrDefault(q => q.Id == quotationId);
                    if (quotation != null)
                    {
                        decimal discount = ApplyDiscountIfApplicable(quotation);
                        quotation.Discount = discount;
                        quotation.FinalAmount = quotation.TotalAmount - discount;

                        context.SaveChanges();

                        MessageBox.Show($"Quotation generated with a discount of {discount:C}", "Quotation Finalized");

                        var notification = new Notification
                        {
                            CustomerId = quotation.CustomerId,
                            Message = $"Your quotation (ID: {quotationId}) is ready for review. Total: {quotation.FinalAmount:C}",
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
                MessageBox.Show($"An error occurred while generating the quotation: {ex.Message}", "Error");
            }
        }
        private decimal CalculateQuotationAmount(Quotation quotation, string containerType)
        {
            decimal subtotal = 0;

            try
            {
                using (var context = new AppDbContext())
                {
                    var rateSchedule = context.RateSchedules.FirstOrDefault(r => r.ContainerType == containerType);
                    if (rateSchedule == null)
                    {
                        MessageBox.Show($"Rate schedule not found for container type: {containerType}", "Error");
                        return 0;
                    }

                    subtotal += rateSchedule.WharfBookingFee;
                    subtotal += rateSchedule.LiftOnLiftOffFee;
                    subtotal += rateSchedule.FumigationFee;
                    subtotal += rateSchedule.LclDeliveryDepot;
                    subtotal += rateSchedule.TailgateInspectionFee;
                    subtotal += rateSchedule.StorageFee;
                    subtotal += rateSchedule.FacilityFee;
                    subtotal += rateSchedule.WharfInspectionFee;

                    // Apply GST
                    decimal gstAmount = subtotal * rateSchedule.GstRate;
                    decimal totalWithGst = subtotal + gstAmount;

                    // Apply discount if applicable
                    decimal discount = ApplyDiscountIfApplicable(quotation);
                    decimal finalAmount = totalWithGst - discount;

                    quotation.TotalAmount = subtotal;
                    quotation.FinalAmount = finalAmount;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while calculating the quotation: {ex.Message}", "Error");
            }

            return subtotal;
        }

        private decimal ApplyDiscountIfApplicable(Quotation quotation)
        {
            decimal discount = 0;
            if (quotation.ContainerQuantity >= 5) // Example discount criterion
            {
                MessageBoxResult result = MessageBox.Show("Apply a 10% discount?", "Discount Available", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    discount = quotation.TotalAmount * 0.10m; // Apply 10% discount
                }
            }
            return discount;
        }

        private void RefreshQuotationGrid()
        {
            LoadQuotations(); // Reload quotations to reflect any updates
        }
    }
}
