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
        // Define Quotations as a class-level ObservableCollection to support data binding
        public ObservableCollection<Quotation> Quotations { get; set; }

        public EmployeeDashboardPage()
        {
            InitializeComponent();
            LoadQuotations();
        }

        private void LoadQuotations()
        {
            using (var context = new AppDbContext())
            {
                // Load quotations from the database, including related Customer information
                var quotationsFromDb = context.Quotations
                    .Include(q => q.Customer) // Include the related Customer entity
                    .ToList();

                // Initialize Quotations collection
                Quotations = new ObservableCollection<Quotation>(quotationsFromDb);

                // Bind the collection to the DataGrid
                QuotationsDataGrid.ItemsSource = Quotations;
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int quotationId)
            {
                // Handle accept action - update status to "Accepted"
                UpdateQuotationStatus(quotationId, "Accepted");
            }
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int quotationId)
            {
                // Handle reject action - update status to "Rejected"
                UpdateQuotationStatus(quotationId, "Rejected");
            }
        }

        // Method to update the status of the quotation
        private void UpdateQuotationStatus(int quotationId, string status)
        {
            // Find and update the status in the Quotations collection
            var quotation = Quotations.FirstOrDefault(q => q.Id == quotationId);
            if (quotation != null)
            {
                quotation.Status = status;

                // Save changes to the database
                using (var context = new AppDbContext())
                {
                    var quotationToUpdate = context.Quotations.Find(quotationId);
                    if (quotationToUpdate != null)
                    {
                        quotationToUpdate.Status = status;
                        context.SaveChanges();
                    }
                }

                RefreshQuotationGrid(); // Refresh the DataGrid to show the updated status
            }
        }

        private void RefreshQuotationGrid()
        {
            QuotationsDataGrid.ItemsSource = null;
            QuotationsDataGrid.ItemsSource = Quotations; // Reassign the data source to refresh
        }
    }
}
