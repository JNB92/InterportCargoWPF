using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;

namespace InterportCargoWPF.Views;

public partial class QuotationPage : Page
{
    private int _loggedInCustomerId;

    public QuotationPage(int loggedInCustomerId)
    {
        InitializeComponent();
        _loggedInCustomerId = loggedInCustomerId;
        GenerateQuotationId();
    }

    private void GenerateQuotationId()
    {
        using (var context = new AppDbContext())
        {
            var newQuotationId = context.Quotations.Max(q => (int?)q.Id) + 1 ?? 1;
            QuotationIdTextBlock.Text = newQuotationId.ToString();
        }
    }

    private void SubmitQuotation_Click(object sender, RoutedEventArgs e)
    {
        // Retrieve inputs and validate required fields
        var origin = OriginBox.Text;
        var destination = DestinationBox.Text;
        var cargoType = CargoTypeBox.Text;

        var selectedQuantityItem = ContainerQuantityComboBox.SelectedItem as ComboBoxItem;
        if (selectedQuantityItem == null)
        {
            MessageBox.Show("Please select the container quantity.");
            return;
        }

        int containerQuantity;
        if (!int.TryParse(selectedQuantityItem.Content.ToString(), out containerQuantity))
        {
            MessageBox.Show("Invalid container quantity. Please select a valid number.");
            return;
        }

        var selectedDate = TransportationDatePicker.SelectedDate;
        var natureOfJob = (NatureOfJobComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination) ||
            string.IsNullOrWhiteSpace(cargoType) || selectedDate == null || string.IsNullOrWhiteSpace(natureOfJob))
        {
            MessageBox.Show("Please fill in all required fields.");
            return;
        }

        if (_loggedInCustomerId <= 0)
        {
            MessageBox.Show("Customer ID is not valid. Please log in again.");
            return;
        }

        // Create a new quotation with the user-provided data
        var newQuotation = new Quotation
        {
            Origin = origin,
            Destination = destination,
            CargoType = cargoType,
            ContainerQuantity = containerQuantity,
            NatureOfJob = natureOfJob,
            TransportationDate = selectedDate.Value,
            CustomerId = _loggedInCustomerId,
            Status = "Pending"
        };

        // Calculate initial amount and apply discounts
        newQuotation.CalculateInitialAmount();
        newQuotation.ApplyDiscount();

        // Display the calculated values to the user
        MessageBox.Show($"Initial Amount: {newQuotation.TotalAmount:C}\n" +
                        $"Discount: {newQuotation.Discount * 100}%\n" +
                        $"Final Amount: {newQuotation.FinalAmount:C}", "Quotation Summary");

        // Save the quotation to the database
        using (var context = new AppDbContext())
        {
            context.Quotations.Add(newQuotation);
            context.SaveChanges();

            // Create a notification for the Quotation officer
            var notification = new Notification
            {
                CustomerId = _loggedInCustomerId,
                Message = $"New quotation request submitted by Customer ID: {_loggedInCustomerId}.",
                DateCreated = DateTime.Now,
                IsRead = false
            };
            context.Notifications.Add(notification);
            context.SaveChanges();
        }

        MessageBox.Show("Quotation successfully submitted!");
        MainWindow.Instance?.MainFrame.GoBack();
    }

    private void OnDateSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedDate = ((DatePicker)sender).SelectedDate;
        if (selectedDate.HasValue) MessageBox.Show($"Selected date: {selectedDate.Value.ToShortDateString()}");
    }
}