using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;

namespace InterportCargoWPF.Views
{
    public partial class QuotationPage : Page
    {
        private int _loggedInCustomerId;

        public QuotationPage(int loggedInCustomerId)
        {
            InitializeComponent();
            _loggedInCustomerId = loggedInCustomerId;
            GenerateQuotationId(); // Auto-generate and display the Quotation ID
        }

        private void GenerateQuotationId()
        {
            using (var context = new AppDbContext())
            {
                int newQuotationId = context.Quotations.Max(q => (int?)q.Id) + 1 ?? 1;
                QuotationIdTextBlock.Text = newQuotationId.ToString();
            }
        }

        private void ViewRateSchedule_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RateScheduleViewPage());
        }

        private void SubmitQuotation_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve inputs
            string origin = OriginBox.Text;
            string destination = DestinationBox.Text;
            string cargoType = CargoTypeBox.Text;
            var selectedComboBoxItem = ContainerQuantityComboBox.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem == null)
            {
                MessageBox.Show("Please select the container quantity.");
                return;
            }

            int containerQuantity = int.Parse(selectedComboBoxItem.Content.ToString());
            string natureOfJob = NatureOfJobComboBox.SelectedItem?.ToString();
            var selectedDate = TransportationDatePicker.SelectedDate;

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

            using (var context = new AppDbContext())
            {
                var customerExists = context.Customers.Any(c => c.Id == _loggedInCustomerId);
                if (!customerExists)
                {
                    MessageBox.Show("Selected customer does not exist. Please log in with a valid account.");
                    return;
                }

                context.Quotations.Add(newQuotation);
                context.SaveChanges();

                // Create a notification for the quotation officer
                var notification = new Notification
                {
                    CustomerId = _loggedInCustomerId,
                    Message = $"New quotation request submitted by Customer ID: {_loggedInCustomerId}.",
                    DateCreated = DateTime.Now
                };
                context.Notifications.Add(notification);
                context.SaveChanges();
            }

            MessageBox.Show("Quotation successfully submitted!");
            MainWindow.Instance.MainFrame.GoBack();
        }


        private void OnDateSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = ((DatePicker)sender).SelectedDate;
            if (selectedDate.HasValue)
            {
                MessageBox.Show($"Selected date: {selectedDate.Value.ToShortDateString()}");
            }
        }
    }
}
