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
            // Navigate to the QuotationPage using the MainFrame in MainWindow
            var mainWindow = MainWindow.Instance;

            // Set MainFrame's visibility if it's not already visible
            mainWindow.MainFrame.Visibility = Visibility.Visible;

            // Navigate to QuotationPage within the MainFrame
            mainWindow.MainFrame.Navigate(new QuotationPage());

            // Optionally, hide the current content if needed
            // (e.g., hiding a login form or other initial content)
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