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

                    foreach (var quotation in quotationsFromDb)
                    {
                        if (string.IsNullOrEmpty(quotation.Status))
                        {
                            quotation.Status = "Pending";
                            context.SaveChanges();
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

        private void ActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
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
    }
}
