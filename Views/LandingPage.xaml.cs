using System.Windows;
using System.Windows.Controls;

namespace InterportCargoWPF.Views
{
    public partial class LandingPage : Page
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        private void ViewMyQuotations_Click(object sender, RoutedEventArgs e)
        {
            int loggedInCustomerId = SessionManager.LoggedInCustomerId;

            if (loggedInCustomerId <= 0)
            {
                MessageBox.Show("Customer ID is not valid. Please log in again.");
                return;
            }

            // Navigate to CustomerQuotationsPage within the ContentFrame
            ContentFrame.Navigate(new CustomerQuotationsPage(loggedInCustomerId));
        }

        private void GoToQuotation_Click(object sender, RoutedEventArgs e)
        {
            int loggedInCustomerId = SessionManager.LoggedInCustomerId;

            // Navigate to QuotationPage within the ContentFrame
            ContentFrame.Navigate(new QuotationPage(loggedInCustomerId));
        }
        
        private void OutturnButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OutturnPage());
        }

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BookingPage());
        }

    }
}