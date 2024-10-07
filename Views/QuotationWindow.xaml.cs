using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace InterportCargoWPF.Views
{
    /// <summary>
    /// Interaction logic for QuotationWindow.xaml
    /// </summary>
    public partial class QuotationWindow : Window
    {
        public QuotationWindow()
        {
            InitializeComponent();
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

            // Process transportation date
            string formattedDate = selectedDate.Value.ToShortDateString();

            // Simulate submitting the quotation
            MessageBox.Show(
                $"Quotation submitted:\nSource: {origin}\nDestination: {destination}\nCargo Type: {cargoType}\nContainers: {containerQuantity}\nDate: {formattedDate}\nAdditional Requirements: {additionalRequirements}");

            // Get the main window instance
            var mainWindow = MainWindow.Instance;

            mainWindow.Show();
            this.Close();
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
