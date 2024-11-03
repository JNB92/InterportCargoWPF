using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;

namespace InterportCargoWPF.Views
{
    public partial class QuotationPage : Page
    {
        private int _loggedInCustomerId;
        

        public QuotationPage(int loggedInCustomerId) // Pass logged-in customer ID
        {
            InitializeComponent();
            _loggedInCustomerId = loggedInCustomerId; // Store the customer ID for later use
        }
        private void ViewRateSchedule_Click(object sender, RoutedEventArgs e)
        {
            // Open RateScheduleViewPage for reference
            NavigationService?.Navigate(new RateScheduleViewPage());
        }

        private void SubmitQuotation_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve inputs
            string origin = OriginBox.Text;
            string destination = DestinationBox.Text;
            string cargoType = CargoTypeBox.Text;

            // Get the selected container quantity from ComboBox
            var selectedComboBoxItem = ContainerQuantityComboBox.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem == null)
            {
                MessageBox.Show("Please select the container quantity.");
                return;
            }

            // Extract the content from ComboBoxItem and convert it to an integer
            int containerQuantity = int.Parse(selectedComboBoxItem.Content.ToString());

            string additionalRequirements = AdditionalRequirementsBox.Text;
            var selectedDate = TransportationDatePicker.SelectedDate;

            // Validate required fields
            if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination) ||
                string.IsNullOrWhiteSpace(cargoType) || selectedDate == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Validate that a valid customer ID is available
            if (_loggedInCustomerId <= 0)
            {
                MessageBox.Show("Customer ID is not valid. Please log in again.");
                return;
            }

            // Create a new Quotation object
            var newQuotation = new Quotation
            {
                Origin = origin,
                Destination = destination,
                CargoType = cargoType,
                ContainerQuantity = containerQuantity,
                NatureOfJob = additionalRequirements,
                TransportationDate = selectedDate.Value,
                CustomerId = _loggedInCustomerId, // Assign the logged-in customer's ID
                Status = "Pending" // Set initial status to Pending
            };


            // Save the quotation to the database
            using (var context = new AppDbContext())
            {
                // Verify if the customer exists
                var customerExists = context.Customers.Any(c => c.Id == _loggedInCustomerId);
                if (!customerExists)
                {
                    MessageBox.Show("Selected customer does not exist. Please log in with a valid account.");
                    return;
                }

                context.Quotations.Add(newQuotation);
                context.SaveChanges();
            }

            // Confirm submission
            MessageBox.Show("Quotation successfully submitted!");

            // Navigate back to the main page or another target page
            NavigationService?.Navigate(new LandingPage());
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
