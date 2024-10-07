using System.Windows;
using System.Windows.Controls;

namespace InterportCargoWPF.Views
{
    public partial class LandingPage
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        private void GoToQuotation_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Quotation window or page
            var quotationWindow = new QuotationWindow();
            quotationWindow.Show();
            var mainWindow = MainWindow.Instance;
            mainWindow.Hide();
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