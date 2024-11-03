using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Views;

namespace InterportCargoWPF.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void OpenEmployeeLoginPage_Click(object sender, RoutedEventArgs e)
        {
            // Show the Employee Login page within the Frame
            MainFrame.Visibility = Visibility.Visible;
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Navigate(new EmployeeLoginPage());

            // Do not alter the button visibility here; only handle it after successful login.
        }

        private void OpenRegisterWindow_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Visible;
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Navigate(new RegisterPage());
        }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string enteredPassword = PasswordBox.Password;

            using (var context = new AppDbContext())
            {
                // First, check if the customer exists in the database
                var customer = context.Customers.FirstOrDefault(c => c.Email == email);

                if (customer != null)
                {
                    // Verify the entered password against the stored hashed password
                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(enteredPassword, customer.Password);

                    if (isPasswordCorrect)
                    {
                        MessageBox.Show($"Login successful! Welcome, {customer.FirstName}.");
                        
                        // Set the LoggedInCustomerId in SessionManager
                        SessionManager.LoggedInCustomerId = customer.Id;

                        // Navigate to the landing page on successful login
                        LoginSuccessful();
                    }
                    else
                    {
                        MessageBox.Show("Invalid password.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email or password.");
                }
            }
        }

        public void LoginSuccessful()
        {
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new LandingPage());

            // Show Back and Logout buttons, hide Employee Login button after successful login
            EmployeeLoginButton.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Visible;
            LogoutButton.Visibility = Visibility.Visible;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Go back to the previous page in the Frame, if possible
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
            else
            {
                // If there's no page to go back to, reset to the login form
                MainFrame.Visibility = Visibility.Collapsed;
                LoginForm.Visibility = Visibility.Visible;
                EmployeeLoginButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Collapsed;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear session or perform any logout-related operations
            MessageBox.Show("You have been logged out.");

            // Reset to the login form and update button visibility
            LoginForm.Visibility = Visibility.Visible;
            MainFrame.Visibility = Visibility.Collapsed;

            EmployeeLoginButton.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Collapsed;
            LogoutButton.Visibility = Visibility.Collapsed;
        }
    }
}
