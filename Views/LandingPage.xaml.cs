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

            var mainWindow = MainWindow.Instance;
            mainWindow.MainFrame.Visibility = Visibility.Visible;
            mainWindow.MainFrame.Navigate(new CustomerQuotationsPage(loggedInCustomerId));
        }



        private void GoToQuotation_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the logged-in customer's ID from SessionManager
            int loggedInCustomerId = SessionManager.LoggedInCustomerId;

            // Navigate to the QuotationPage using the MainFrame in MainWindow
            var mainWindow = MainWindow.Instance;

            // Set MainFrame's visibility if it's not already visible
            mainWindow.MainFrame.Visibility = Visibility.Visible;

            // Navigate to QuotationPage within the MainFrame, passing the loggedInCustomerId
            mainWindow.MainFrame.Navigate(new QuotationPage(loggedInCustomerId));

            // Optionally, hide the current content if needed
            this.Visibility = Visibility.Collapsed;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Ensure the current application is referenced properly
            var mainWindow = MainWindow.Instance;

            // Hide the frame and show the login form
            mainWindow.MainFrame.Visibility = Visibility.Collapsed;
            mainWindow.LoginForm.Visibility = Visibility.Visible;

            // Optionally, clear the login fields
            mainWindow.EmailBox.Text = string.Empty;
            mainWindow.PasswordBox.Password = string.Empty;
        }
    }
}